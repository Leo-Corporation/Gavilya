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
using Gavilya.Enums;
using Gavilya.UserControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Gavilya.Pages;

/// <summary>
/// Logique d'interaction pour GamesCardsPages.xaml
/// </summary>
public partial class GamesCardsPages : Page
{
	public GamesCardsPages()
	{
		InitializeComponent();
		Definitions.GamesCardsPages = this; // Define the GamesCardsPages
	}

	public void LoadGames()
	{
		Dispatcher.Invoke(new Action(() =>
		{
			Definitions.HomePage.FavoriteBar.Children.Clear();
			GamePresenter.Children.Clear(); // Remove all the games

			var recommandedGames = Global.GetRecommandedGames();

			Definitions.HomePage.RecommandedPlaceholder.Visibility = recommandedGames.Count <= 0 ? Visibility.Visible : Visibility.Collapsed;


			if (Definitions.Games.Count > 0)
			{
				GamePresenter.Visibility = Visibility.Visible; // Visible
				WelcomeHost.Visibility = Visibility.Collapsed; // Hidden
				foreach (GameInfo gameInfo in Definitions.Games) // For each game
				{
					GamePresenter.Children.Add(new GameCard(gameInfo, GavilyaPages.Cards, false, recommandedGames.Contains(gameInfo))); // Add the game
				}
			}
			else
			{
				WelcomeAddGames welcomeAddGames = new(); // New WelcomeAddGames
				welcomeAddGames.VerticalAlignment = VerticalAlignment.Stretch; // Center
				welcomeAddGames.HorizontalAlignment = HorizontalAlignment.Stretch; // Center
				WelcomeHost.Children.Add(welcomeAddGames); // Add a welcome add games
				WelcomeHost.Visibility = Visibility.Visible; // Visible
				GamePresenter.Visibility = Visibility.Collapsed; // Hidden
			}
		}));
	}

	private async void GamePresenter_Drop(object sender, DragEventArgs e)
	{
		try
		{
			if (Definitions.Games.Count <= 0)
			{
				Global.RemoveWelcomeScreen(); // Remove the "Welcome" screen
			}

			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				string[] files = (string[])e.Data.GetData(DataFormats.FileDrop); // Get all the files droped
				List<string> executables = new(); // The execuables files

				for (int i = 0; i < files.Length; i++) // For each file
				{
					if (System.IO.Path.GetExtension(files[i]) == ".exe") // If the file is a .exe
					{
						executables.Add(files[i]); // Add the file to the executables
					}
				}

				for (int i = 0; i < executables.Count; i++) // For each executables (or games)
				{
					FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(executables[i]);
					int id = await Global.GetGameId(string.IsNullOrEmpty(fileVersionInfo.ProductName) ? System.IO.Path.GetFileNameWithoutExtension(executables[i]) : fileVersionInfo.ProductName);
					GameInfo gameInfo = new()
					{
						FileLocation = executables[i],
						IsFavorite = false,
						Name = string.IsNullOrEmpty(fileVersionInfo.ProductName) ? System.IO.Path.GetFileNameWithoutExtension(executables[i]) : fileVersionInfo.ProductName,
						LastTimePlayed = 0,
						TotalTimePlayed = 0,
						IconFileLocation = await Global.GetCoverImageAsync(string.IsNullOrEmpty(fileVersionInfo.ProductName) ? System.IO.Path.GetFileNameWithoutExtension(executables[i]) : fileVersionInfo.ProductName),
						RAWGID = id, // Set the id
						ProcessName = "", // Set the ProcessName: "" => Default
						Description = (id != -1) ? await Global.GetGameDescriptionAsync(id) : "", // Get the description
						Platforms = (id != -1) ? await Global.GetGamePlatformsAsync(id) : new List<SDK.RAWG.Platform> { Definitions.DefaultPlatform }, // Get platforms
						Stores = (id != -1) ? await Global.GetStoresAsync(id) : new(), // Get
						Version = fileVersionInfo.FileVersion // Get the version
					};
					Definitions.Games.Add(gameInfo); // Add the games to the List<GameInfo>
					Definitions.GamesCardsPages.GamePresenter.Children.Add(new GameCard(gameInfo, GavilyaPages.Cards)); // Add the games to the GamePresenter
					GameSaver.Save(Definitions.Games); // Save the added games
					Global.SortGames(); // Sort
					Definitions.GamesCardsPages.LoadGames(); // Reload the page
					Definitions.RecentGamesPage.LoadGames(); // Reload the page
					Definitions.GamesListPage.LoadGames(); // Reload the page
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Properties.Resources.MainWindowTitle, MessageBoxButton.OK, MessageBoxImage.Error); // Error
		}
	}
}
