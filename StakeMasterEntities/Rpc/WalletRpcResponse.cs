// ******************************* Module Header *******************************
// Module Name: WalletRpcResponse.cs
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

	public sealed class WalletRpcResponse<T>
	{
		[JsonProperty(PropertyName = "error", Order = 2)]
		public WalletRpcError Error { get; set; }

		[JsonProperty(PropertyName = "id", Order = 1)]
		public int Id { get; set; }

		[JsonProperty(PropertyName = "result", Order = 0)]
		public T Result { get; set; }
	}
}
