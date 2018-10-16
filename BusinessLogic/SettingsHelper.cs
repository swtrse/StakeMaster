// ******************************* Module Header *******************************
// Module Name: Class1.cs
// Project:     StakeMasterBusinessLogic
// Copyright (c) Michael Goldfinger.
// 
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// *****************************************************************************

namespace StakeMaster.BusinessLogic
{
	using System.Linq;

	/// <summary>
	/// Contains methods for initializing and check settings.
	/// </summary>
	public static class SettingsHelper
	{
		/// <summary>
		/// Returns the settings based on the arguments given.
		/// </summary>
		/// <param name="args">
		/// Arguments that are considered when generating the settings.
		/// </param>
		/// <returns>
		/// Returns a <see cref="Settings"/>-object with all properties set based on the given arguments.
		/// </returns>
		public static Settings Read(string[] args)
		{
			ReadHelpSettings(args);
			var stakeSettings = ReadStakeSettings(args);
			throw new System.NotImplementedException();
		}

		private static StakeSettings ReadStakeSettings(string[] args)
		{
			throw new System.NotImplementedException();
		}

		private static void ReadHelpSettings(string[] args)
		{
			if (args.Contains("-?") || args.Contains("--help"))
			{
				throw new SettingsArgumentInvalidException();
			}
		}

		public static void DisplayHelp(string message)
		{
			throw new System.NotImplementedException();
		}
	}
}
