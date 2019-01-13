// ******************************* Module Header *******************************
// Module Name: SettingsHelper.cs
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
	using JetBrains.Annotations;
	using Properties;

	/// <summary>
	///     Contains methods for initializing and check settings.
	/// </summary>
	[PublicAPI]
	public static class SettingsHelper
	{
		/// <summary>
		///     Displays the help message for the command line options.
		/// </summary>
		/// <param name="message">
		///     An Additional message to show before the help message.
		/// </param>
		public static void DisplayHelp([CanBeNull] string message)
		{
			if (!string.IsNullOrEmpty(message))
			{
				Console.WriteLine(Resources.SettingsHelper_DisplayHelp_InvalidArgument_Text, message, Environment.NewLine);
			}

			Console.WriteLine(Resources.SettingsHelper_DisplayHelp_Header);
			Console.WriteLine(Resources.SettingsHelper_DisplayHelp_Text, Environment.NewLine);
		}

		private static bool ExtractArgumentBoolValue([NotNull] string[] args, string argumentString, string alternativeArgumentString, bool defaultValue)
		{
			try
			{
				string arg = args.SingleOrDefault(a => a.StartsWith(argumentString, StringComparison.Ordinal))?.Substring(argumentString.Length);
				string arg2 = args.SingleOrDefault(a => a.StartsWith(alternativeArgumentString, StringComparison.Ordinal))?.Substring(alternativeArgumentString.Length);
				if (arg != null && arg2 != null)
				{
					throw new SettingsArgumentInvalidException($"({argumentString}|{alternativeArgumentString})",
					                                           Resources.SettingsHelper_ExtractArgument_ArgumentNotUnique_Message);
				}

				if (arg == null && arg2 == null)
				{
					return defaultValue;
				}

				bool ret = arg != null ? bool.Parse(arg) : bool.Parse(arg2);
				return ret;
			}
			catch (Exception e)
			{
				throw new SettingsArgumentInvalidException($"({argumentString}|{alternativeArgumentString})", e);
			}
		}

		private static int ExtractArgumentIntValue([NotNull] string[] args, string argumentString, string alternativeArgumentString, int defaultValue)
		{
			try
			{
				string arg = args.SingleOrDefault(a => a.StartsWith(argumentString, StringComparison.Ordinal))?.Substring(argumentString.Length);
				string arg2 = args.SingleOrDefault(a => a.StartsWith(alternativeArgumentString, StringComparison.Ordinal))?.Substring(alternativeArgumentString.Length);
				if (arg != null && arg2 != null)
				{
					throw new SettingsArgumentInvalidException($"({argumentString}|{alternativeArgumentString})",
					                                           Resources.SettingsHelper_ExtractArgument_ArgumentNotUnique_Message);
				}

				if (arg == null && arg2 == null)
				{
					return defaultValue;
				}

				int ret = arg != null ? int.Parse(arg) : int.Parse(arg2);
				return ret;
			}
			catch (Exception e)
			{
				throw new SettingsArgumentInvalidException($"({argumentString}|{alternativeArgumentString})", e);
			}
		}

		[NotNull]
		private static string[] ExtractArgumentStringArrayValue([NotNull] string[] args, string argumentString, string alternativeArgumentString)
		{
			try
			{
				string arg = args.SingleOrDefault(a => a.StartsWith(argumentString, StringComparison.Ordinal))?.Substring(argumentString.Length);
				string arg2 = args.SingleOrDefault(a => a.StartsWith(alternativeArgumentString, StringComparison.Ordinal))?.Substring(alternativeArgumentString.Length);
				if (arg != null && arg2 != null)
				{
					throw new SettingsArgumentInvalidException($"({argumentString}|{alternativeArgumentString})",
					                                           Resources.SettingsHelper_ExtractArgument_ArgumentNotUnique_Message);
				}

				if (arg == null && arg2 == null)
				{
					return new string[0];
				}

				string[] ret = (arg ?? arg2).Split(',');
				return ret.Where(r => !string.IsNullOrEmpty(r)).ToArray();
			}
			catch (Exception e)
			{
				throw new SettingsArgumentInvalidException($"({argumentString}|{alternativeArgumentString})", e);
			}
		}

		[NotNull]
		private static string ExtractArgumentStringValue([NotNull] string[] args, string argumentString, string alternativeArgumentString)
		{
			try
			{
				string arg = args.SingleOrDefault(a => a.StartsWith(argumentString, StringComparison.Ordinal))?.Substring(argumentString.Length);
				string arg2 = args.SingleOrDefault(a => a.StartsWith(alternativeArgumentString, StringComparison.Ordinal))?.Substring(alternativeArgumentString.Length);
				if (arg != null && arg2 != null)
				{
					throw new SettingsArgumentInvalidException($"({argumentString}|{alternativeArgumentString})",
					                                           Resources.SettingsHelper_ExtractArgument_ArgumentNotUnique_Message);
				}

				if (arg == null && arg2 == null)
				{
					throw new SettingsArgumentInvalidException($"({argumentString}|{alternativeArgumentString})",
					                                           Resources.SettingsHelper_ExtractArgument_ArgumentNotDefined_Message);
				}

				string ret = arg ?? arg2;
				return ret;
			}
			catch (Exception e)
			{
				throw new SettingsArgumentInvalidException($"({argumentString}|{alternativeArgumentString})", e);
			}
		}

		[NotNull]
		private static Uri ExtractArgumentUriValue([NotNull] string[] args, string argumentString, string alternativeArgumentString)
		{
			try
			{
				string arg = args.SingleOrDefault(a => a.StartsWith(argumentString, StringComparison.Ordinal))?.Substring(argumentString.Length);
				string arg2 = args.SingleOrDefault(a => a.StartsWith(alternativeArgumentString, StringComparison.Ordinal))?.Substring(alternativeArgumentString.Length);
				if (arg != null && arg2 != null)
				{
					throw new SettingsArgumentInvalidException($"({argumentString}|{alternativeArgumentString})",
					                                           Resources.SettingsHelper_ExtractArgument_ArgumentNotUnique_Message);
				}

				if (arg == null && arg2 == null)
				{
					throw new SettingsArgumentInvalidException($"({argumentString}|{alternativeArgumentString})",
					                                           Resources.SettingsHelper_ExtractArgument_ArgumentNotDefined_Message);
				}

				var ret = new Uri(arg ?? arg2, UriKind.Absolute);
				return ret;
			}
			catch (Exception e)
			{
				throw new SettingsArgumentInvalidException($"({argumentString}|{alternativeArgumentString})", e);
			}
		}

		/// <summary>
		///     Returns the settings based on the arguments given.
		/// </summary>
		/// <param name="args">
		///     Arguments that are considered when generating the settings.
		/// </param>
		/// <returns>
		///     Returns a <see cref="Settings" />-object with all properties set based on the given arguments.
		/// </returns>
		/// <exception cref="SettingsArgumentInvalidException">Will be thrown when arguments cannot be parsed.</exception>
		[NotNull]
		public static Settings Read([CanBeNull] string[] args)
		{
			if (args == null)
			{
				args = new string[0];
			}

			ReadHelpSettings(args);
			StakeSettings stakeSettings = ReadStakeSettings(args);
			OtherAddressSettings otherAddressSetings = ReadOtherAddressSettings(args, stakeSettings.DedicatedStakingAddress, stakeSettings.DedicatedCollectingAddress);
			ConnectionSettings connectionSettings = ReadConnectionSettings(args);
			return new Settings(stakeSettings, otherAddressSetings, connectionSettings);
		}

		[NotNull]
		private static ConnectionSettings ReadConnectionSettings([NotNull] string[] args) => new ConnectionSettings(ExtractArgumentUriValue(args, "-o=", "--uri="),
		                                                                                                            ExtractArgumentStringValue(args, "-u=", "--user="),
		                                                                                                            ExtractArgumentStringValue(args, "-p=", "--password="));

		private static void ReadHelpSettings([NotNull] string[] args)
		{
			if (args.Contains("-?") || args.Contains("--help"))
			{
				throw new SettingsArgumentInvalidException(string.Empty, string.Empty);
			}
		}

		[NotNull]
		private static OtherAddressSettings ReadOtherAddressSettings([NotNull] string[] args, string dedicatedStakingAddress, string dedicatedCollectingAddress)
		{
			string[] excludes = ExtractArgumentStringArrayValue(args, "-e=", "--excludeaddress=");
			excludes = excludes.Concat(new[] {dedicatedStakingAddress, dedicatedCollectingAddress}).ToArray();
			bool merge = ExtractArgumentBoolValue(args, "-x=", "--mergeInputs=", false);
			bool collect = ExtractArgumentBoolValue(args, "-i=", "--collectinputs=", true);
			return new OtherAddressSettings(collect, excludes, merge);
		}

		[NotNull]
		private static StakeSettings ReadStakeSettings([NotNull] string[] args) => new StakeSettings(ExtractArgumentBoolValue(args, "-s=", "--stakes=", true),
		                                                                                             ExtractArgumentStringValue(args, "-a=", "--stakeaddress="),
		                                                                                             ExtractArgumentStringValue(args, "-c=", "--collectaddress="),
		                                                                                             ExtractArgumentIntValue(args, "-w=", "--patience=", 7),
		                                                                                             ExtractArgumentStringValue(args, "-q=", "--walletpassword="));
	}
}
