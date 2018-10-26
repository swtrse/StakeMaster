// ******************************* Module Header *******************************
// Module Name: ListUnspentResponse.cs
// Project:     StakeMasterEntities
// Copyright (c) Michael Goldfinger.
// 
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// *****************************************************************************

namespace StakeMaster.Rpc
{
	public sealed class ListUnspentResponse
	{
		public string Account { get; set; }
		public string Address { get; set; }
		public decimal Amount { get; set; }
		public int Confirmations { get; set; }
		public string ScriptPubKey { get; set; }
		public bool Spendable { get; set; }
		public string TxId { get; set; }
		public int Vout { get; set; }

		public override string ToString() => $"Account: {Account}, Address: {Address}, Amount: {Amount}, Confirmations: {Confirmations}";
	}
}
