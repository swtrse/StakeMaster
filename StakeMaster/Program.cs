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
	using BusinessLogic;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Configuration;
	using Serilog;

	[UsedImplicitly]
	internal sealed class Program
	{
		private static IConfiguration Configuration { get; } = InitializeAppSettings();

		[NotNull]
		private static TransactionHelper GetConfiguration()
		{
			IConfigurationSection section = Configuration.GetSection("TransactionSettings");
			return new TransactionHelper(section.GetValue<int>("InputSize"),
			                             section.GetValue<int>("OutputSize"),
			                             section.GetValue<int>("Overhead"),
			                             section.GetValue<int>("FreeTransactionSizeLimit"),
			                             section.GetValue<DateTime>("BaseDateOfTransactions").Date,
			                             section.GetValue<int>("RpcTimeoutInSeconds"));
		}

		private static IConfiguration InitializeAppSettings()
		{
			IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", false, false);

			return builder.Build();
		}

		private static void InitializeLogging()
		{
			Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(Configuration).CreateLogger();
		}

		private static void Main(string[] args)
		{
			try
			{
				InitializeAppSettings();
				InitializeLogging();
				Log.Information("StakeMaster started");
				Log.Verbose("Command line arguments: {@args}", args);
				Log.Debug("Parse command line arguments...");
				Settings settings = SettingsHelper.Read(args);
				Log.Verbose("Settings: {@settings}", settings);
				TransactionHelper transactionHelper = GetConfiguration();
				Log.Verbose("TransactionHelper: {@transactionHelper}", transactionHelper);
				var wallet = new ProcessWallet(settings, transactionHelper);
				wallet.Run();

				Log.Information("StakeMaster finished.");
			}
			catch (SettingsArgumentInvalidException e)
			{
				Log.Verbose(e, "Error while reading the command line arguments.");
				SettingsHelper.DisplayHelp(e.Message);
			}
			catch (Exception e)
			{
				Log.Fatal(e, "An unexpected error occured. The application will be terminated.");
			}
			finally
			{
				Log.CloseAndFlush();
			}
		}
	}
}
