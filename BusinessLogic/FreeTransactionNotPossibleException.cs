// ******************************* Module Header *******************************
// Module Name: FreeTransactionNotPossibleException.cs
// Project:     StakeMasterBusinessLogic
// Copyright (c) Michael Goldfinger.
// 
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// *****************************************************************************

namespace StakeMaster.BusinessLogic
{
	using System;
	using System.Runtime.Serialization;
	using System.Security.Permissions;
	using JetBrains.Annotations;
	using Properties;

	/// <summary>Represents errors that occur when no zero fee transaction is possible.</summary>
	/// <inheritdoc />
	[PublicAPI]
	[Serializable]
	public sealed class FreeTransactionNotPossibleException : Exception
	{
		/// <summary>
		///     Gets the calculated size of the transaction.
		/// </summary>
		public int CalculatedTransactionSize { get; }

		/// <summary>
		///     Gets the actual size limit for zero fee transactions.
		/// </summary>
		public int FreeTransactionByteLimit { get; }

		/// <summary>
		///     Gets the number of inputs for the transaction.
		/// </summary>
		public int InputCount { get; }

		/// <inheritdoc />
		[NotNull]
		public override string Message =>
			$"{base.Message}{Environment.NewLine}{Resources.FreeTransactionNotPossibleException_InputCount_Name} {InputCount}{Environment.NewLine}{Resources.FreeTransactionNotPossibleException_OutputCount_Name} {OutputCount}{Environment.NewLine}{Resources.FreeTransactionNotPossibleException_Size_Name} {CalculatedTransactionSize}{Environment.NewLine}{Resources.FreeTransactionNotPossibleException_Free_Name} {FreeTransactionByteLimit}";

		/// <summary>
		///     Gets the number of outputs for the transaction.
		/// </summary>
		public int OutputCount { get; }

		/// <summary>
		///     Initializes a new instance of the <see cref="FreeTransactionNotPossibleException" /> class.
		/// </summary>
		/// <param name="inputCount">The number of inputs for the transaction.</param>
		/// <param name="outputCount">The number of outputs for the transaction.</param>
		/// <param name="calculatedTransactionSize">The calculated size of the transaction.</param>
		/// <param name="freeTransactionByteLimit">The actual size limit for zero fee transactions.</param>
		/// <inheritdoc />
		public FreeTransactionNotPossibleException(int inputCount, int outputCount, int calculatedTransactionSize, int freeTransactionByteLimit) :
			base(Resources.FreeTransactionNotPossibleException_Generic)
		{
			InputCount = inputCount;
			OutputCount = outputCount;
			CalculatedTransactionSize = calculatedTransactionSize;
			FreeTransactionByteLimit = freeTransactionByteLimit;
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="FreeTransactionNotPossibleException" /> class.
		/// </summary>
		/// <param name="inputCount">The number of inputs for the transaction.</param>
		/// <param name="outputCount">The number of outputs for the transaction.</param>
		/// <param name="calculatedTransactionSize">The calculated size of the transaction.</param>
		/// <param name="freeTransactionByteLimit">The actual size limit for zero fee transactions.</param>
		/// <param name="innerException">
		///     The exception that is the cause of the current exception, or a null reference (
		///     <value>Nothing</value>
		///     in Visual Basic) if no inner exception is specified.
		/// </param>
		/// <inheritdoc />
		public FreeTransactionNotPossibleException(int inputCount, int outputCount, int calculatedTransactionSize, int freeTransactionByteLimit, Exception innerException) :
			base(Resources.FreeTransactionNotPossibleException_Generic, innerException)
		{
			InputCount = inputCount;
			OutputCount = outputCount;
			CalculatedTransactionSize = calculatedTransactionSize;
			FreeTransactionByteLimit = freeTransactionByteLimit;
		}

		/// <inheritdoc />
		private FreeTransactionNotPossibleException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
		{
			InputCount = info.GetInt32("InputCount");
			OutputCount = info.GetInt32("OutputCount");
			CalculatedTransactionSize = info.GetInt32("CalculatedTransactionSize");
			FreeTransactionByteLimit = info.GetInt32("FreeTransactionByteLimit");
		}

		/// <inheritdoc />
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("InputCount", InputCount);
			info.AddValue("OutputCount", OutputCount);
			info.AddValue("CalculatedTransactionSize", CalculatedTransactionSize);
			info.AddValue("FreeTransactionByteLimit", FreeTransactionByteLimit);
		}
	}
}