// ******************************* Module Header *******************************
// Module Name: ProcessWallet.cs
// Project:     StakeMasterBusinessLogic
// Copyright (c) Michael Goldfinger.
// 
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// *****************************************************************************

namespace StakeMaster.BusinessLogic
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using DataAccess;
	using JetBrains.Annotations;
	using Rpc;
	using Serilog;

	/// <summary>
	///     Contains the core logic for the StakeMaster features.
	/// </summary>
	public sealed class ProcessWallet
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="ProcessWallet" /> class.
		/// </summary>
		/// <param name="settings">
		///     The settings for the current run.
		/// </param>
		/// <param name="transactionHelper">
		///     The transaction settings for the current run.
		/// </param>
		/// <inheritdoc />
		public ProcessWallet([NotNull] Settings settings, TransactionHelper transactionHelper)
		{
			Log.Debug("Call of constructor: ProcessWallet(Settings settings, TransactionHelper transactionHelper).");
			Log.Verbose("Parameter settings: {@settings}.", settings);
			Log.Verbose("Parameter transactionHelper: {@transactionHelper}.", transactionHelper);
			Settings = settings;
			TransactionHelper = transactionHelper;
			AccessWallet = new AccessWallet(settings.Connection, TransactionHelper.ConnectionTimeout);
		}

		private AccessWallet AccessWallet { get; }
		private Settings Settings { get; }
		private TransactionHelper TransactionHelper { get; }

		private decimal CheckForHigherThreshold(decimal stakeSplitThreshold)
		{
			Log.Debug("Call of method: decimal ProcessWallet.CheckForHigherThreshold(decimal stakeSplitThreshold).");
			Log.Verbose("Parameter stakeSplitThreshold: {stakeSplitThreshold}.", stakeSplitThreshold);
			decimal newStakeSplitThreshold = stakeSplitThreshold;

			List<ListUnspentResponse> stakeInputs = AccessWallet.ListUnspent(0, int.MaxValue, new List<string> {Settings.Stake.DedicatedStakingAddress});
			if (stakeInputs.Count <= 0)
			{
				return newStakeSplitThreshold;
			}

			List<ListUnspentResponse> oldestInputs = stakeInputs.OrderByDescending(s => s.Confirmations)
			                                                    .Where(s => TransactionHelper.BaseDate.AddSeconds(AccessWallet.GetTransaction(s.TxId).BlockTime) <
			                                                                DateTime.UtcNow.AddDays(-Settings.Stake.StakingPatience))
			                                                    .ToList();
			if (oldestInputs.Count > 0)
			{
				newStakeSplitThreshold = oldestInputs.Max(i => i.Amount);
				newStakeSplitThreshold = Math.Floor(newStakeSplitThreshold);
				if (newStakeSplitThreshold <= stakeSplitThreshold)
				{
					newStakeSplitThreshold = oldestInputs.Max(i => i.Amount) * 1.2M;
					newStakeSplitThreshold = Math.Ceiling(newStakeSplitThreshold);
				}
				newStakeSplitThreshold = Math.Max(newStakeSplitThreshold, stakeSplitThreshold);
			}

			Log.Verbose("Returnvalue of ProcessWallet.CheckForHigherThreshold(decimal stakeSplitThreshold): {newStakeSplitThreshold}.", newStakeSplitThreshold);
			return newStakeSplitThreshold;
		}

		private decimal CheckForLowerThreshold(decimal stakeSplitThreshold)
		{
			Log.Debug("Call of method: decimal ProcessWallet.CheckForLowerThreshold(decimal stakeSplitThreshold).");
			Log.Verbose("Parameter stakeSplitThreshold: {stakeSplitThreshold}.", stakeSplitThreshold);
			//If every staking input is not older than StakePatient/2 days lower the thereshold
			List<ListUnspentResponse> stakeInputs = AccessWallet.ListUnspent(0, int.MaxValue, new List<string> {Settings.Stake.DedicatedStakingAddress});
			if (stakeInputs.Count <= 0)
			{
				return stakeSplitThreshold;
			}

			decimal newStakeSplitThreshold = stakeSplitThreshold;
			ListUnspentResponse oldestInput = stakeInputs.OrderByDescending(s => s.Confirmations).First();
			double waitDays = Settings.Stake.StakingPatience / 2D;

			DateTime blockdate = TransactionHelper.BaseDate.AddSeconds(AccessWallet.GetTransaction(oldestInput.TxId).BlockTime);
			if (blockdate > DateTime.UtcNow.AddDays(-waitDays))
			{
				newStakeSplitThreshold = oldestInput.Amount >= stakeSplitThreshold ? Math.Ceiling(oldestInput.Amount * 0.8M) : Math.Ceiling(oldestInput.Amount);
			}

			decimal ret = Math.Min(stakeSplitThreshold, newStakeSplitThreshold);
			Log.Verbose("Returnvalue of ProcessWallet.CheckForLowerThreshold(decimal stakeSplitThreshold): {ret}.", ret);
			return ret;
		}

		private void CreateNewInputs(decimal stakeSplitThreshold)
		{
			Log.Debug("Call of method: void ProcessWallet.CreateNewInputs(decimal stakeSplitThreshold).");
			Log.Verbose("Parameter stakeSplitThreshold: {stakeSplitThreshold}.", stakeSplitThreshold);
			if (!Settings.Stake.EditStakes)
			{
				Log.Information("Edit stakes deactivated. No new stake input will be created.");
				return;
			}

			ListUnspentResponse addressInput = AccessWallet.ListUnspent(1, int.MaxValue, new List<string> {Settings.Stake.DedicatedCollectingAddress})
			                                               .OrderByDescending(i => i.Amount)
			                                               .FirstOrDefault();
			if (!(addressInput?.Amount >= stakeSplitThreshold))
			{
				Log.Information("No need to create a new stake input.");
				return;
			}

			Log.Information("Create a new stake input.");
			var inputs = new List<CreateRawTransactionInput>();
			var outputs = new Dictionary<string, decimal>();
			inputs.Add(new CreateRawTransactionInput {TxId = addressInput.TxId, Vout = addressInput.Vout});
			outputs.Add(Settings.Stake.DedicatedStakingAddress, addressInput.Amount);
			Send(inputs, outputs);
		}

		private void GenerateStakingInputs()
		{
			Log.Debug("Call of method: void ProcessWallet.GenerateStakingInputs().");
			//Get Current stakeSplitThereshold
			decimal stakeSplitThreshold = AccessWallet.GetStakeSplitThreshold();
			Log.Information($"Initialize run with current staking threshold: {stakeSplitThreshold}.");
			//If ever staking input is newer than half of the patience value lower the thereshold.
			decimal newStakeSplitThreshold = CheckForLowerThreshold(stakeSplitThreshold);
			//If every staking input is older than patience value rise the thereshold
			newStakeSplitThreshold = CheckForHigherThreshold(newStakeSplitThreshold);
			if (stakeSplitThreshold != newStakeSplitThreshold)
			{
				Log.Information($"Set new staking threshold: {(int)newStakeSplitThreshold}.");
				AccessWallet.SetStakeSplitThreshold((int) newStakeSplitThreshold);
				stakeSplitThreshold = (int)newStakeSplitThreshold;
			}

			//Add Coins to low inputs
			ManageLowInputs(stakeSplitThreshold);
			CreateNewInputs(stakeSplitThreshold);
		}

		private void ManageLowInputs(decimal stakeSplitThreshold)
		{
			Log.Debug("Call of method: void ProcessWallet.ManageLowInputs(decimal stakeSplitThreshold).");
			Log.Verbose("Parameter stakeSplitThreshold: {stakeSplitThreshold}.", stakeSplitThreshold);
			if (!Settings.Stake.EditStakes)
			{
				Log.Information("Edit stakes deactivated. No stake input will be altered.");
				return;
			}

			var found = true;
			int maxInputs = TransactionHelper.GetMaxPossibleInputCountForFreeTransaction(2);
			Log.Verbose("Allowed inputs: {maxInputs}.", maxInputs);
			int maxOutputs = TransactionHelper.GetMaxPossibleOutputCountForFreeTransaction(2);
			Log.Verbose("Allowed outputs: {maxInputs}.", maxOutputs);

			if (maxInputs < 2 || maxOutputs < 2)
			{
				Log.Warning("Unable to create a zero fee transaction.");
				//Impossible to make zero fee transaction.
				return;
			}

			while (found)
			{
				found = false;

				ListUnspentResponse addressInput = AccessWallet.ListUnspent(1, int.MaxValue, new List<string> {Settings.Stake.DedicatedCollectingAddress})
				                                               .OrderByDescending(i => i.Amount)
				                                               .FirstOrDefault();
				if (!(addressInput?.Amount > 0))
				{
					continue;
				}

				List<ListUnspentResponse> lowInputs = AccessWallet.ListUnspent(1, int.MaxValue, new List<string> {Settings.Stake.DedicatedStakingAddress});
				//Low confirmations first. This will prevent the threshold update logic to be confused and this are the inputs that stake less.
				ListUnspentResponse lowInput = lowInputs.OrderBy(i => i.Confirmations).FirstOrDefault(i => i.Amount < stakeSplitThreshold);
				if (lowInput == null)
				{
					Log.Information("No need to alter a stake input.");
					continue;
				}

				Log.Information("Alter a stake input to match stake split threshold.");
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

		private void MinimizeInputs()
		{
			Log.Debug("Call of method: void ProcessWallet.MinimizeInputs().");
			List<string> transactions;
			do
			{
				transactions = ProcessInputs(new List<string> {Settings.Stake.DedicatedCollectingAddress}, 2);
				WaitTillAllConfirmed(transactions);
			} while (transactions.Count > 0);
		}

		private void MoveToCollectAddress(List<string> addresses)
		{
			Log.Debug("Call of method: void ProcessWallet.MoveToCollectAddress(List<string> addresses).");
			Log.Verbose("Parameter addresses: {@addresses}.", addresses);
			if (!Settings.Address.CollectInputs)
			{
				Log.Information("Input collection deactivated. No inputs will be moved to the collection address.");
				return;
			}

			List<string> transactions = ProcessInputs(addresses.Except(Settings.Address.ExcludeAddresses).ToList(), 1);
			WaitTillAllConfirmed(transactions);
		}

		[NotNull]
		private List<string> ProcessInputs([NotNull] List<string> addresses, int minimunNeededInputs, decimal amountLock = decimal.MaxValue)
		{
			Log.Debug("Call of method: List<string> ProcessWallet.ProcessInputs(List<string> addresses, int minimunNeededInputs).");
			Log.Verbose("Parameter addresses: {@addresses}.", addresses);
			Log.Verbose("Parameter minimunNeededInputs: {minimunNeededInputs}.", minimunNeededInputs);
			Log.Verbose("Parameter amount: {amountLock}.", amountLock);

			if (addresses.Count == 0)
			{
				var erg = new List<string>();
				Log.Verbose("Returnvalue of ProcessWallet.ProcessInputs(List<string> addresses, int minimunNeededInputs): {@ret}.", erg);
				return erg;
			}

			var transactionList = new List<string>();
			int maxInputs = TransactionHelper.GetMaxPossibleInputCountForFreeTransaction(1);
			Log.Verbose("Allowed inputs: {maxInputs}.", maxInputs);

			List<ListUnspentResponse> transactions = AccessWallet.ListUnspent(1, int.MaxValue, addresses);
			Log.Verbose("Transactions: {@transactions}.", transactions);
			transactions = transactions.Where(t => t.Amount > 0M && t.Amount <= amountLock).ToList();

			var tCount = 0;
			var inputs = new List<CreateRawTransactionInput>();
			var outputs = new Dictionary<string, decimal>();
			var amount = 0M;

			if (transactions.Count >= minimunNeededInputs)
			{
				string plural = transactions.Count > 1 ? "s" : string.Empty;
				Log.Information($"Merge {transactions.Count} input{plural} to {Settings.Stake.DedicatedCollectingAddress}.");
				foreach (ListUnspentResponse trans in transactions)
				{
					Log.Debug($"Include Transaction: {trans.TxId}.");
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


			List<string> ret = transactionList.Where(t => !string.IsNullOrWhiteSpace(t)).ToList();
			Log.Verbose("Returnvalue of ProcessWallet.ProcessInputs(List<string> addresses, int minimunNeededInputs): {@ret}.", ret);
			return ret;
		}

		/// <summary>
		///     Process all operations on the wallet based on the current settings.
		/// </summary>
		public void Run()
		{
			Log.Debug("Call of method: void ProcessWallet.Run().");
			Log.Information("Unlock wallet.");
			AccessWallet.WalletPassphrase(Settings.Stake.WalletPassword, 0);
			//Get all addresses from the wallet
			Log.Information("Get all addresses of the wallet that hold balance.");
			IEnumerable<ListReceivedByAccountResponse> addresses = AccessWallet.ListReceivedByAddress();
			//Move inputs too collect address
			MoveToCollectAddress(addresses.Select(a => a.Address).ToList());
			// Move small stakes to Collect Address
			MoveSmallStakesToCollectAddress();
			MinimizeInputs();
			GenerateStakingInputs();
			Log.Information("Lock wallet.");
			AccessWallet.WalletLock();
			Log.Information("Unlock wallet for staking only.");
			AccessWallet.WalletPassphrase(Settings.Stake.WalletPassword, 0, true);
		}

		private void MoveSmallStakesToCollectAddress()
		{
			Log.Debug("Call of method: void ProcessWallet.MoveSmallStakesToCollectAddress().");
			if (!Settings.Address.CollectInputs)
			{
				Log.Information("Input collection deactivated. No inputs will be moved to the collection address.");
				return;
			}

			decimal stakeSplitThreshold = AccessWallet.GetStakeSplitThreshold();
			List<string> transactions = ProcessInputs(new List<string>{Settings.Stake.DedicatedStakingAddress}, 1, stakeSplitThreshold * 0.7M);
			WaitTillAllConfirmed(transactions);
		}

		[CanBeNull]
		private string Send(IList<CreateRawTransactionInput> inputs, IDictionary<string, decimal> outputs)
		{
			const int maxTries = 10;
			var currentTry = 0;
			var finished = false;
			Log.Debug("Call of method: string ProcessWallet.Send(IList<CreateRawTransactionInput> inputs, IDictionary<string, decimal> outputs).");
			Log.Verbose("Parameter inputs: {@inputs}.", inputs);
			Log.Verbose("Parameter outputs: {@outputs}.", outputs);
			while (!finished)
			{
				try
				{
					++currentTry;
					var request = new CreateRawTransactionRequest(inputs, outputs);
					string txId = AccessWallet.CreateRawTransaction(request);

					SignRawTransactionResponse res = AccessWallet.SignRawTransaction(txId);
					string ret = res.Complete ? AccessWallet.SendRawTransaction(res.Hex) : null;
					finished = true;
					Log.Verbose("Returnvalue of ProcessWallet.Send(IList<CreateRawTransactionInput> inputs, IDictionary<string, decimal> outputs): {ret}.", ret);
					return ret;
				}
				catch (Exception e)
				{
					if (currentTry <= maxTries)
					{
						Log.Warning("Unable to send zero fee transaction. Retry...");
						Task.Delay(TimeSpan.FromMinutes(1D)).Wait();
						continue;
					}

					Log.Error(e, "An unresolveable Error occured. Giving up.");
					throw;
				}
			}

			throw new Exception("An unresolveable Error occured.");
		}

		private void WaitTillAllConfirmed([NotNull] List<string> transactions)
		{
			Log.Debug("Call of method: string ProcessWallet.WaitTillAllConfirmed(List<string> transactions).");
			Log.Verbose("Parameter transactions: {@transactions}.", transactions);
			List<string> waitingList = transactions.ToList();
			while (waitingList.Count > 0)
			{
				Log.Information($"Waiting for {waitingList.Count} transactions to complete.");
				Task.Delay(TimeSpan.FromSeconds(10)).Wait();
				//Some wallets do not allow Zero Fee transactions if the inputs have 0 or even 1 confirmation.
				//The wallets are not consistent in that, there are wallets that sometimes let the transaction happen
				//and sometimes the refuse. There seem to be no logic behind that behavior.
				waitingList = waitingList.Where(t => AccessWallet.GetTransaction(t).Confirmations < TransactionHelper.Confirms).ToList();
			}

			Log.Information("All transactions complete.");
		}
	}
}
