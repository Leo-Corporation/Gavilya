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
using Gavilya.Classes;
using Gavilya.Pages;
using Gavilya.Windows;
using System.Windows;

namespace Gavilya
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			SettingsSaver.Load(); // Load the settings
			Global.ChangeLanguage(); // Change the language

			Definitions.GameInfoPage = new(); // Create the page
			Definitions.GameInfoPage2 = new(); // Create the page

			ProfileManager.LoadProfiles(); // Load profiles
			GameSaver.Load(); // Load the .gav file in the Definitions class

			Definitions.StatGameInfoControl = new(); // New control
			Definitions.Statistics = new(); // New page

			Definitions.HomePage = new();

			RecentGamesPage recentGamesPage = new(); // RecentGamesPage
			Definitions.RecentGamesPage = recentGamesPage; // Define the RecentGamesPage
			Definitions.RecentGamesPage.LoadGames(); // Load the games

			GamesListPage gamesListPage = new(); // GamesListPage
			Definitions.GamesListPage = gamesListPage; // Define the GamesListPage
			Definitions.GamesListPage.LoadGames(); // Load the games

			Definitions.LibraryPage = new();
			Definitions.ProfilePage = new();

			if (Definitions.Settings.DefaultGavilyaHomePage is null)
			{
				Definitions.Settings.DefaultGavilyaHomePage = Enums.GavilyaWindowPages.Home; // Set default value
				SettingsSaver.Save(); // Save changes
			}

			if (Definitions.Settings.IsFirstRun) // If it is the app first run
			{
				new FirstRun().Show(); // Show the first run experience
			}
			else
			{
				int? pageID = (e.Args.Length >= 2 && e.Args[0] == "/page") ? int.Parse(e.Args[1]) : null;

				new MainWindow(pageID == null ? null : (Enums.GavilyaWindowPages)pageID).Show(); // Show the regular main window
				Global.CreateJumpLists();
			}
		}
	}
}
