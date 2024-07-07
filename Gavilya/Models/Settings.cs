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

using PeyrSharp.Env;

namespace Gavilya.Models;

public class Settings
{
	/// <summary>
	/// Indicates if it is the first run of Gavilya.
	/// </summary>
	public bool IsFirstRun { get; set; }

	/// <summary>
	/// Indicates if the <see cref="MainWindow"/> is maximized or not.
	/// </summary>
	public bool IsMaximized { get; set; }

	public Language Language { get; set; }

	/// <summary>
	/// True if Gavilya should make an autosave.
	/// </summary>
	public bool MakeAutoSave { get; set; }

	/// <summary>
	/// The day when Gavilya should make an auto save.
	/// </summary>
	public int AutoSaveDay { get; set; }

	/// <summary>
	/// The save file path.
	/// </summary>
	public string SavePath { get; set; }

	/// <summary>
	/// The maximum amount of recent games shown in the home page.
	/// </summary>
	public int MaxNumberRecentGamesShown { get; set; }

	/// <summary>
	/// The number of search results to display in the search box. (default = 3)
	/// </summary>
	public int NumberOfSearchResultsToDisplay { get; set; }

	/// <summary>
	/// The opacity of the FPS Counter (a value between 0 et 1)
	/// </summary>
	public double FpsCounterOpacity { get; set; }

	/// <summary>
	/// True if Gavilya should show a notification if updates are available.
	/// </summary>
	public bool UpdatesAvNotification { get; set; }

	/// <summary>
	/// The position of the sidebar.
	/// </summary>
	public Position SidebarPosition { get; set; }

	/// <summary>
	/// The default view to display in the "Library" page.
	/// </summary>
	public View DefaultView { get; set; }

	/// <summary>
	/// The default page to display when starting the app.
	/// </summary>
	public Page DefaultPage { get; set; }

	/// <summary>
	/// The theme of the app. Set null to use default.
	/// </summary>
	public string CurrentTheme { get; set; }

	/// <summary>
	/// True if Games that are hidden by default should be shown anyways.
	/// </summary>
	public bool ShowHiddenGames { get; set; }

	/// <summary>
	/// True if the search keyboard shortcut is enabled.
	/// </summary>
	public bool? EnableSearchShortcut { get; set; }

	/// <summary>
	/// True if games should be displayed using dates instead of "A long time ago".
	/// </summary>
	public bool? GroupGamesByDate { get; set; }

	public Settings()
	{
		IsFirstRun = true;
		Language = Language.Default;
		IsMaximized = false;
		MakeAutoSave = true;
		AutoSaveDay = 1;
		SavePath = $@"{FileSys.AppDataPath}\Léo Corporation\Gavilya\Backups";
		MaxNumberRecentGamesShown = 4;
		NumberOfSearchResultsToDisplay = 3;
		FpsCounterOpacity = 1;
		UpdatesAvNotification = true;
		SidebarPosition = Position.Left;
		DefaultPage = Page.Home;
		DefaultView = View.Card;
		CurrentTheme = "";
		ShowHiddenGames = false;
		EnableSearchShortcut = true;
		GroupGamesByDate = true;
	}
}

public enum Page
{
	Home,
	Library,
	Favorites,
	Recent,
	Profile
}

public enum View
{
	Card,
	Tag,
	List
}

public enum Position
{
	Left,
	Right
}

public enum Language
{
	Default,
	en_US,
	fr_FR,
	zh_CN
}
