// ******************************* Module Header *******************************
// Module Name: CreateRawTransactionInput.cs
// Project:     StakeMasterEntities
// Copyright (c) Michael Goldfinger.
// 
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// *****************************************************************************

namespace StakeMaster.Rpc
{
	using Newtonsoft.Json;

	public sealed class CreateRawTransactionInput
	{
		[JsonProperty("txid")]
		public string TxId { get; set; }

		[JsonProperty("vout")]
		public int Vout { get; set; }
	}
}
