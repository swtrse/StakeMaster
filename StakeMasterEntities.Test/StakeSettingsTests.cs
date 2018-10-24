// ******************************* Module Header *******************************
// Module Name: StakeSettingsTests.cs
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
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	[TestCategory("Unit")]
	[ExcludeFromCodeCoverage]
	public sealed class StakeSettingsTests
	{
		[TestMethod]
		public void Constructor_AllParametersSet()
		{
			//Arrange
			const string dedicatedStakingAddress = "DummyStakeAddress";
			const string dedicatedCollectionAddres = "DummyCollectingAddress";
			const int stakingPatience = 7;
			const bool editStakes = true;
			const string walletPassword = "DummyPassword";
			//Act
			var result = new StakeSettings(editStakes, dedicatedStakingAddress, dedicatedCollectionAddres, stakingPatience, walletPassword);
			//Assert
			Assert.AreEqual(dedicatedStakingAddress, result.DedicatedStakingAddress);
			Assert.AreSame(dedicatedCollectionAddres, result.DedicatedCollectingAddress);
			Assert.AreEqual(stakingPatience, result.StakingPatience);
			Assert.AreEqual(editStakes, result.EditStakes);
			Assert.AreEqual(walletPassword, result.WalletPassword);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		[SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
		[SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
		public void Constructor_ParameterdedicatedCollectionAddresIsNull()
		{
			//Arrange
			const string dedicatedStakingAddress = "DummyStakeAddress";
			const string dedicatedCollectionAddres = null;
			const int stakingPatience = 7;
			const bool editStakes = true;
			const string walletPassword = "DummyPassword";
			//Act
			new StakeSettings(editStakes, dedicatedStakingAddress, dedicatedCollectionAddres, stakingPatience, walletPassword);
			//Assert
			//Will be handled through ExpectedException.
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		[SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
		[SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
		public void Constructor_ParameterDedicatedStakingAddressIsNull()
		{
			//Arrange
			const string dedicatedStakingAddress = null;
			const string dedicatedCollectionAddres = "DummyCollectingAddress";
			const int stakingPatience = 7;
			const bool editStakes = true;
			const string walletPassword = "DummyPassword";
			//Act
			new StakeSettings(editStakes, dedicatedStakingAddress, dedicatedCollectionAddres, stakingPatience, walletPassword);
			//Assert
			//Will be handled through ExpectedException.
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		[SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
		[SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
		public void Constructor_ParameterWalletPasswordIsNull()
		{
			//Arrange
			const string dedicatedStakingAddress = "DummyStakeAddress";
			const string dedicatedCollectionAddres = "DummyCollectingAddress";
			const int stakingPatience = 7;
			const bool editStakes = true;
			const string walletPassword = null;
			//Act
			new StakeSettings(editStakes, dedicatedStakingAddress, dedicatedCollectionAddres, stakingPatience, walletPassword);
			//Assert
			//Will be handled through ExpectedException.
		}
	}
}
