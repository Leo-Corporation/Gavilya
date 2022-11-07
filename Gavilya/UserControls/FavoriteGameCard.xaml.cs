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
using PeyrSharp.Env;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Gavilya.UserControls;

/// <summary>
/// Logique d'interaction pour FavoriteGameCard.xaml
/// </summary>
public partial class FavoriteGameCard : UserControl
{
	public GameInfo GameInfo { get; set; }
	string GamePath = ""; // Location of the game

	readonly UIElement parentElement;

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

		try
		{
			if (!string.IsNullOrEmpty(gameInfo.IconFileLocation)) // If there is an image
			{
				var bitmap = new BitmapImage();
				var stream = File.OpenRead(gameInfo.IconFileLocation);

				bitmap.BeginInit();
				bitmap.CacheOption = BitmapCacheOption.OnLoad;
				bitmap.StreamSource = stream;
				bitmap.DecodePixelWidth = 170;
				bitmap.EndInit();
				stream.Close();
				stream.Dispose();
				bitmap.Freeze();
				GameIcon.ImageSource = bitmap; // Put the icon of the game
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
		catch
		{
			GameIcon.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Gavilya;component/Assets/PC.png")); // Show the default image
		}
	}

	private void PlayBtn_Click(object sender, RoutedEventArgs e)
	{
		if (GameInfo.IsSteam && !Global.CanLaunchSteamGame())
		{
			return; // If the user can't launch the game, stop
		}

		if (!GameInfo.IsUWP && !GameInfo.IsSteam)
		{
			if (File.Exists(GamePath)) // If the game location file exist
			{
				Process p = new();
				p.StartInfo.WorkingDirectory = Path.GetDirectoryName(GamePath);
				p.StartInfo.FileName = GamePath;
				p.Start();

				if (parentElement is GameCard gameCard)
				{
					gameCard.GameInfo.LastTimePlayed = Sys.UnixTime; // Get the current unix time
					GameSaver.Save(Definitions.Games); // Save the changes

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
													 // Create a game card

			if (parentElement is GameCard gameCard)
			{
				gameCard.GameInfo.LastTimePlayed = Sys.UnixTime; // Get the current unix time
				GameSaver.Save(Definitions.Games); // Save the changes

				Definitions.GameInfoPage.UpdateLastTimePlayed(GameInfo.LastTimePlayed); // Update informations
				Definitions.GameInfoPage2.UpdateLastTimePlayed(GameInfo.LastTimePlayed); // Update informations

				gameCard.Timer.Start(); // Start the timer
			}

			Definitions.RecentGamesPage.LoadGames(); // Reload the games
		}
	}
}
