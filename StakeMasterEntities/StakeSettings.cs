// ******************************* Module Header *******************************
// Module Name: StakeSettings.cs
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
	///     Contains settings for the staking. This class cannot be inherited.
	/// </summary>
	[PublicAPI]
	public sealed class StakeSettings
	{
		/// <summary>
		///     Initialzes a new instance of the <see cref="StakeSettings" /> class.
		/// </summary>
		/// <param name="dedicatedStakingAddress">The public address where all inputs for staking are located.</param>
		/// <param name="dedicatedCollectingAddress">
		///     The public addess where all inputs are stored temporarily till they get
		///     processed.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///     <paramref name="dedicatedStakingAddress" /> is
		///     <value>null</value>
		///     .
		/// </exception>
		/// <exception cref="ArgumentNullException">
		///     <paramref name="dedicatedCollectingAddress" /> is
		///     <value>null</value>
		///     .
		/// </exception>
		public StakeSettings([NotNull] string dedicatedStakingAddress, [NotNull] string dedicatedCollectingAddress)
		{
			DedicatedStakingAddress = dedicatedStakingAddress ?? throw new ArgumentNullException(nameof(dedicatedStakingAddress));
			DedicatedCollectingAddress = dedicatedCollectingAddress ?? throw new ArgumentNullException(nameof(dedicatedCollectingAddress));
		}

		/// <summary>
		///     Gets the public address where inputs are stored temporarily during the maintanance process.
		/// </summary>
		[NotNull]
		public string DedicatedCollectingAddress { get; }

		/// <summary>
		///     Gets the public address where all inputs fore staking are located.
		/// </summary>
		[NotNull]
		public string DedicatedStakingAddress { get; }
	}
}
