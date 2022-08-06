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
using Gavilya.Pages;
using Gavilya.UserControls;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;

namespace Gavilya.Classes;

/// <summary>
/// Definitions.
/// </summary>
public static class Definitions
{
	/// <summary>
	/// The gradient for the "HomeButtonBackColor".
	/// </summary>
	public static SolidColorBrush HomeButtonBackColor => new(Color.FromRgb(102, 0, 255)); // Gradient to return

	/// <summary>
	/// The <see cref="UserControls.StatGameInfoControl"/> control.
	/// </summary>
	internal static StatGameInfoControl StatGameInfoControl { get; set; }

	/// <summary>
	/// The default platform of a game.
	/// </summary>
	public static SDK.RAWG.Platform DefaultPlatform => new() { id = 4, name = "PC", slug = "pc" };

	/// <summary>
	/// Version of the software (Gavilya).
	/// </summary>
	public static string Version => "2.8.0.2208";

	/// <summary>
	/// True if the menu is shown.
	/// </summary>
	public static bool IsProfileMenuVisible { get; set; }

	/// <summary>
	/// Contains the least used game(s).
	/// </summary>
	public static Dictionary<GameInfo, GameCard> LeastUsedGames { get; set; }

	public static string BetaVersion
	{
		get
		{
			FileInfo fileInfo = new(Directory.GetCurrentDirectory() + @"\Gavilya.exe");
			return $"vNext.{fileInfo.LastWriteTimeUtc.Year.ToString()[2..4]}{fileInfo.LastWriteTime:MM}-{fileInfo.LastWriteTime.Day}-{fileInfo.LastWriteTime.Hour}{fileInfo.LastWriteTime.Minute}";
		}
	}

	/// <summary>
	/// Define if the current build is a preversion of Gavilya, not made for production nor Pre-Release.
	/// </summary>
	public static bool IsBeta => false;


	/// <summary>
	/// Gavilya profiles
	/// </summary>
	public static List<Profile> Profiles { get; set; }

	/// <summary>
	/// The Main <see cref="System.Windows.Window"/> of the App.
	/// </summary>
	public static MainWindow MainWindow { get; set; }

	/// <summary>
	/// The <see cref="Pages.GamesCardsPages"/> of the <see cref="MainWindow"/>.
	/// </summary>
	public static GamesCardsPages GamesCardsPages { get; set; }

	/// <summary>
	/// The <see cref="Pages.GamesListPage"/> of the <see cref="MainWindow"/>.
	/// </summary>
	public static GamesListPage GamesListPage { get; set; }

	/// <summary>
	/// The <see cref="Pages.RecentGamesPage"/> of the <see cref="MainWindow"/>.
	/// </summary>
	public static RecentGamesPage RecentGamesPage { get; set; }

	/// <summary>
	/// The <see cref="Pages.Statistics"/> page.
	/// </summary>
	public static Statistics Statistics { get; set; }

	/// <summary>
	/// The <see cref="Pages.HomePage"/> page.
	/// </summary>
	public static HomePage HomePage { get; set; }

	/// <summary>
	/// The <see cref="Pages.LibraryPage"/> page.
	/// </summary>
	public static LibraryPage LibraryPage { get; set; }

	/// <summary>
	/// The <see cref="Pages.ProfilePage"/> page.
	/// </summary>
	public static ProfilePage ProfilePage { get; set; }

	/// <summary>
	/// The state of the checkboxes of all the <see cref="UserControls.GameCard"/>.
	/// </summary>
	public static bool IsGamesCardsPagesCheckBoxesVisible { get; set; }

	/// <summary>
	/// The games that are added to the <see cref="MainWindow"/>.
	/// </summary>
	public static List<GameInfo> Games = new();

	/// <summary>
	/// The link of the last version <see cref="string"/>.
	/// </summary>
	public static string LastVersionLink => "https://raw.githubusercontent.com/Leo-Corporation/LeoCorp-Docs/master/Liens/Update%20System/Gavilya/Version.txt";

	/// <summary>
	/// True if the menu is shown
	/// </summary>
	public static bool IsMenuShown { get; set; }

	/// <summary>
	/// The <see cref="Pages.GameInfoPage"/> of any games.
	/// </summary>
	public static GameInfoPage GameInfoPage { get; set; }

	/// <summary>
	/// The <see cref="Pages.GameInfoPage"/> of any games.
	/// </summary>
	public static GameInfoPage GameInfoPage2 { get; set; }

	/// <summary>
	/// The transparent color.
	/// </summary>
	public static SolidColorBrush TransparentColor => new() { Color = Colors.Transparent };

	/// <summary>
	/// The languages that are available in Gavilya.
	/// </summary>
	public static List<string> Languages => new() { "English (United States)", "Français (France)", "中文（简体）" };

	/// <summary>
	/// The languages codes based on the languages that are available in Gavilya.
	/// </summary>
	public static List<string> LanguagesCodes => new() { "en-US", "fr-FR", "zh-CN" };

	public static Settings Settings { get; set; }
}

/// <summary>
/// The actions when a <see cref="Windows.SearchGameCover"/> window is opened.
/// </summary>
public enum GameAssociationActions
{
	/// <summary>
	/// Search for a game.
	/// </summary>
	Search,

	/// <summary>
	/// Associate a game.
	/// </summary>
	Associate
}
