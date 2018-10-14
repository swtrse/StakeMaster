// ******************************* Module Header *******************************
// Module Name: SettingsTests.cs
// Project:     StakeMasterEntities.Test
// Copyright (c) Michael Goldfinger.
// 
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// *****************************************************************************

namespace StakeMaster.Entities
{
	using System;
	using System.Diagnostics.CodeAnalysis;
	using System.Security;
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	[TestCategory("Unit")]
	[ExcludeFromCodeCoverage]
	public sealed class SettingsTests
	{
		[TestMethod]
		public void Constructor_AllParametersSet()
		{
			//Arrange
			var stake = new StakeSettings(true, "DummyStakeAddress", "DummyCollectingAddress", 7);
			var address = new OtherAddressSettings(true, null);
			var connection = new ConnectionSettings(new Uri("http://someuriitis:12345", UriKind.Absolute), "DummyUser", new SecureString());
			//Act
			var result = new Settings(stake, address, connection);
			//Assert
			Assert.AreSame(stake, result.Stake);
			Assert.AreSame(address, result.Address);
			Assert.AreSame(connection, result.Connection);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		[SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
		[SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
		public void Constructor_ParameterAddressIsNull()
		{
			//Arrange
			var stake = new StakeSettings(true, "DummyStakeAddress", "DummyCollectingAddress", 7);
			OtherAddressSettings address = null;
			var connection = new ConnectionSettings(new Uri("http://someuriitis:12345", UriKind.Absolute), "DummyUser", new SecureString());
			//Act
			new Settings(stake, address, connection);
			//Assert
			//Will be handled through ExpectedException.
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		[SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
		[SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
		public void Constructor_ParameterConnectionIsNull()
		{
			//Arrange
			var stake = new StakeSettings(true, "DummyStakeAddress", "DummyCollectingAddress", 7);
			var address = new OtherAddressSettings(true, null);
			ConnectionSettings connection = null;
			//Act
			new Settings(stake, address, connection);
			//Assert
			//Will be handled through ExpectedException.
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		[SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
		[SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
		public void Constructor_ParameterStakeIsNull()
		{
			//Arrange
			StakeSettings stake = null;
			var address = new OtherAddressSettings(true, null);
			var connection = new ConnectionSettings(new Uri("http://someuriitis:12345", UriKind.Absolute), "DummyUser", new SecureString());
			//Act
			new Settings(stake, address, connection);
			//Assert
			//Will be handled through ExpectedException.
		}
	}
}
