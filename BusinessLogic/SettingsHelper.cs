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
	using System;
	using System.Linq;
	using System.Runtime.InteropServices;
	using JetBrains.Annotations;
	using Properties;

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
		[NotNull]
		public static Settings Read(string[] args)
		{
			ReadHelpSettings(args);
			StakeSettings stakeSettings = ReadStakeSettings(args);
			OtherAddressSettings otherAddressSetings = ReadOtherAddressSettings(args);
			ConnectionSettings connectionSettings = ReadConnectionSettings(args);
			return new Settings(stakeSettings, otherAddressSetings, connectionSettings);
		}

		private static ConnectionSettings ReadConnectionSettings(string[] args)
		{
			throw new NotImplementedException();
		}

		private static OtherAddressSettings ReadOtherAddressSettings(string[] args)
		{
			throw new NotImplementedException();
		}

		private static StakeSettings ReadStakeSettings(string[] args)
		{
			throw new NotImplementedException();
		}

		private static void ReadHelpSettings(string[] args)
		{
			if (args.Contains("-?") || args.Contains("--help"))
			{
				throw new SettingsArgumentInvalidException(string.Empty, string.Empty);
			}
		}

		public static void DisplayHelp(string message)
		{
			if (!string.IsNullOrEmpty(message))
			{
				Console.WriteLine(Resources.SettingsHelper_DisplayHelp_InvalidArgument_Text, message, Environment.NewLine);
			}
			Console.WriteLine(Resources.SettingsHelper_DisplayHelp_Header);
			Console.WriteLine(Resources.SettingsHelper_DisplayHelp_Text, Environment.NewLine);
			//Console.WriteLine("General settings{0}-? or --help: Displays this help{0}{0}Settings regarding the stake function{0}-s=<true/false> or --stakes=<true/false>:   Enables or disables modifications of inputs at the stake address. Default: true{0}-a=<address> or --stakeaddress=<addres>:    Sets the dedicated stake address. Mandatory.{0}-c=<address> or --collectaddress=<address>: Sets the dedocated collect address. Mandatory.{0}{0}Settings regarding all other addresses in the wallet{0}-i=<true/false> or --collectinputs=<true/false>                : Moves all inputs from other addresses to the collect address. Default: true{0}-e=<adr_1>{{,<adr_n>}} or --excludeaddress==<adr_1>{{,<adr_n>}}: Comma seperated list of addresses that will be excluded.{0}Settings regarding the rpc connection to the wallet{0}-u=<user> or --user=<user>:             The user for the rpc connection. Mandatory.{0}-p=<password> or --password=<password>: The password for the rpc connection. Mandatory.{0}-o or --uri=<uri>:                      The uri for the rpc connection. Mandatory.{0}", Environment.NewLine);
		}
	}
}
