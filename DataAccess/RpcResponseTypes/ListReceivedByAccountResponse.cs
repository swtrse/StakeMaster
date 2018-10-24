// ******************************* Module Header *******************************
// Module Name: ListReceivedByAccountResponse.cs
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

	public class ListReceivedByAccountResponse
	{
		public string Account { get; set; }
		public string Address { get; set; }
		public decimal Amount { get; set; }
		public int Confirmations { get; set; }
		public List<string> TxIds { get; set; }
	}
}
