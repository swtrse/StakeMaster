using System;
using System.Collections.Generic;

namespace StakeMaster.BusinessLogic
{
	using System.Linq;
	using System.Threading.Tasks;
	using DataAccess;
	using DataAccess.RpcResponseTypes;
	using JetBrains.Annotations;
	using Rpc;
	using Serilog;

	public class ProcessWallet
	{
		private Settings Settings { get; }
		private TransactionHelper TransactionHelper { get; }
		private AccessWallet AccessWallet { get; }

		/// <inheritdoc />
		public ProcessWallet(Settings settings, TransactionHelper transactionHelper)
		{
			Settings = settings;
			TransactionHelper = transactionHelper;
			AccessWallet = new AccessWallet(settings.Connection, TransactionHelper.ConnectionTimeout);
		}

		//Used Commands: getstakesplitthreshold, listreceivedbyaddress, walletpassphrase,listunspent, walletlock, createrawtransaction, signrawtransaction
		//sendrawtransaction, setstakesplitthreshold

		public void Run()
		{
			AccessWallet.WalletPassphrase(Settings.Stake.WalletPassword, 0);
			//Get all addresses from the wallet
			IEnumerable<ListReceivedByAccountResponse> addresses = AccessWallet.ListReceivedByAddress();
			//Move inputs too collect address
			MoveToCollectAddress(addresses.Select(a => a.Address));
			MinimizeInputs();
			GenerateStakingInputs();
			AccessWallet.WalletLock();
			AccessWallet.WalletPassphrase(Settings.Stake.WalletPassword, 0, true);
		}

		private void GenerateStakingInputs()
		{
			//Get Current stakeSplitThereshold
			decimal stakeSplitThreshold = AccessWallet.GetStakeSplitThreshold();
			Log.Information($"Initialize run with staking threshold: {stakeSplitThreshold}");
			decimal newStakeSplitThreshold = stakeSplitThreshold;
			newStakeSplitThreshold = CheckForLowerThreshold(stakeSplitThreshold);
			//If every staking input is older than patience value rise the thereshold
			newStakeSplitThreshold = CheckForHigherThreshold(newStakeSplitThreshold);
			if(stakeSplitThreshold != newStakeSplitThreshold)
			{
				Log.Information($"Set new staking threshold: {newStakeSplitThreshold}");
				AccessWallet.SetStakeSplitThreshold((int)newStakeSplitThreshold);
				stakeSplitThreshold = newStakeSplitThreshold;
			}
			//Add Coins to low inputs
			ManageLowInputs(stakeSplitThreshold);
			CreateNewInputs(stakeSplitThreshold);
		}

		private void CreateNewInputs(decimal stakeSplitThreshold)
		{
			if (!Settings.Stake.EditStakes)
			{
				return;
			}

			ListUnspentResponse addressInput = AccessWallet.ListUnspent(1, int.MaxValue, new List<string> {Settings.Stake.DedicatedCollectingAddress})
			                                               .OrderByDescending(i => i.Amount)
			                                               .FirstOrDefault();
			if (!(addressInput?.Amount >= stakeSplitThreshold))
			{
				return;
			}

			var inputs = new List<CreateRawTransactionInput>();
			var outputs = new Dictionary<string, decimal>();
			inputs.Add(new CreateRawTransactionInput {TxId = addressInput.TxId, Vout = addressInput.Vout});
			outputs.Add(Settings.Stake.DedicatedStakingAddress, addressInput.Amount);
			Send(inputs, outputs);
		}

