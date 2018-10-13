// ******************************* Module Header *******************************
// Module Name: OtherAddressSettings.cs
// Project:     StakeMasterEntities
// Copyright (c) Michael Goldfinger.
// 
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// *****************************************************************************

namespace StakeMaster
{
	using JetBrains.Annotations;

	/// <summary>
	///     Contains settings for the processing of other addresses in the wallet. This class cannot be inherited.
	/// </summary>
	[PublicAPI]
	public sealed class OtherAddressSettings
	{
		/// <summary>
		///     Initialzes a new instance of the <see cref="OtherAddressSettings" /> class.
		/// </summary>
		/// <param name="collectInputs">
		///     Controls if wallet address inputs should be collected and moved to
		///     <see cref="StakeSettings.DedicatedCollectingAddress" />.
		/// </param>
		/// <param name="excludeAddresses">A list of addresses that are excluded from the move.</param>
		public OtherAddressSettings(bool collectInputs, [CanBeNull] string[] excludeAddresses)
		{
			CollectInputs = collectInputs;
			ExcludeAddresses = excludeAddresses ?? new string[0];
		}

		/// <summary>
		///     When
		///     <value>true</value>
		///     inputs will be moved to the <see cref="StakeSettings.DedicatedCollectingAddress" />.
		///     When
		///     <value>false</value>
		///     no input will be moved.
		/// </summary>
		public bool CollectInputs { get; }

		/// <summary>
		///     Contains a list of addresses that will not be moved even if <see cref="CollectInputs" /> is
		///     <value>true</value>
		///     .
		/// </summary>
		/// <remarks>
		///     <see cref="StakeSettings.DedicatedCollectingAddress" /> and <see cref="StakeSettings.DedicatedStakingAddress" />
		///     are automaicaly excluded from the move and must not be spezified explicitly.
		/// </remarks>
		[NotNull]
		public string[] ExcludeAddresses { get; }
	}
}
