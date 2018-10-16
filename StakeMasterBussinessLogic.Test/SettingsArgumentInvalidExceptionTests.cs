// ******************************* Module Header *******************************
// Module Name: SettingsArgumentInvalidExceptionTests.cs
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
	using System.IO;
	using System.Runtime.Serialization;
	using System.Runtime.Serialization.Formatters.Binary;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using StakeMaster.BusinessLogic;

	[TestClass]
	[TestCategory("Unit")]
	[ExcludeFromCodeCoverage]
	public sealed class SettingsArgumentInvalidExceptionTests
	{
		[TestMethod]
		public void Constructor_ArgumentAndExceptionSet()
		{
			//Arrange
			const string expectedArgument = "-dummy";
			string expectedMessage = $"The argument given is invalid or unknown in the current context.{Environment.NewLine}Argument: -dummy";
			//Act
			var result = new SettingsArgumentInvalidException("-dummy", new Exception("Inner"));
			//Assert
			Assert.AreEqual(expectedArgument, result.Argument);
			Assert.IsNotNull(result.InnerException);
			Assert.AreEqual(expectedMessage, result.Message);
		}

		[TestMethod]
		public void Constructor_ArgumentAndMessageSet()
		{
			//Arrange
			const string expectedArgument = "-dummy";
			string expectedMessage = $"Message me.{Environment.NewLine}Argument: -dummy";
			//Act
			var result = new SettingsArgumentInvalidException("-dummy", "Message me.");
			//Assert
			Assert.AreEqual(expectedArgument, result.Argument);
			Assert.IsNull(result.InnerException);
			Assert.AreEqual(expectedMessage, result.Message);
		}

		[TestMethod]
		public void Constructor_ArgumentMessageAndExceptionSet()
		{
			//Arrange
			const string expectedArgument = "-dummy";
			string expectedMessage = $"Message me.{Environment.NewLine}Argument: -dummy";
			//Act
			var result = new SettingsArgumentInvalidException("-dummy", "Message me.", new Exception("Inner"));
			//Assert
			Assert.AreEqual(expectedArgument, result.Argument);
			Assert.IsNotNull(result.InnerException);
			Assert.AreEqual(expectedMessage, result.Message);
		}

		[TestMethod]
		public void Constructor_ArgumentSet()
		{
			//Arrange
			const string expectedArgument = "-dummy";
			string expectedMessage = $"The argument given is invalid or unknown in the current context.{Environment.NewLine}Argument: -dummy";
			//Act
			var result = new SettingsArgumentInvalidException("-dummy");
			//Assert
			Assert.AreEqual(expectedArgument, result.Argument);
			Assert.IsNull(result.InnerException);
			Assert.AreEqual(expectedMessage, result.Message);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		[SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
		public void Constructor_GetObjectData_InfoIsNull()
		{
			// Arrange
			var result = new SettingsArgumentInvalidException("-dummy", "message");
			// Act
			result.GetObjectData(null, new StreamingContext());
			// Assert
			// [ExpectedException(typeof(ArgumentNullException))]
		}

		[TestMethod]
		public void Constructor_NoParameters()
		{
			//Arrange
			const string expectedMessage = "The argument given is invalid or unknown in the current context.";
			//Act
			var result = new SettingsArgumentInvalidException();
			//Assert
			Assert.IsNull(result.Argument);
			Assert.IsNull(result.InnerException);
			Assert.AreEqual(expectedMessage, result.Message);
		}

		[TestMethod]
		public void Constructor_SerializationDeserialization()
		{
			// Arrange
			var innerEx = new Exception("foo");
			var originalException = new SettingsArgumentInvalidException("-dummy", "message", innerEx);
			var ms = new MemoryStream();
			var formatter = new BinaryFormatter();
			// Act
			formatter.Serialize(ms, originalException);
			ms.Position = 0;
			var deserializedException = (SettingsArgumentInvalidException) formatter.Deserialize(ms);

			// Assert
			Assert.AreEqual(originalException.Argument, deserializedException.Argument);
			Assert.AreEqual(originalException.InnerException.Message, deserializedException.InnerException.Message);
			Assert.AreEqual(originalException.Message, deserializedException.Message);
		}
	}
}
