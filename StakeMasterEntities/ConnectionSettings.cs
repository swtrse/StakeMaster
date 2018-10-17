// ******************************* Module Header *******************************
// Module Name: ConnectionSettings.cs
// Project:     StakeMasterEntities
// Copyright (c) Michael Goldfinger.
// 
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// *****************************************************************************

namespace StakeMaster
{
	using System;
	using System.Security;
	using JetBrains.Annotations;

	/// <summary>
	///     Contains settings for the connection to the wallet. This class cannot be inherited.
	/// </summary>
	[PublicAPI]
	public sealed class ConnectionSettings
	{
		/// <summary>
		///     Initialzes a new instance of the <see cref="ConnectionSettings" /> class.
		/// </summary>
		/// <param name="rpcUri">The uri where the wallet is listening to connections.</param>
		/// <param name="rpcUser">The user that is used for authentication.</param>
		/// <param name="rpcPassword">The password that is used for authentication.</param>
		/// <exception cref="ArgumentNullException">
		///     <paramref name="rpcUri" /> is
		///     <value>null</value>
		///     .
		/// </exception>
		/// <exception cref="ArgumentNullException">
		///     <paramref name="rpcUser" /> is
		///     <value>null</value>
		///     .
		/// </exception>
		/// <exception cref="ArgumentNullException">
		///     <paramref name="rpcPassword" /> is
		///     <value>null</value>
		///     .
		/// </exception>
		public ConnectionSettings([NotNull] Uri rpcUri, [NotNull] string rpcUser, [NotNull] string rpcPassword)
		{
			RpcPassword = rpcPassword ?? throw new ArgumentNullException(nameof(rpcPassword));
			RpcUri = rpcUri ?? throw new ArgumentNullException(nameof(rpcUri));
			RpcUser = rpcUser ?? throw new ArgumentNullException(nameof(rpcUser));
		}

		/// <summary>
		///     Gets the password for the authentication.
		/// </summary>
		[NotNull]
		public string RpcPassword { get; }

		/// <summary>
		///     Gets the uri for the connection.
		/// </summary>
		[NotNull]
		public Uri RpcUri { get; }

		/// <summary>
		///     Gets the user for the authentication.
		/// </summary>
		[NotNull]
		public string RpcUser { get; }
	}
}
