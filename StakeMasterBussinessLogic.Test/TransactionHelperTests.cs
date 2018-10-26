// ******************************* Module Header *******************************
// Module Name: TransactionHelperTests.cs
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
	using System.Globalization;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using StakeMaster.BusinessLogic;

	[TestClass]
	[TestCategory("Unit")]
	[ExcludeFromCodeCoverage]
	public sealed class TransactionHelperTests
	{
		[TestMethod]
		public void Constructor_SetBaseDate()
		{
			//Arrange
			var expectedBaseDate = new DateTime(2016, 11, 20);
			//Act
			var result = new TransactionHelper(1, 1, 581, 1, new DateTime(2016, 11, 20), 60);
			//Assert
			Assert.AreEqual(expectedBaseDate, result.BaseDate);
		}

		[TestMethod]
		public void Constructor_SetConnectionTimeout()
		{
			//Arrange
			const int expectedTimeout = 60;
			//Act
			var result = new TransactionHelper(1, 1, 581, 1, DateTime.Now, 60);
			//Assert
			Assert.AreEqual(expectedTimeout, result.ConnectionTimeout);
		}

		[TestMethod]
		public void Constructor_SetFreeByteLimit()
		{
			//Arrange
			const int expectedFree = 985;
			//Act
			var result = new TransactionHelper(1, 1, 1, 985, DateTime.Now, 60);
			//Assert
			Assert.AreEqual(expectedFree, result.FreeTransactionByteLimit);
		}

		[TestMethod]
		public void Constructor_SetInputSize()
		{
			//Arrange
			const int expectedInputs = 167;
			//Act
			var result = new TransactionHelper(167, 1, 1, 1, DateTime.Now, 60);
			//Assert
			Assert.AreEqual(expectedInputs, result.InputSize);
		}

		[TestMethod]
		public void Constructor_SetOutputSize()
		{
			//Arrange
			const int expectedOutputs = 289;
			//Act
			var result = new TransactionHelper(1, 289, 1, 1, DateTime.Now, 60);
			//Assert
			Assert.AreEqual(expectedOutputs, result.OutputSize);
		}

		[TestMethod]
		public void Constructor_SetOverheadSize()
		{
			//Arrange
			const int expectedOverhead = 581;
			//Act
			var result = new TransactionHelper(1, 1, 581, 1, DateTime.Now, 60);
			//Assert
			Assert.AreEqual(expectedOverhead, result.TransactionOverhead);
		}

		[DataTestMethod]
		[DataRow(23, 3, 62, 200, 3, "2017-05-16", 60, 5)]
		[DataRow(17, 12, 100, 1200, 20, "2017-05-16", 60, 50)]
		[DataRow(10, 11, 12, 77, 5, "2017-05-16", 60, 1)]
		public void GetMaxPossibleInputCountForFreeTransaction_Default(int inputSize,
		                                                               int outputSize,
		                                                               int overhead,
		                                                               int byteLimit,
		                                                               int outputs,
		                                                               string date,
		                                                               int timeout,
		                                                               int expectedResult)
		{
			//Arrange
			DateTime dateTime = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
			var test = new TransactionHelper(inputSize, outputSize, overhead, byteLimit, dateTime, timeout);
			//Act
			int result = test.GetMaxPossibleInputCountForFreeTransaction(outputs);
			//Assert
			Assert.AreEqual(expectedResult, result);
		}

		[TestMethod]
		[ExpectedException(typeof(FreeTransactionNotPossibleException))]
		public void GetMaxPossibleInputCountForFreeTransaction_NotPossibleException()
		{
			//Arrange
			var test = new TransactionHelper(10, 11, 12, 76, DateTime.Now, 60);
			const int outputs = 5;
			//Act
			test.GetMaxPossibleInputCountForFreeTransaction(outputs);
			//Assert
			//[ExpectedException(typeof(FreeTransactionNotPossibleException))]
		}

		[DataTestMethod]
		[DataRow(23, 3, 62, 200, 3, "2017-05-16", 60, 23)]
		[DataRow(17, 12, 100, 1200, 20, "2017-05-16", 60, 63)]
		[DataRow(10, 11, 12, 73, 5, "2017-05-16", 60, 1)]
		public void GetMaxPossibleOutputCountForFreeTransaction_Default(int inputSize,
		                                                                int outputSize,
		                                                                int overhead,
		                                                                int byteLimit,
		                                                                int inputs,
		                                                                string date,
		                                                                int timeout,
		                                                                int expectedResult)
		{
			//Arrange
			DateTime dateTime = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
			var test = new TransactionHelper(inputSize, outputSize, overhead, byteLimit, dateTime, timeout);
			//Act
			int result = test.GetMaxPossibleOutputCountForFreeTransaction(inputs);
			//Assert
			Assert.AreEqual(expectedResult, result);
		}

		[TestMethod]
		[ExpectedException(typeof(FreeTransactionNotPossibleException))]
		public void GetMaxPossibleOutputCountForFreeTransaction_NotPossibleException()
		{
			//Arrange5
			var test = new TransactionHelper(10, 11, 12, 72, DateTime.Now, 60);
			const int inputs = 5;
			//Act
			test.GetMaxPossibleOutputCountForFreeTransaction(inputs);
			//Assert
			//[ExpectedException(typeof(FreeTransactionNotPossibleException))]
		}

		[TestMethod]
		public void GetTransactionSize_Default()
		{
			//Arrange
			var test = new TransactionHelper(12, 3, 50, 200, DateTime.Now, 60);
			const int inputs = 5;
			const int outputs = 7;
			const int expectedSize = 131;
			//Act
			int result = test.GetTransactionSize(inputs, outputs);
			//Assert
			Assert.AreEqual(expectedSize, result);
		}
	}
}
