// ******************************* Module Header *******************************
// Module Name: Settings.cs
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
	using JetBrains.Annotations;

	/// <summary>
	///     Contains all settings for the maintanace of a staking masternode coin.
	/// </summary>
	[PublicAPI]
	public sealed class Settings
	{
		/// <summary>
		///     Initialzes a new instance of the <see cref="Settings" /> class.
		/// </summary>
		/// <param name="stake">The settings for stake maintanace.</param>
		/// <param name="address">The settings for maintanace of other public addresses in the wallet.</param>
		/// <param name="connection">The settings for the connection to the wallet.</param>
		/// <exception cref="ArgumentNullException">
		///     <paramref name="stake" /> is
		///     <value>null</value>
		///     .
		/// </exception>
		/// <exception cref="ArgumentNullException">
		///     <paramref name="address" /> is
		///     <value>null</value>
		///     .
		/// </exception>
		/// <exception cref="ArgumentNullException">
		///     <paramref name="connection" /> is
		///     <value>null</value>
		///     .
		/// </exception>
		public Settings([NotNull] StakeSettings stake, [NotNull] OtherAddressSettings address, [NotNull] ConnectionSettings connection)
		{
			Stake = stake ?? throw new ArgumentNullException(nameof(stake));
			Address = address ?? throw new ArgumentNullException(nameof(address));
			Connection = connection ?? throw new ArgumentNullException(nameof(connection));
		}

		/// <summary>
		///     Gets the <see cref="OtherAddressSettings" /> values.
		/// </summary>
		[NotNull]
		public OtherAddressSettings Address { get; }

		/// <summary>
		///     Gets the <see cref="ConnectionSettings" /> values.
		/// </summary>
		[NotNull]
		public ConnectionSettings Connection { get; }

		/// <summary>
		///     Gets the <see cref="StakeSettings" /> values.
		/// </summary>
		[NotNull]
		public StakeSettings Stake { get; }
	}
}