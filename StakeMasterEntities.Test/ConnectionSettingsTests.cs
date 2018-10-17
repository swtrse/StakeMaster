// ******************************* Module Header *******************************
// Module Name: ConnectionSettingsTests.cs
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
	public sealed class ConnectionSettingsTests
	{
		[TestMethod]
		public void Constructor_AllParametersSet()
		{
			//Arrange
			var rpcUri = new Uri("http://someuriitis:12345", UriKind.Absolute);
			const string rpcUser = "DummyUser";
			const string rpcPassword = "DummyPassword";
			//Act
			var result = new ConnectionSettings(rpcUri, rpcUser, rpcPassword);
			//Assert
			Assert.AreSame(rpcUri, result.RpcUri);
			Assert.AreEqual(rpcUser, result.RpcUser);
			Assert.AreEqual(rpcPassword, result.RpcPassword);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		[SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
		[SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
		public void Constructor_ParameterRpcPasswordIsNull()
		{
			//Arrange
			var rpcUri = new Uri("http://someuriitis:12345", UriKind.Absolute);
			const string rpcUser = "DummyUser";
			string rpcPassword = null;
			//Act
			new ConnectionSettings(rpcUri, rpcUser, rpcPassword);
			//Assert
			//Will be handled through ExpectedException.
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		[SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
		[SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
		public void Constructor_ParameterRpcUriIsNull()
		{
			//Arrange
			Uri rpcUri = null;
			const string rpcUser = "DummyUser";
			const string rpcPassword = "DummyPassword";
			//Act
			new ConnectionSettings(rpcUri, rpcUser, rpcPassword);
			//Assert
			//Will be handled through ExpectedException.
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		[SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
		[SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
		public void Constructor_ParameterRpcUserIsNull()
		{
			//Arrange
			var rpcUri = new Uri("http://someuriitis:12345", UriKind.Absolute);
			const string rpcUser = null;
			const string rpcPassword = "DummyPassword";
			//Act
			new ConnectionSettings(rpcUri, rpcUser, rpcPassword);
			//Assert
			//Will be handled through ExpectedException.
		}
	}
}
