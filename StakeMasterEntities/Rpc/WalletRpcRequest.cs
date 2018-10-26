// ******************************* Module Header *******************************
// Module Name: WalletRpcRequest.cs
// Project:     StakeMasterEntities
// Copyright (c) Michael Goldfinger.
// 
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// *****************************************************************************

namespace StakeMaster.Rpc
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using JetBrains.Annotations;
	using Newtonsoft.Json;

	public sealed class WalletRpcRequest
	{
		/// <inheritdoc />
		public WalletRpcRequest(int id, string method, [CanBeNull] params object[] parameters)
		{
			Method = method;
			Parameters = (parameters ?? new object[0]).ToList();
			Id = id;
		}

		[JsonProperty(PropertyName = "id", Order = 2)]
		public int Id { get; set; }

		[JsonProperty(PropertyName = "method", Order = 0)]
		public string Method { get; set; }

		[JsonProperty(PropertyName = "params", Order = 1)]
		public IList<object> Parameters { get; set; }

		[NotNull]
		public byte[] GetBytes()
		{
			string json = JsonConvert.SerializeObject(this);
			return Encoding.UTF8.GetBytes(json);
		}
	}
}
