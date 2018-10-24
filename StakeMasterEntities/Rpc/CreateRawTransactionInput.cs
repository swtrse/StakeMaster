using System;
using System.Collections.Generic;
using System.Text;

namespace StakeMaster.Rpc
{
	using Newtonsoft.Json;

	public class CreateRawTransactionInput
	{
		[JsonProperty("txid")]
		public string TxId { get; set; }

		[JsonProperty("vout")]
		public int Vout { get; set; }
	}
}
