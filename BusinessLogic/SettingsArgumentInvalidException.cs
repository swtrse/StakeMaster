// ******************************* Module Header *******************************
// Module Name: SettingsArgumentInvalidException.cs
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

	/// <summary>Represents errors that occur when unexpected arguments are found.</summary>
	/// <inheritdoc />
	[PublicAPI]
	[Serializable]
	public sealed class SettingsArgumentInvalidException : Exception
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="SettingsArgumentInvalidException" /> class.
		/// </summary>
		/// <inheritdoc />
		public SettingsArgumentInvalidException() : base(Resources.SettingsArgumentInvalidException_Generic) { }

		/// <summary>
		///     Initializes a new instance of the <see cref="SettingsArgumentInvalidException" /> class.
		/// </summary>
		/// <param name="argument">
		///     The argument that causes the exception.
		/// </param>
		public SettingsArgumentInvalidException(string argument) : base(Resources.SettingsArgumentInvalidException_Generic)
		{
			Argument = argument;
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="SettingsArgumentInvalidException" /> class.
		/// </summary>
		/// <param name="argument">
		///     The argument that causes the exception.
		/// </param>
		/// <param name="innerException">
		///     The exception that is the cause of the current exception, or a null reference (
		///     <value>Nothing</value>
		///     in Visual Basic) if no inner exception is specified.
		/// </param>
		/// <inheritdoc />
		public SettingsArgumentInvalidException(string argument, Exception innerException) : base(Resources.SettingsArgumentInvalidException_Generic, innerException)
		{
			Argument = argument;
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="SettingsArgumentInvalidException" /> class.
		/// </summary>
		/// <param name="argument">
		///     The argument that causes the exception.
		/// </param>
		/// <param name="message">
		///     The custom message that describes the error.
		/// </param>
		/// <inheritdoc />
		public SettingsArgumentInvalidException(string argument, string message) : base(message)
		{
			Argument = argument;
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="SettingsArgumentInvalidException" /> class.
		/// </summary>
		/// <param name="argument">
		///     The argument that raised this exception.
		/// </param>
		/// <param name="message">
		///     The custom message that describes the error.
		/// </param>
		/// <param name="innerException">
		///     The exception that is the cause of the current exception, or a null reference (
		///     <value>Nothing</value>
		///     in Visual Basic) if no inner exception is specified.
		/// </param>
		/// <inheritdoc />
		public SettingsArgumentInvalidException(string argument, string message, Exception innerException) : base(message, innerException)
		{
			Argument = argument;
		}

		/// <inheritdoc />
		private SettingsArgumentInvalidException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
		{
			Argument = info.GetString("Argument");
		}

		/// <summary>
		///     Holds the argument that causes the exception.
		/// </summary>
		public string Argument { get; set; }

		/// <inheritdoc />
		public override string Message
		{
			get
			{
				string s = base.Message;
				return string.IsNullOrEmpty(Argument) ? s : $"{s}{Environment.NewLine}{Resources.SettingsArgumentInvalidException_Argument_Name} {Argument}";
			}
		}

		/// <inheritdoc />
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("Argument", Argument);
		}
	}
}
