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
using LeoCorpLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Gavilya.UserControls
{
	/// <summary>
	/// Logique d'interaction pour FavoriteGameCard.xaml
	/// </summary>
	public partial class FavoriteGameCard : UserControl
	{
		public GameInfo GameInfo { get; set; }
		string GamePath = ""; // Location of the game

		UIElement parentElement;

		public FavoriteGameCard(GameInfo gameInfo, UIElement parent = null)
		{
			InitializeComponent();
			GameInfo = gameInfo;
			parentElement = parent; // Define the parent element

			InitUI(gameInfo); // Load the UI
		}

		/// <summary>
		/// Load the User Interface (UI).
		/// </summary>
		/// <param name="gameInfo"></param>
		private void InitUI(GameInfo gameInfo)
		{
			// Tooltip
			GameNameToolTip.Content = gameInfo.Name;
			ToolTipGamePlay.Content = Properties.Resources.PlayLowerCase + " " + Properties.Resources.PlayTo + gameInfo.Name;

			if (gameInfo.IconFileLocation != string.Empty) // If there is an image
			{
				GameIcon.ImageSource = new BitmapImage(new Uri(gameInfo.IconFileLocation)); // Put the icon of the game
				GamePath = gameInfo.FileLocation; // Set the location of the game
			}
			else // If the image is the app icon
			{
				if (!gameInfo.IsUWP && !gameInfo.IsSteam) // If the game isn't UWP
				{
					Icon icon = Icon.ExtractAssociatedIcon(gameInfo.FileLocation); // Grab the icon of the game
					GameIcon.ImageSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()); // Show the image
					GamePath = gameInfo.FileLocation; // Set the location of the game 
				}
			}

		}

		private void PlayBtn_Click(object sender, RoutedEventArgs e)
		{
			if (!GameInfo.IsUWP && !GameInfo.IsSteam)
			{
				if (File.Exists(GamePath)) // If the game location file exist
				{
					Process.Start(GamePath); // Start the game

					if (parentElement is GameCard)
					{
						GameCard gameCard = (GameCard)parentElement; // Create a game card
						gameCard.GameInfo.LastTimePlayed = Env.GetUnixTime(); // Get the current unix time
						new GameSaver().Save(Definitions.Games); // Save the changes

						Definitions.GameInfoPage.UpdateLastTimePlayed(GameInfo.LastTimePlayed); // Update informations
						Definitions.GameInfoPage2.UpdateLastTimePlayed(GameInfo.LastTimePlayed); // Update informations

						gameCard.Timer.Start(); // Start the timer
					}

					Definitions.RecentGamesPage.LoadGames(); // Reload the games
				}
			}
			else
			{
				Process.Start("explorer.exe", GamePath); // Start the game

				if (parentElement is GameCard)
				{
					GameCard gameCard = (GameCard)parentElement; // Create a game card
					gameCard.GameInfo.LastTimePlayed = Env.GetUnixTime(); // Get the current unix time
					new GameSaver().Save(Definitions.Games); // Save the changes

					Definitions.GameInfoPage.UpdateLastTimePlayed(GameInfo.LastTimePlayed); // Update informations
					Definitions.GameInfoPage2.UpdateLastTimePlayed(GameInfo.LastTimePlayed); // Update informations

					gameCard.Timer.Start(); // Start the timer
				}

				Definitions.RecentGamesPage.LoadGames(); // Reload the games
			}
		}
	}
}
