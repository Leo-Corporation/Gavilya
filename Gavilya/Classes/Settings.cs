/*
MIT License

Copyright (c) Léo Corporation

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE. 
*/
using Gavilya.Enums;
using PeyrSharp.Env;
using System.IO;
using System.Xml.Serialization;

namespace Gavilya.Classes;

/// <summary>
/// Settings of Gavilya
/// </summary>
public class Settings
{
	/// <summary>
	/// The language of Gavilya.
	/// </summary>
	public string Language { get; set; }

	/// <summary>
	/// Indicates if it is the first run of Gavilya.
	/// </summary>
	public bool IsFirstRun { get; set; }

	/// <summary>
	/// Indicates if the <see cref="MainWindow"/> is maximized or not.
	/// </summary>
	public bool IsMaximized { get; set; }

	/// <summary>
	/// The page id to check on startup.
	/// </summary>
	public int PageId { get; set; }

	/// <summary>
	/// The current profile index in <see cref="Definitions.Profiles"/>.
	/// </summary>
	public int CurrentProfileIndex { get; set; }

	/// <summary>
	/// True if Gavilya should make an autosave.
	/// </summary>
	public bool? MakeAutoSave { get; set; }

	/// <summary>
	/// The day when Gavilya should make an auto save.
	/// </summary>
	public int? AutoSaveDay { get; set; }

	/// <summary>
	/// The save file path.
	/// </summary>
	public string SavePath { get; set; }

	/// <summary>
	/// The default startup home page of Gavilya.
	/// </summary>
	public GavilyaWindowPages? DefaultGavilyaHomePage { get; set; }

	/// <summary>
	/// The maximum amount of recent games shown in the home page.
	/// </summary>
	public int? MaxNumberRecentGamesShown { get; set; }

	/// <summary>
	/// True if Gavilya should display games that the user doesn't play often
	/// </summary>
	public bool? ShowMoreUnplayedGamesRecommanded { get; set; }

	/// <summary>
	/// True if Gavilya should hide the search bar. (default = false)
	/// </summary>
	public bool? HideSearchBar { get; set; }

	/// <summary>
	/// The number of search results to display in the search box. (default = 3)
	/// </summary>
	public int? NumberOfSearchResultsToDisplay { get; set; }

	/// <summary>
	/// The opacity of the FPS Counter (a value between 0 et 1)
	/// </summary>
	public double? FpsCounterOpacity { get; set; }

	/// <summary>
	/// True if Gavilya should show a notification if updates are available.
	/// </summary>
	public bool? UpdatesAvNotification { get; set; }

	/// <summary>
	/// True if Gavilya should show a reminder to the user when a game is unused.
	/// </summary>
	public bool? UnusedGameNotification { get; set; }
}

public static class SettingsSaver
{
	/// <summary>
	/// Loads the settings into <see cref="Definitions.Settings"/>.
	/// </summary>
	public static void Load()
	{
		string AppDataPath = FileSys.AppDataPath; // Get %APPDATA% folder
		if (File.Exists(AppDataPath + @"\Gavilya\Settings.gavsettings"))
		{
			XmlSerializer xmlSerializer = new(typeof(Settings)); // XML Serializer
			StreamReader streamReader = new(AppDataPath + @"\Gavilya\Settings.gavsettings"); // Where the file is going to be read

			Definitions.Settings = (Settings)xmlSerializer.Deserialize(streamReader); // Read

			if (!Definitions.Settings.MaxNumberRecentGamesShown.HasValue)
			{
				Definitions.Settings.MaxNumberRecentGamesShown = 4;
			}

			if (!Definitions.Settings.ShowMoreUnplayedGamesRecommanded.HasValue)
			{
				Definitions.Settings.ShowMoreUnplayedGamesRecommanded = true;
			}

			if (!Definitions.Settings.HideSearchBar.HasValue)
			{
				Definitions.Settings.HideSearchBar = false;
			}

			if (!Definitions.Settings.NumberOfSearchResultsToDisplay.HasValue)
			{
				Definitions.Settings.NumberOfSearchResultsToDisplay = 3;
			}

			if (!Definitions.Settings.FpsCounterOpacity.HasValue)
			{
				Definitions.Settings.FpsCounterOpacity = 1;
			}

			if (!Definitions.Settings.UpdatesAvNotification.HasValue)
			{
				Definitions.Settings.UpdatesAvNotification = true;
			}

			if (!Definitions.Settings.UnusedGameNotification.HasValue)
			{
				Definitions.Settings.UnusedGameNotification = true;
			}

			streamReader.Dispose();
		}
		else
		{
			Definitions.Settings = new Settings
			{
				Language = "_default",
				IsFirstRun = true,
				IsMaximized = false,
				PageId = 0,
				CurrentProfileIndex = 0,
				MakeAutoSave = true,
				AutoSaveDay = 1,
				SavePath = $@"{FileSys.AppDataPath}\Gavilya\Backups",
				DefaultGavilyaHomePage = GavilyaWindowPages.Home,
				MaxNumberRecentGamesShown = 4,
				ShowMoreUnplayedGamesRecommanded = true,
				HideSearchBar = false,
				NumberOfSearchResultsToDisplay = 3,
				FpsCounterOpacity = 1,
				UpdatesAvNotification = true,
				UnusedGameNotification = true
			};
			Save();
		}
	}

	/// <summary>
	/// Saves the settings into the <c>%APPDATA%</c> folder.
	/// </summary>
	public static void Save()
	{
		string AppDataPath = FileSys.AppDataPath; // Get %APPDATA% folder
		XmlSerializer xmlSerializer = new(Definitions.Settings.GetType()); // XML Serializer

		if (!Directory.Exists(AppDataPath + @"\Gavilya")) // If the directory doesn't exist
		{
			Directory.CreateDirectory(AppDataPath + @"\Gavilya"); // Create the directory
		}

		StreamWriter streamWriter = new(AppDataPath + @"\Gavilya\Settings.gavsettings"); // The place where the file is going to be written
		xmlSerializer.Serialize(streamWriter, Definitions.Settings);

		streamWriter.Dispose();
	}
}
