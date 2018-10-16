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
	using BusinessLogic;
	using NLog;

	internal class Program
	{
		private static readonly ILogger Logger = LogManager.GetLogger(typeof(Program).FullName);
		private static void Main(string[] args)
		{
			try
			{
				InitializeLogging();
				//Settings settings = SettingsHelper.Read(args);
				Logger.Log(LogLevel.Trace, "Trace");
				Logger.Log(LogLevel.Debug, "Debug");
				Logger.Log(LogLevel.Info, "Info");
				Logger.Log(LogLevel.Warn, "Warning");
				Logger.Log(LogLevel.Error, "Error", new Exception("xxx"));
				Logger.Log(LogLevel.Fatal, "Critical");
			}
			catch (SettingsArgumentInvalidException e)
			{
				SettingsHelper.DisplayHelp(e.Message);
			}
		}

		private static void InitializeLogging()
		{
			LogManager.LoadConfiguration("NLog.config");
			LogManager.ReconfigExistingLoggers();
		}
	}
}
