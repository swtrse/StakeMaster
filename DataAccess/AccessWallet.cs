// ******************************* Module Header *******************************
// Module Name: AccessWallet.cs
// Project:     StakeMasterDataAccess
// Copyright (c) Michael Goldfinger.
// 
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// *****************************************************************************

namespace StakeMaster.DataAccess
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Net;
	using System.Text;
	using JetBrains.Annotations;
	using Newtonsoft.Json;
	using Rpc;
	using Serilog;

	/// <summary>
	///     Contains methods to access the wallet rpc api.
	/// </summary>
	public sealed class AccessWallet
	{
		/// <inheritdoc />
		public AccessWallet(ConnectionSettings connectionSettings, int timeout)
		{
			ConnectionSettings = connectionSettings;
			Timeout = timeout;
		}

		private ConnectionSettings ConnectionSettings { get; }
		private int Timeout { get; }

		private T CallWallet<T>(string method, params object[] parameters)
		{
			Log.Debug("Call of method: T AccessWallet.CallWallet<T>(string method, params object[] parameters).");
			Log.Verbose("Parameter method: {method}.", method);
			Log.Verbose("Parameter parameters: {@parameters}.", parameters);

			var request = new WalletRpcRequest(1, method, parameters);
			WebRequest webRequest = WebRequest.Create(ConnectionSettings.RpcUri);
			webRequest = SetAuthHeader(webRequest);
			webRequest.ContentType = "application/json-rpc";
			webRequest.Method = "POST";
			webRequest.Proxy = null;
			webRequest.Timeout = Timeout * 1000;
			byte[] byteArray = request.GetBytes();
			webRequest.ContentLength = request.GetBytes().Length;

			try
			{
				using (Stream dataStream = webRequest.GetRequestStream())
				{
					dataStream.Write(byteArray, 0, byteArray.Length);
				}

				string json;
				using (WebResponse webResponse = webRequest.GetResponse())
				{
					using (Stream stream = webResponse.GetResponseStream())
					{
						using (var reader = new StreamReader(stream ?? throw new InvalidOperationException()))
						{
							string result = reader.ReadToEnd();
							json = result;
						}
					}
				}

				var rpcResponse = JsonConvert.DeserializeObject<WalletRpcResponse<T>>(json);
				T res = rpcResponse.Result;
				Log.Verbose("Returnvalue of AccessWallet.CallWallet<T>(string method, params object[] parameters): {@res}.", res);
				return res;
			}
			catch (Exception exception)
			{
				Log.Error(exception, "An error occurd during the rpc call.");
				throw;
			}
		}

		/// <summary>
		///     Creates a new raw transaction.
		/// </summary>
		/// <param name="rawTransactionRequest">The request holding the inputs and outputs for the transaction.</param>
		/// <returns>
		///     The hex string of the transaction.
		/// </returns>
		public string CreateRawTransaction([NotNull] CreateRawTransactionRequest rawTransactionRequest) =>
			CallWallet<string>("createrawtransaction", rawTransactionRequest.Inputs, rawTransactionRequest.Outputs);

		/// <summary>
		///     Returns the threshold for stake splitting.
		/// </summary>
		/// <returns>The threshold value.</returns>
		public decimal GetStakeSplitThreshold() => CallWallet<decimal>("getstakesplitthreshold");

		/// <summary>
		///     Get detailed information about in-wallet transaction.
		/// </summary>
		/// <param name="transactionId">The transaction id.</param>
		/// <returns>A <see cref="GetTransactionResponse" />-object holding the result.</returns>
		public GetTransactionResponse GetTransaction(string transactionId) =>
			CallWallet<GetTransactionResponse>("gettransaction", transactionId);


		/// <summary>
		///     List balances by receiving address.
		/// </summary>
		/// <returns>A <see cref="IEnumerable{ListReceivedByAccountResponse}" />-object holding the result.</returns>
		[NotNull]
		public IEnumerable<ListReceivedByAccountResponse> ListReceivedByAddress() => CallWallet<List<ListReceivedByAccountResponse>>("listreceivedbyaddress");

		/// <summary>
		///     Returns a List of unspent transaction outputs with between minimumConfirmations and maximumConfirmations
		///     (inclusive) confirmations.
		/// </summary>
		/// <param name="minimumConfirmations">
		///     The minimum confirmations to filter. (Default=1).
		/// </param>
		/// <param name="maximumConfirmations">
		///     The maximum confirmations to filter. (Default=999999).
		/// </param>
		/// <param name="addresses">A list of addresses to filter.</param>
		/// <returns>A <see cref="List{ListUnspentResponse}" />-object holding the result.</returns>
		public List<ListUnspentResponse> ListUnspent(int minimumConfirmations = 1, int maximumConfirmations = 999999, [CanBeNull] List<string> addresses = null) =>
			addresses == null
				? CallWallet<List<ListUnspentResponse>>("listunspent", minimumConfirmations, maximumConfirmations)
				: CallWallet<List<ListUnspentResponse>>("listunspent", minimumConfirmations, maximumConfirmations, addresses);

		/// <summary>
		///     Submits raw transaction (serialized, hex-encoded) to local node and network.
		/// </summary>
		/// <param name="transaction">The hex string of the raw transaction.</param>
		/// <returns>The transaction hash in hex.</returns>
		public string SendRawTransaction(string transaction) => CallWallet<string>("sendrawtransaction", transaction);

		[NotNull]
		private WebRequest SetAuthHeader([NotNull] WebRequest webRequest)
		{
			string authInfo = ConnectionSettings.RpcUser + ":" + ConnectionSettings.RpcPassword;
			authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
			webRequest.Headers["Authorization"] = "Basic " + authInfo;
			return webRequest;
		}

		/// <summary>
		///     This will set the output size of your stakes to never be below this number.
		/// </summary>
		/// <param name="amount">Threshold value between 1 and 999999.</param>
		public void SetStakeSplitThreshold(int amount)
		{
			CallWallet<object>("setstakesplitthreshold", amount);
		}

		/// <summary>
		///     Sign inputs for raw transaction (serialized, hex-encoded).
		/// </summary>
		/// <param name="transaction">The transaction hex string.</param>
		/// <returns>A <see cref="SignRawTransactionResponse" />-object holding the result.</returns>
		public SignRawTransactionResponse SignRawTransaction(string transaction) =>
			CallWallet<SignRawTransactionResponse>("signrawtransaction", transaction);

		/// <summary>
		///     Removes the wallet encryption key from memory, locking the wallet.
		/// </summary>
		public void WalletLock()
		{
			CallWallet<object>("walletlock");
		}

		/// <summary>
		///     Stores the wallet decryption key in memory for 'timeout' seconds.
		/// </summary>
		/// <param name="password">The wallet passphrase.</param>
		/// <param name="secondsTillLock">
		///     The time to keep the decryption key in seconds. A timeout of "0" unlocks until the wallet
		///     is closed.
		/// </param>
		/// <param name="anonymizeOnly">If is true sending functions are disabled.</param>
		public void WalletPassphrase(string password, int secondsTillLock, bool anonymizeOnly = false)
		{
			CallWallet<object>("walletpassphrase", password, secondsTillLock, anonymizeOnly);
		}
	}
}
