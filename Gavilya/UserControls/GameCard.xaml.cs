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
using Gavilya.Windows;
using LeoCorpLibrary;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Gavilya.UserControls;

/// <summary>
/// Logique d'interaction pour GameCard.xaml
/// </summary>
public partial class GameCard : UserControl
{
	string location; // Location

	/// <summary>
	/// The infos of the game
	/// </summary>
	public GameInfo GameInfo { get; set; }

	/// <summary>
	/// The timer of the game.
	/// </summary>
	public DispatcherTimer Timer { get; set; } // Create a timer

	public GameCard(GameInfo gameInfo, GavilyaPages gavilyaPages, bool isFromEdit = false, bool recommanded = false)
	{
		InitializeComponent();

		GameInfo = gameInfo; // Define the info
		InitializeUI(gameInfo, gavilyaPages, isFromEdit, recommanded); // Load the UI

		Timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) }; // Define the timer
		Timer.Tick += Timer_Tick; // Set the event
	}

	/// <summary>
	/// Initialize the elements of the Interface.
	/// </summary>
	/// <param name="gameInfo"><see cref="Classes.GameInfo"/></param>
	/// <param name="isFromEdit"><see cref="true"/> if is called from the <see cref="EditGame"/> window.</param>
	internal void InitializeUI(GameInfo gameInfo, GavilyaPages gavilyaPages, bool isFromEdit = false, bool recommanded = false)
	{
		// Tooltip
		PlayToolTip.Content = Properties.Resources.PlayLowerCase + " " + Properties.Resources.PlayTo + gameInfo.Name;
		GameToolTipName.Content = gameInfo.Name;

		// Border thickness
		GameCardBorder.BorderThickness = new Thickness { Bottom = 0, Top = 0, Left = 0, Right = 0 }; // Set the border thickness

		// Location
		location = gameInfo.FileLocation;

		// Icon
		if (gameInfo.IconFileLocation != string.Empty && gameInfo.IconFileLocation != null) // If a custom image is used
		{
			var bitmap = new BitmapImage();
			var stream = File.OpenRead(gameInfo.IconFileLocation);

			bitmap.BeginInit();
			bitmap.CacheOption = BitmapCacheOption.OnLoad;
			bitmap.StreamSource = stream;
			bitmap.DecodePixelWidth = 256;
			bitmap.EndInit();
			stream.Close();
			stream.Dispose();
			bitmap.Freeze();
			GameIcon.ImageSource = bitmap;
		}
		else
		{
			if (!gameInfo.IsUWP && !gameInfo.IsSteam) // If the game isn't UWP
			{
				System.Drawing.Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(gameInfo.FileLocation);
				GameIcon.ImageSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()); // Show the image 
			}
		}

		// Favorite
		if (gameInfo.IsFavorite && !isFromEdit) // If the game is a favorite
		{
			FavoriteGameCard = new FavoriteGameCard(gameInfo, this);
			Definitions.HomePage.FavoriteBar.Children.Add(FavoriteGameCard); // Add the game to the favorite bar
			FavBtn.Content = ""; // Change icon
		}

		if (recommanded)
		{
			Definitions.HomePage.RecommandedBar.Children.Add(new FavoriteGameCard(gameInfo, this)); // Add the game to the recommanded bar
		}

		// Checkbox visibility
		if (Definitions.IsGamesCardsPagesCheckBoxesVisible) // If the checkboxes are visibles
		{
			CheckBox.Visibility = Visibility.Visible; // Visible
		}
		else
		{
			CheckBox.Visibility = Visibility.Hidden; // Hiddent
		}

		// Page
		switch (gavilyaPages)
		{
			case GavilyaPages.Recent: // If the page is recent
				FavBtn.Visibility = Visibility.Hidden; // Hide the favorite button
				CheckBox.Visibility = Visibility.Hidden; // Hide the checkbox
				break;
			case GavilyaPages.Cards: // If the page is card
				FavBtn.Visibility = Visibility.Visible; // Show the favorite button
				break;
		}
	}

	bool gameStarted = false;

	private void Button_Click(object sender, RoutedEventArgs e) // Play button
	{
		if (!GameInfo.IsUWP && !GameInfo.IsSteam)
		{
			if (File.Exists(location)) // If the file exist
			{
				Process.Start(location); // Start the game
				GameInfo.LastTimePlayed = Env.GetUnixTime(); // Set the last time played

				Timer.Start(); // Start the timer

				Definitions.RecentGamesPage.LoadGames(); // Reload the games
				GameSaver.Save(Definitions.Games); // Save the changes
			}
		}
		else
		{
			Process.Start("explorer.exe", location); // Start the game
			GameInfo.LastTimePlayed = Env.GetUnixTime(); // Set the last time played

			Timer.Start(); // Start the timer

			Definitions.RecentGamesPage.LoadGames(); // Reload the games
			GameSaver.Save(Definitions.Games); // Save the changes
		}
	}

	private void Timer_Tick(object sender, EventArgs e)
	{
		string processName = location;

		if (!GameInfo.IsUWP)
		{
			processName = (!string.IsNullOrEmpty(GameInfo.ProcessName)) ? GameInfo.ProcessName : System.IO.Path.GetFileNameWithoutExtension(GameInfo.FileLocation); // Get the process name
		}
		else
		{
			processName = (!string.IsNullOrEmpty(GameInfo.ProcessName)) ? GameInfo.ProcessName : location;
		}

		Definitions.GameInfoPage.DisplayTotalTimePlayed((Definitions.GameInfoPage.GameInfo == null) ? GameInfo.TotalTimePlayed : Definitions.GameInfoPage.GameInfo.TotalTimePlayed); // Refresh
		Definitions.GameInfoPage2.DisplayTotalTimePlayed((Definitions.GameInfoPage2.GameInfo == null) ? GameInfo.TotalTimePlayed : Definitions.GameInfoPage2.GameInfo.TotalTimePlayed); // Refresh

		if (Global.IsProcessRunning(processName)) // If the game is running
		{
			gameStarted = true; // The game has started
			GameInfo.TotalTimePlayed += 1; // Increment the time played
		}
		else
		{
			if (gameStarted) // If the game has been started
			{
				GameSaver.Save(Definitions.Games); // Save
				if (!GameInfo.AlwaysCheckIfRunning)
				{
					Timer.Stop(); // Stop 
				}
			}
		}
	}

	private void StackPanel_MouseEnter(object sender, MouseEventArgs e)
	{
		GameCardBorder.BorderThickness = new Thickness { Bottom = 3, Top = 3, Left = 3, Right = 3 }; // Set the border thickness
	}

	private void StackPanel_MouseLeave(object sender, MouseEventArgs e)
	{
		GameCardBorder.BorderThickness = new Thickness { Bottom = 0, Top = 0, Left = 0, Right = 0 }; // Set the border thickness
	}

	FavoriteGameCard FavoriteGameCard;

	private void FavBtn_Click(object sender, RoutedEventArgs e)
	{
		if (GameInfo.IsFavorite) // If the game is a favorite
		{
			GameInfo.IsFavorite = false; // The game is no longer a favorite
			Definitions.HomePage.FavoriteBar.Children.Remove(FavoriteGameCard); // Remove from favorite bar
			FavBtn.Content = ""; // Change icon
		}
		else
		{
			GameInfo.IsFavorite = true; // Set the game to be a favorite
			FavoriteGameCard = new FavoriteGameCard(GameInfo, this);
			Definitions.HomePage.FavoriteBar.Children.Add(FavoriteGameCard); // Add to favorite bar
			FavBtn.Content = ""; // Change icon
		}
		GameSaver.Save(Definitions.Games); // Save the changes
	}

	private void EditBtn_Click(object sender, RoutedEventArgs e)
	{
		new EditGame(this).Show(); // Show the EditGame window
	}

	private void GameCardBorder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		try
		{
			Definitions.GameInfoPage.InitializeUI(GameInfo, this);
			Definitions.MainWindow.PageContent.Content = Definitions.GameInfoPage;
		}
		catch (Exception)
		{

		}
	}
}