		private void ManageLowInputs(decimal stakeSplitThreshold)
		{
			if (!Settings.Stake.EditStakes)
			{
				return;
			}

			var found = true;
			int maxInputs = TransactionHelper.GetMaxPossibleInputCountForFreeTransaction(2);
			int maxOutputs = TransactionHelper.GetMaxPossibleOutputCountForFreeTransaction(2);

			if (maxInputs < 2 || maxOutputs < 2)
			{
				//Impossible to make zero fee transaction.
				return;
			}

			while (found)
			{
				found = false;

				ListUnspentResponse addressInput = AccessWallet.ListUnspent(1, int.MaxValue, new List<string> {Settings.Stake.DedicatedCollectingAddress})
				                                               .OrderByDescending(i => i.Amount)
				                                               .FirstOrDefault();
				if (addressInput?.Amount > 0)
				{
					List<ListUnspentResponse> lowInputs = AccessWallet.ListUnspent(1, int.MaxValue, new List<string> {Settings.Stake.DedicatedStakingAddress});
					ListUnspentResponse lowInput = lowInputs.FirstOrDefault(i => i.Amount < stakeSplitThreshold);
					if (lowInput != null)
					{
						decimal mergedAmount = lowInput.Amount + addressInput.Amount;
						var inputs = new List<CreateRawTransactionInput>();
						var outputs = new Dictionary<string, decimal>();
						inputs.Add(new CreateRawTransactionInput {TxId = addressInput.TxId, Vout = addressInput.Vout});
						inputs.Add(new CreateRawTransactionInput {TxId = lowInput.TxId, Vout = lowInput.Vout});
						if (mergedAmount > stakeSplitThreshold)
						{
							outputs.Add(Settings.Stake.DedicatedStakingAddress, stakeSplitThreshold);
							outputs.Add(Settings.Stake.DedicatedCollectingAddress, mergedAmount - stakeSplitThreshold);
						}
						else
						{
							outputs.Add(Settings.Stake.DedicatedStakingAddress, mergedAmount);
						}

						string transaction = Send(inputs, outputs);
						found = true;
						WaitTillAllConfirmed(new List<string> {transaction});
					}
				}
			}
		}

		private decimal CheckForHigherThreshold(decimal stakeSplitThreshold)
		{
			decimal newStakeSplitThreshold = stakeSplitThreshold;
			//If every staking input is not older than 1 day lower the thereshold
			List<ListUnspentResponse> stakeInputs = AccessWallet.ListUnspent(0, int.MaxValue, new List<string> {Settings.Stake.DedicatedStakingAddress});
			if (stakeInputs.Count > 0)
			{
				List<ListUnspentResponse> oldestInputs = stakeInputs.OrderByDescending(s => s.Confirmations)
				                                                          .Where(s =>
					                                                                 TransactionHelper.BaseDate.AddSeconds(AccessWallet.GetTransaction(s.TxId)
					                                                                                                                   .BlockTime) <
					                                                                 DateTime.UtcNow.AddDays(-Settings.Stake.StakingPatience)).ToList();
				if (oldestInputs.Count > 0)
				{
					newStakeSplitThreshold = Math.Max(oldestInputs.Max(i => i.Amount), stakeSplitThreshold);
				}
			}

			return newStakeSplitThreshold;
		}

		private decimal CheckForLowerThreshold(decimal stakeSplitThreshold)
		{
			decimal newStakeSplitThreshold = stakeSplitThreshold;
			//If every staking input is not older than 1 day lower the thereshold
			List<ListUnspentResponse> stakeInputs = AccessWallet.ListUnspent(0, int.MaxValue, new List<string> {Settings.Stake.DedicatedStakingAddress});
			if (stakeInputs.Count > 0)
			{
				ListUnspentResponse oldestInput = stakeInputs.OrderByDescending(s => s.Confirmations).First();
				double waitDays = Settings.Stake.StakingPatience / 2D;

				DateTime blockdate = TransactionHelper.BaseDate.AddSeconds(AccessWallet.GetTransaction(oldestInput.TxId).BlockTime);
				if (blockdate > DateTime.UtcNow.AddDays(-waitDays))
				{
					newStakeSplitThreshold = Math.Ceiling(oldestInput.Amount / 2);
				}
			}

			return Math.Min(stakeSplitThreshold, newStakeSplitThreshold);
		}

