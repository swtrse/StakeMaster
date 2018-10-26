// ******************************* Module Header *******************************
// Module Name: SettingsHelperTests.cs
// Project:     StakeMasterBussinessLogic.Test
// Copyright (c) Michael Goldfinger.
// 
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// *****************************************************************************

namespace StakeMasterBussinessLogic.Test
{
	using System;
	using System.Diagnostics.CodeAnalysis;
	using System.Linq;
	using JetBrains.Annotations;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using StakeMaster;
	using StakeMaster.BusinessLogic;

	[TestClass]
	[TestCategory("Unit")]
	[ExcludeFromCodeCoverage]
	public sealed class SettingsHelperTests
	{
		[TestMethod]
		[SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
		public void DisplayHelp_MessageIsNull()
		{
			//Arrange
			string message = null;
			//Act
			SettingsHelper.DisplayHelp(message);
			//Assert
			//If there is no exception everything is fine
		}

		[TestMethod]
		public void DisplayHelp_MessageIsSet()
		{
			//Arrange
			const string message = "A additional message.";
			//Act
			SettingsHelper.DisplayHelp(message);
			//Assert
			//If there is no exception everything is fine
		}

		[DataTestMethod]
		[DataRow(new[] {"-a=adr1", "-c=adr2", "-q=pass2", "-u=user", "-p=pass", "-o=localhost:1234", "-s=true", "-w=11", "-i=true", "-e=xadr1,xadr2"},
			"adr1",
			"adr2",
			"pass2",
			"user",
			"pass",
			"localhost:1234",
			true,
			11,
			true,
			new[] {"xadr1", "xadr2", "adr1", "adr2"})]
		[DataRow(new[] {"-a=adr1", "-c=adr2", "-q=pass2", "-u=user", "-p=pass", "-o=localhost:1234", "-s=true", "-w=13", "-i=true", "-e="},
			"adr1",
			"adr2",
			"pass2",
			"user",
			"pass",
			"localhost:1234",
			true,
			13,
			true,
			new[] {"adr1", "adr2"})]
		[DataRow(new[] {"-a=adr1", "-c=adr2", "-q=pass2", "-u=user", "-p=pass", "-o=localhost:1234", "-s=false", "-w=12", "-i=false", "-e=xadr3,xadr4"},
			"adr1",
			"adr2",
			"pass2",
			"user",
			"pass",
			"localhost:1234",
			false,
			12,
			false,
			new[] {"xadr3", "xadr4", "adr1", "adr2"})]
		[DataRow(new[]
		         {
			         "--stakeaddress=adr1",
			         "--collectaddress=adr2",
			         "--walletpassword=pass2",
			         "--user=user",
			         "--password=pass",
			         "--uri=localhost:1234",
			         "--stakes=true",
			         "--patience=11",
			         "--collectinputs=true",
			         "--excludeaddress=xadr1,xadr2"
		         },
			"adr1",
			"adr2",
			"pass2",
			"user",
			"pass",
			"localhost:1234",
			true,
			11,
			true,
			new[] {"xadr1", "xadr2", "adr1", "adr2"})]
		[DataRow(new[]
		         {
			         "--stakeaddress=adr1",
			         "--collectaddress=adr2",
			         "--walletpassword=pass2",
			         "--user=user",
			         "--password=pass",
			         "--uri=localhost:1234",
			         "--stakes=false",
			         "--patience=13",
			         "--collectinputs=false",
			         "--excludeaddress=xadr5,xadr6"
		         },
			"adr1",
			"adr2",
			"pass2",
			"user",
			"pass",
			"localhost:1234",
			false,
			13,
			false,
			new[] {"xadr5", "xadr6", "adr1", "adr2"})]
		public void Read_AllArgumentsPresent(string[] args,
		                                     string stakeAddress,
		                                     string collectAddress,
		                                     string walletPassword,
		                                     string user,
		                                     string password,
		                                     string uri,
		                                     bool stake,
		                                     int patience,
		                                     bool collect,
		                                     [NotNull] string[] exclude)
		{
			//Arrange
			var expectedUri = new Uri(uri, UriKind.Absolute);
			//Act
			Settings result = SettingsHelper.Read(args);
			//Assert
			Assert.AreEqual(stake, result.Stake.EditStakes);
			Assert.AreEqual(stakeAddress, result.Stake.DedicatedStakingAddress);
			Assert.AreEqual(collectAddress, result.Stake.DedicatedCollectingAddress);
			Assert.AreEqual(walletPassword, result.Stake.WalletPassword);
			Assert.AreEqual(patience, result.Stake.StakingPatience);
			Assert.AreEqual(collect, result.Address.CollectInputs);
			Assert.AreEqual(exclude.Length, result.Address.ExcludeAddresses.Length);
			foreach (string adr in exclude)
			{
				Assert.IsTrue(result.Address.ExcludeAddresses.Contains(adr));
			}

			Assert.AreEqual(user, result.Connection.RpcUser);
			Assert.AreEqual(password, result.Connection.RpcPassword);
			Assert.AreEqual(expectedUri, result.Connection.RpcUri);
		}

		[DataTestMethod]
		[DataRow(new[] {"-a=adr1", "-c=adr2", "-q=pass2", "-u=user", "-p=pass", "-o=localhost:1234"}, "adr1", "adr2", "pass2", "user", "pass", "localhost:1234")]
		[DataRow(new[] {"--stakeaddress=adr1", "--collectaddress=adr2", "--walletpassword=pass2", "--user=user", "--password=pass", "--uri=localhost:1234"},
			"adr1",
			"adr2",
			"pass2",
			"user",
			"pass",
			"localhost:1234")]
		public void Read_AllMandatoryArgumentsPresent(string[] args, string stakeAddress, string collectAddress, string walletpassword, string user, string password, string uri)
		{
			//Arrange
			var expectedUri = new Uri(uri, UriKind.Absolute);
			string[] exclude = {stakeAddress, collectAddress};
			//Act
			Settings result = SettingsHelper.Read(args);
			//Assert
			Assert.IsTrue(result.Stake.EditStakes);
			Assert.AreEqual(stakeAddress, result.Stake.DedicatedStakingAddress);
			Assert.AreEqual(collectAddress, result.Stake.DedicatedCollectingAddress);
			Assert.AreEqual(walletpassword, result.Stake.WalletPassword);
			Assert.AreEqual(7, result.Stake.StakingPatience);
			Assert.IsTrue(result.Address.CollectInputs);
			Assert.AreEqual(exclude.Length, result.Address.ExcludeAddresses.Length);
			foreach (string adr in exclude)
			{
				Assert.IsTrue(result.Address.ExcludeAddresses.Contains(adr));
			}

			Assert.AreEqual(user, result.Connection.RpcUser);
			Assert.AreEqual(password, result.Connection.RpcPassword);
			Assert.AreEqual(expectedUri, result.Connection.RpcUri);
		}

		[TestMethod]
		[ExpectedException(typeof(SettingsArgumentInvalidException))]
		[SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
		public void Read_ArgumentsNull()
		{
			//Arrange
			string[] args = null;
			//Act
			SettingsHelper.Read(args);
			//Assert
			//[ExpectedException(typeof(SettingsArgumentInvalidException))]
		}

		[DataTestMethod]
		[DataRow(new[] {"-?"})]
		[DataRow(new[] {"--help"})]
		[ExpectedException(typeof(SettingsArgumentInvalidException))]
		public void Read_HelpArgumentsPresent(string[] args)
		{
			//Arrange
			//Act
			try
			{
				SettingsHelper.Read(args);
			}
			catch (SettingsArgumentInvalidException e)
			{
				//Assert
				Assert.AreEqual(string.Empty, e.Argument);
				throw;
			}

			//Assert
			//[ExpectedException(typeof(SettingsArgumentInvalidException))]
		}

		[DataTestMethod]
		[ExpectedException(typeof(SettingsArgumentInvalidException))]
		[DataRow(new[] {"-a=adr1", "-c=adr2", "-q=pass2", "-u=user", "-p=pass", "-o=localhost:1234", "-a=adr3"}, "(-a=|--stakeaddress=)")]
		[DataRow(new[] {"-a=adr1", "-c=adr2", "-q=pass2", "-u=user", "-p=pass", "-o=localhost:1234", "--stakeaddress=adr3"}, "(-a=|--stakeaddress=)")]
		[DataRow(new[] {"--stakeaddress=adr1", "-c=adr2", "-q=pass2", "-u=user", "-p=pass", "-o=localhost:1234", "--stakeaddress=adr3"}, "(-a=|--stakeaddress=)")]
		[DataRow(new[] {"-a=adr1", "-c=adr2", "-q=pass2", "-u=user", "-p=pass", "-o=localhost:1234", "-c=adr3"}, "(-c=|--collectaddress=)")]
		[DataRow(new[] {"-a=adr1", "-c=adr2", "-q=pass2", "-u=user", "-p=pass", "-o=localhost:1234", "--collectaddress=adr3"}, "(-c=|--collectaddress=)")]
		[DataRow(new[] {"-a=adr1", "--collectaddress=adr2", "-q=pass2", "-u=user", "-p=pass", "-o=localhost:1234", "--collectaddress=adr3"}, "(-c=|--collectaddress=)")]
		[DataRow(new[] {"-a=adr1", "-c=adr2", "-q=pass2", "-u=user", "-p=pass", "-o=localhost:1234", "-q=pass2"}, "(-q=|--walletpassword=)")]
		[DataRow(new[] {"-a=adr1", "-c=adr2", "-q=pass2", "-u=user", "-p=pass", "-o=localhost:1234", "--walletpassword=pass2"}, "(-q=|--walletpassword=)")]
		[DataRow(new[] {"--stakeaddress=adr1", "-c=adr2", "--walletpassword=pass2", "-u=user", "-p=pass", "-o=localhost:1234", "--walletpassword=pass2"},
			"(-q=|--walletpassword=)")]
		[DataRow(new[] {"-a=adr1", "-c=adr2", "-q=pass2", "-u=user", "-p=pass", "-o=localhost:1234", "-u=user2"}, "(-u=|--user=)")]
		[DataRow(new[] {"-a=adr1", "-c=adr2", "-q=pass2", "-u=user", "-p=pass", "-o=localhost:1234", "--user=user2"}, "(-u=|--user=)")]
		[DataRow(new[] {"-a=adr1", "-c=adr2", "-q=pass2", "--user=user", "-p=pass", "-o=localhost:1234", "--user=user2"}, "(-u=|--user=)")]
		[DataRow(new[] {"-a=adr1", "-c=adr2", "-q=pass2", "-u=user", "-p=pass", "-o=localhost:1234", "-p=pass2"}, "(-p=|--password=)")]
		[DataRow(new[] {"-a=adr1", "-c=adr2", "-q=pass2", "-u=user", "-p=pass", "-o=localhost:1234", "--password=pass2"}, "(-p=|--password=)")]
		[DataRow(new[] {"-a=adr1", "-c=adr2", "-q=pass2", "-u=user", "--password=pass", "-o=localhost:1234", "--password=pass2"}, "(-p=|--password=)")]
		[DataRow(new[] {"-a=adr1", "-c=adr2", "-q=pass2", "-u=user", "-p=pass", "-o=localhost:1234", "-o=localhost2:1234"}, "(-o=|--uri=)")]
		[DataRow(new[] {"-a=adr1", "-c=adr2", "-q=pass2", "-u=user", "-p=pass", "-o=localhost:1234", "--uri=localhost2:1234"}, "(-o=|--uri=)")]
		[DataRow(new[] {"-a=adr1", "-c=adr2", "-q=pass2", "-u=user", "-p=pass", "--uri=localhost:1234", "--uri=localhost2:1234"}, "(-o=|--uri=)")]
		[DataRow(new[] {"-a=adr1", "-c=adr2", "-q=pass2", "-u=user", "-p=pass", "-o=localhost:1234", "-s=true", "-s=false"}, "(-s=|--stakes=)")]
		[DataRow(new[] {"-a=adr1", "-c=adr2", "-q=pass2", "-u=user", "-p=pass", "-o=localhost:1234", "-s=true", "--stakes=false"}, "(-s=|--stakes=)")]
		[DataRow(new[] {"-a=adr1", "-c=adr2", "-q=pass2", "-u=user", "-p=pass", "-o=localhost:1234", "--stakes=true", "--stakes=false"}, "(-s=|--stakes=)")]
		[DataRow(new[] {"-a=adr1", "-c=adr2", "-q=pass2", "-u=user", "-p=pass", "-o=localhost:1234", "-w=11", "-w=12"}, "(-w=|--patience=)")]
		[DataRow(new[] {"-a=adr1", "-c=adr2", "-q=pass2", "-u=user", "-p=pass", "-o=localhost:1234", "-w=11", "--patience=12"}, "(-w=|--patience=)")]
		[DataRow(new[] {"-a=adr1", "-c=adr2", "-q=pass2", "-u=user", "-p=pass", "-o=localhost:1234", "--patience=11", "--patience=12"}, "(-w=|--patience=)")]
		[DataRow(new[] {"-a=adr1", "-c=adr2", "-q=pass2", "-u=user", "-p=pass", "-o=localhost:1234", "-i=true", "-i=false"}, "(-i=|--collectinputs=)")]
		[DataRow(new[] {"-a=adr1", "-c=adr2", "-q=pass2", "-u=user", "-p=pass", "-o=localhost:1234", "-i=true", "--collectinputs=false"}, "(-i=|--collectinputs=)")]
		[DataRow(new[] {"-a=adr1", "-c=adr2", "-q=pass2", "-u=user", "-p=pass", "-o=localhost:1234", "--collectinputs=true", "--collectinputs=false"}, "(-i=|--collectinputs=)")]
		[DataRow(new[] {"-a=adr1", "-c=adr2", "-q=pass2", "-u=user", "-p=pass", "-o=localhost:1234", "-e=adr3,adr5", "-e=adr6,adr7"}, "(-e=|--excludeaddress=)")]
		[DataRow(new[] {"-a=adr1", "-c=adr2", "-q=pass2", "-u=user", "-p=pass", "-o=localhost:1234", "-e=adr3,adr5", "--excludeaddress=adr6,adr7"}, "(-e=|--excludeaddress=)")]
		[DataRow(new[] {"-a=adr1", "-c=adr2", "-q=pass2", "-u=user", "-p=pass", "-o=localhost:1234", "--excludeaddress=adr3,adr5", "--excludeaddress=adr6,adr7"},
			"(-e=|--excludeaddress=)")]
		public void Read_MandatoryArgumentDuplicate(string[] args, string argument)
		{
			//Arrange
			//Act
			try
			{
				SettingsHelper.Read(args);
			}
			catch (SettingsArgumentInvalidException e)
			{
				//Assert
				Assert.AreEqual(argument, e.Argument);
				throw;
			}

			//Assert
			//[ExpectedException(typeof(SettingsArgumentInvalidException))]
		}

		[DataTestMethod]
		[ExpectedException(typeof(SettingsArgumentInvalidException))]
		[DataRow(new[] {"-c=adr2", "-q=pass2", "-u=user", "-p=pass", "-o=localhost:1234"}, "(-a=|--stakeaddress=)")]
		[DataRow(new[] {"-a=adr1", "-q=pass2", "-u=user", "-p=pass", "-o=localhost:1234"}, "(-c=|--collectaddress=)")]
		[DataRow(new[] {"-a=adr1", "-c=adr2", "-u=user", "-p=pass", "-o=localhost:1234"}, "(-q=|--walletpassword=)")]
		[DataRow(new[] {"-a=adr1", "-c=adr2", "-q=pass2", "-p=pass", "-o=localhost:1234"}, "(-u=|--user=)")]
		[DataRow(new[] {"-a=adr1", "-c=adr2", "-q=pass2", "-u=user", "-o=localhost:1234"}, "(-p=|--password=)")]
		[DataRow(new[] {"-a=adr1", "-c=adr2", "-q=pass2", "-u=user", "-p=pass"}, "(-o=|--uri=)")]
		public void Read_MandatoryArgumentsMissing(string[] args, string argument)
		{
			//Arrange
			//Act
			try
			{
				SettingsHelper.Read(args);
			}
			catch (SettingsArgumentInvalidException e)
			{
				//Assert
				Assert.AreEqual(argument, e.Argument);
				throw;
			}

			//Assert
			//[ExpectedException(typeof(SettingsArgumentInvalidException))]
		}
	}
}
