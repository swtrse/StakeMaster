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
	using RpcResponseTypes;

	public class AccessWallet
	{
		/// <inheritdoc />
		public AccessWallet(ConnectionSettings connectionSettings, int timeout)
		{
			ConnectionSettings = connectionSettings;
			Timeout = timeout;
		}
		private int Timeout { get; }
		private ConnectionSettings ConnectionSettings { get; }

		private T CallWallet<T>(string method, params object[] parameters)
		{
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
				return rpcResponse.Result;
			}
			catch (Exception exception)
			{
				throw new Exception("A more friendly Exception will be added later.", exception);
			}
		}

		public decimal GetStakeSplitThreshold() => CallWallet<decimal>("getstakesplitthreshold");

		public GetTransactionResponse GetTransaction(string transactionId, bool includeWatchonly = false) =>
			CallWallet<GetTransactionResponse>("gettransaction", transactionId, includeWatchonly);

		[NotNull]
		public IEnumerable<ListReceivedByAccountResponse> ListReceivedByAddress(int minimumConfirmations = 1, bool includeEmpty = false, bool includeWatchonly = false) =>
			CallWallet<List<ListReceivedByAccountResponse>>("listreceivedbyaddress", minimumConfirmations, includeEmpty, includeWatchonly);

		[NotNull]
		private WebRequest SetAuthHeader([NotNull] WebRequest webRequest)
		{
			string authInfo = ConnectionSettings.RpcUser + ":" + ConnectionSettings.RpcPassword;
			authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
			webRequest.Headers["Authorization"] = "Basic " + authInfo;
			return webRequest;
		}

		public void WalletLock() => CallWallet<object>("walletlock");

		public void WalletPassphrase(string password, int secondsTillLock, bool anonymizeOnly = false) =>
			CallWallet<object>("walletpassphrase", password, secondsTillLock, anonymizeOnly);

		public List<ListUnspentResponse> ListUnspent(int minimumConfirmations = 1, int maximumConfirmations = 999999, List<string> addresses = null)
		{
			addresses = addresses ?? new List<string>();
			return CallWallet<List<ListUnspentResponse>>("listunspent", minimumConfirmations, maximumConfirmations, addresses);
		}

		public string CreateRawTransaction(CreateRawTransactionRequest rawTransactionRequest)
		{
			return CallWallet<string>("createrawtransaction", rawTransactionRequest.Inputs, rawTransactionRequest.Outputs);
		}

		public SignRawTransactionResponse SignRawTransaction(string transaction, List<string> previousTransactions = null, List<string> privateKeys = null, string signatureHashType = "ALL")
		{
			return CallWallet<SignRawTransactionResponse>("signrawtransaction", transaction, previousTransactions, privateKeys, signatureHashType);
		}

		public string SendRawTransaction(string transaction, bool allowHighFees = false)
		{
			return CallWallet<string>("sendrawtransaction", transaction, allowHighFees);
		}

		public void SetStakeSplitThreshold(int amount)
		{
			CallWallet<object>("setstakesplitthreshold", (int)amount);
		}
	}
}
