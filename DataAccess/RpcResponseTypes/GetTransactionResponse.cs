// ******************************* Module Header *******************************
// Module Name: GetTransactionResponse.cs
// Project:     StakeMasterDataAccess
// Copyright (c) Michael Goldfinger.
// 
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// *****************************************************************************

namespace StakeMaster.DataAccess.RpcResponseTypes
{
	using System.Collections.Generic;

	public class GetTransactionResponse
	{
		public decimal Amount { get; set; }
		public string BlockHash { get; set; }
		public int BlockIndex { get; set; }
		public int BlockTime { get; set; }
		public int Confirmations { get; set; }
		public List<GetTransactionResponseDetails> Details { get; set; }
		public decimal Fee { get; set; }
		public string Hex { get; set; }
		public int Time { get; set; }
		public int TimeReceived { get; set; }
		public string TxId { get; set; }
		public List<string> WalletConflicts { get; set; }
	}
}