		private void MinimizeInputs()
		{
			List<string> transactions;
			do
			{
				transactions = ProcessInputs(new List<string>{Settings.Stake.DedicatedCollectingAddress}, 2);
				WaitTillAllConfirmed(transactions);
			} while (transactions.Count > 0);
		}

		private void MoveToCollectAddress(IEnumerable<string> addresses)
		{
			if (!Settings.Address.CollectInputs)
			{
				return;
			}

			IEnumerable<string> transactions = ProcessInputs(addresses.Except(Settings.Address.ExcludeAddresses).ToList(), 1);
			WaitTillAllConfirmed(transactions);
		}

		private void WaitTillAllConfirmed([NotNull] IEnumerable<string> transactions)
		{
			List<string> waitingList = transactions.ToList();
			while (waitingList.Count > 0)
			{
				Log.Information($"Waiting for {waitingList.Count} transactions to complete.");
				Task.Delay(TimeSpan.FromSeconds(10)).Wait();
				waitingList = waitingList.Where(t => AccessWallet.GetTransaction(t).Confirmations == 0).ToList();
			}

			Log.Information("All transactions complete.");
		}

		[NotNull]
		private List<string> ProcessInputs([NotNull] List<string> addresses, int minimunNeededInputs)
		{
			const int maxTries = 10;
			var finished = false;
			var currentTry = 0;
			var transactionList = new List<string>();
			int maxInputs = TransactionHelper.GetMaxPossibleInputCountForFreeTransaction(1);

			while (!finished)
			{
				try
				{
					++currentTry;
					List<ListUnspentResponse> transactions = AccessWallet.ListUnspent(1, int.MaxValue, addresses);
					transactions = transactions.Where(t => t.Amount > 0M).ToList();

					var tCount = 0;
					var inputs = new List<CreateRawTransactionInput>();
					var outputs = new Dictionary<string, decimal>();
					var amount = 0M;

					if (transactions.Count >= minimunNeededInputs)
					{
						string plural = transactions.Count > 1 ? "s" : string.Empty;
						Log.Information($"Merge {transactions.Count} input{plural} to {Settings.Stake.DedicatedCollectingAddress}");
						foreach (ListUnspentResponse trans in transactions)
						{
							Log.Debug($"Include Transaction: {trans.TxId}");
							inputs.Add(new CreateRawTransactionInput {TxId = trans.TxId, Vout = trans.Vout});
							amount += trans.Amount;
							++tCount;

							if (tCount < maxInputs)
							{
								continue;
							}

							Log.Information($"Move {amount} coins to {Settings.Stake.DedicatedCollectingAddress} without fee.");
							outputs.Add(Settings.Stake.DedicatedCollectingAddress, amount);
							transactionList.Add(Send(inputs, outputs));
							tCount = 0;
							inputs = new List<CreateRawTransactionInput>();
							outputs = new Dictionary<string, decimal>();
							amount = 0M;
						}
					}

					if (tCount > 0)
					{
						Log.Information($"Move {amount} coins to {Settings.Stake.DedicatedCollectingAddress} without fee.");
						outputs.Add(Settings.Stake.DedicatedCollectingAddress, amount);
						transactionList.Add(Send(inputs, outputs));
					}

					finished = true;
				}
				catch (Exception e)
				{
					if (currentTry <= maxTries)
					{
						continue;
					}
					Log.Error(e, "An unresolveable Error occured. Giving up.");
					finished = true;
				}
			}

			return transactionList.Where(t => !string.IsNullOrWhiteSpace(t)).ToList();
		}

		[CanBeNull]
		private string Send(IList<CreateRawTransactionInput> inputs, IDictionary<string, decimal> outputs)
		{
			var request = new CreateRawTransactionRequest(inputs, outputs);
			string txId = AccessWallet.CreateRawTransaction(request);

			SignRawTransactionResponse res = AccessWallet.SignRawTransaction(txId);
			return res.Complete ? AccessWallet.SendRawTransaction(res.Hex) : null;
		}
	}
}
