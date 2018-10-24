using System;
using System.Collections.Generic;
using System.Text;

namespace StakeMaster.Rpc
{
	public class CreateRawTransactionRequest
	{
		public CreateRawTransactionRequest(IList<CreateRawTransactionInput> inputs, IDictionary<string, decimal> outputs)
		{
			Inputs = inputs;
			Outputs = outputs;
		}

		public IList<CreateRawTransactionInput> Inputs { get; }
		public IDictionary<string, decimal> Outputs { get; }
	}
}
