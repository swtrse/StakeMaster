using System;
using System.Collections.Generic;
using System.Text;

namespace StakeMasterBussinessLogic.Test
{
	using System.Diagnostics.CodeAnalysis;
	using System.IO;
	using System.Runtime.Serialization;
	using System.Runtime.Serialization.Formatters.Binary;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using StakeMaster.BusinessLogic;

	[TestClass]
	[TestCategory("Unit")]
	[ExcludeFromCodeCoverage]
	public class FreeTransactionNotPossibleExceptionTests
	{
		[TestMethod]
		public void Constructor_ArgumentsAndExceptionSet()
		{
			//Arrange
			const int expectedInput = 1;
			const int expectedOutput = 2;
			const int expectedSize = 3;
			const int expectedFree = 4;
			string expectedMessage = $"A free Transaction is not possible with the given values.{Environment.NewLine}Inputs: 1{Environment.NewLine}Outputs: 2{Environment.NewLine}Calculated transaction size: 3{Environment.NewLine}Zero fee transaction size limit: 4";
			//Act
			var result = new FreeTransactionNotPossibleException(1, 2, 3, 4, new Exception("inner"));
			//Assert
			Assert.AreEqual(expectedInput, result.InputCount);
			Assert.AreEqual(expectedOutput, result.OutputCount);
			Assert.AreEqual(expectedSize, result.CalculatedTransactionSize);
			Assert.AreEqual(expectedFree, result.FreeTransactionByteLimit);
			Assert.IsNotNull(result.InnerException);
			Assert.AreEqual(expectedMessage, result.Message);
		}

		[TestMethod]
		public void Constructor_ArgumentsSet()
		{
			//Arrange
			const int expectedInput = 1;
			const int expectedOutput = 2;
			const int expectedSize = 3;
			const int expectedFree = 4;
			string expectedMessage = $"A free Transaction is not possible with the given values.{Environment.NewLine}Inputs: 1{Environment.NewLine}Outputs: 2{Environment.NewLine}Calculated transaction size: 3{Environment.NewLine}Zero fee transaction size limit: 4";
			//Act
			var result = new FreeTransactionNotPossibleException(1, 2, 3, 4);
			//Assert
			Assert.AreEqual(expectedInput, result.InputCount);
			Assert.AreEqual(expectedOutput, result.OutputCount);
			Assert.AreEqual(expectedSize, result.CalculatedTransactionSize);
			Assert.AreEqual(expectedFree, result.FreeTransactionByteLimit);
			Assert.IsNull(result.InnerException);
			Assert.AreEqual(expectedMessage, result.Message);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		[SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
		public void Constructor_GetObjectData_InfoIsNull()
		{
			// Arrange
			var result = new FreeTransactionNotPossibleException(1, 2, 3, 4);
			// Act
			result.GetObjectData(null, new StreamingContext());
			// Assert
			// [ExpectedException(typeof(ArgumentNullException))]
		}

		[TestMethod]
		public void Constructor_SerializationDeserialization()
		{
			// Arrange
			var innerEx = new Exception("foo");
			var originalException = new FreeTransactionNotPossibleException(1, 2, 3, 4, innerEx);
			var ms = new MemoryStream();
			var formatter = new BinaryFormatter();
			// Act
			formatter.Serialize(ms, originalException);
			ms.Position = 0;
			var deserializedException = (FreeTransactionNotPossibleException)formatter.Deserialize(ms);

			// Assert
			Assert.AreEqual(originalException.InputCount, deserializedException.InputCount);
			Assert.AreEqual(originalException.OutputCount, deserializedException.OutputCount);
			Assert.AreEqual(originalException.CalculatedTransactionSize, deserializedException.CalculatedTransactionSize);
			Assert.AreEqual(originalException.FreeTransactionByteLimit, deserializedException.FreeTransactionByteLimit);
			Assert.AreEqual(originalException.InnerException.Message, deserializedException.InnerException.Message);
			Assert.AreEqual(originalException.Message, deserializedException.Message);
		}
	}
}
