// ******************************* Module Header *******************************
// Module Name: OtherAddressSettingsTests.cs
// Project:     StakeMasterEntities.Test
// Copyright (c) Michael Goldfinger.
// 
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// *****************************************************************************

namespace StakeMaster.Entities
{
	using System.Diagnostics.CodeAnalysis;
	using System.Linq;
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	[TestCategory("Unit")]
	[ExcludeFromCodeCoverage]
	public sealed class OtherAddressSettingsTests
	{
		[TestMethod]
		public void Constructor_AllParametersSet()
		{
			//Arrange
			var excludeAddresses = new[] {"Dummy1", "Dummy2"};
			//Act
			var result = new OtherAddressSettings(false, excludeAddresses);
			//Assert
			Assert.IsFalse(result.CollectInputs);
			Assert.AreEqual(2, result.ExcludeAddresses.Length);
			Assert.IsTrue(result.ExcludeAddresses.Contains(excludeAddresses[0]));
			Assert.IsTrue(result.ExcludeAddresses.Contains(excludeAddresses[1]));
		}

		[TestMethod]
		[SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
		public void Constructor_ParameterAddressIsNull()
		{
			//Arrange
			string[] excludeAddresses = null;
			//Act
			var result = new OtherAddressSettings(true, excludeAddresses);
			//Assert
			Assert.IsTrue(result.CollectInputs);
			Assert.IsNotNull(result.ExcludeAddresses);
			Assert.AreEqual(0, result.ExcludeAddresses.Length);
		}
	}
}