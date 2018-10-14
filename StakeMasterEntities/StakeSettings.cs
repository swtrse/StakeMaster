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
		/// <param name="editStakes">Enables or disables the processing of <see cref="DedicatedStakingAddress" /> inputs.</param>
		/// <param name="dedicatedStakingAddress">The public address where all inputs for staking are located.</param>
		/// <param name="dedicatedCollectingAddress">
		///     The public addess where all inputs are stored temporarily till they get
		///     processed.
		/// </param>
		/// <param name="stakingPatience">The number of days where an input should stake at least once.</param>
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
		public StakeSettings(bool editStakes, [NotNull] string dedicatedStakingAddress, [NotNull] string dedicatedCollectingAddress, int stakingPatience)
		{
			DedicatedStakingAddress = dedicatedStakingAddress ?? throw new ArgumentNullException(nameof(dedicatedStakingAddress));
			DedicatedCollectingAddress = dedicatedCollectingAddress ?? throw new ArgumentNullException(nameof(dedicatedCollectingAddress));
			StakingPatience = stakingPatience;
			EditStakes = editStakes;
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

		/// <summary>
		///     If
		///     <value>true</value>
		///     inputs of the <see cref="DedicatedStakingAddress" /> will be edited and new inputs will be created if there are
		///     enough coins.
		///     If
		///     <value>false</value>
		///     no changes will be made to Inputs of the <see cref="DedicatedStakingAddress" />.
		/// </summary>
		public bool EditStakes { get; }

		/// <summary>
		///     Gets the number of days during an input should at least stake one time.
		/// </summary>
		public int StakingPatience { get; }
	}
}
