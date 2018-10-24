using System;
using System.Collections.Generic;
using System.Text;

namespace StakeMaster.Rpc
{
	public class SignRawTransactionResponse
	{
		public string Hex { get; set; }
		public bool Complete { get; set; }
	}
}
