// ******************************* Module Header *******************************
// Module Name: Program.cs
// Project:     StakeMaster
// Copyright (c) Michael Goldfinger.
// 
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// *****************************************************************************

namespace StakeMaster
{
	using System;
	using System.IO;
	using System.Reflection;
	using System.Threading.Tasks;
	using BusinessLogic;
	using Microsoft.Extensions.Configuration;
	using Serilog;

	internal class Program
	{
		public static IConfiguration Configuration { get; } = InitializeAppSettings();

		private static void Main(string[] args)
		{
			try
			{
				InitializeAppSettings();
				InitializeLogging();
				Settings settings = SettingsHelper.Read(args);
				Log.Verbose("Trace");
				Log.Debug("Debug");
				Log.Information("Info");
				Task.Delay(10000).Wait();
				Log.Warning("Warning");
				Log.Error(new Exception("xxx", new Exception("yyy", new Exception("zzz"))), "Error");
				Log.Fatal("Critical");
				Task.Delay(10000).Wait();
			}
			catch (SettingsArgumentInvalidException e)
			{
				SettingsHelper.DisplayHelp(e.Message);
			}
			finally
			{
				Log.CloseAndFlush();
			}
		}

		private static IConfiguration InitializeAppSettings()
		{
			IConfigurationBuilder builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", false, false);

			return builder.Build();
		}

		private static void InitializeLogging()
		{
			Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(Configuration).CreateLogger();
		}
	}
}
