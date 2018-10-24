using System;
using System.Collections.Generic;
using System.Text;

namespace StakeMaster.DataAccess.RpcResponseTypes
{
	public class ListUnspentResponse
	{
		public string TxId { get; set; }
		public int Vout { get; set; }
		public string Address { get; set; }
		public string Account { get; set; }
		public string ScriptPubKey { get; set; }
		public decimal Amount { get; set; }
		public int Confirmations { get; set; }
		public bool Spendable { get; set; }

		public override string ToString()
		{
			return $"Account: {Account}, Address: {Address}, Amount: {Amount}, Confirmations: {Confirmations}";
		}
	}
}
