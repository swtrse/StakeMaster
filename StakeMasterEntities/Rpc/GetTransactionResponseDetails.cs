// ******************************* Module Header *******************************
// Module Name: GetTransactionResponseDetails.cs
// Project:     StakeMasterEntities
// Copyright (c) Michael Goldfinger.
// 
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// *****************************************************************************

namespace StakeMaster.Rpc
{
	public sealed class GetTransactionResponseDetails
	{
		public string Account { get; set; }
		public string Address { get; set; }
		public decimal Amount { get; set; }
		public string Category { get; set; }
		public decimal Fee { get; set; }
		public string Label { get; set; }
		public int Vout { get; set; }
	}
}
