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
using Gavilya.UserControls;
using Gavilya.Windows;
using PeyrSharp.Env;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Gavilya.Pages;

/// <summary>
/// Logique d'interaction pour GameInfoPage.xaml
/// </summary>
public partial class GameInfoPage : Page
{
	int tabCheckedID = 0;
	internal GameInfo GameInfo { get; set; }
	UIElement parentUIElement = new();
	private DispatcherTimer timer; // Create a timer
	string gameLocation;

	public GameInfoPage(GameInfo gameInfo)
	{
		InitializeComponent();

		GameInfo = gameInfo;
		InitializeUI(gameInfo); // Initialize the UI
	}

	public GameInfoPage()
	{
		InitializeComponent();
		timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) }; // Define the timer
	}

	/// <summary>
	/// Initialize the User Interface of the page.
	/// </summary>
	/// <param name="gameInfo">The game to load informations.</param>
	/// <param name="parent">The parent element.</param>
	public async void InitializeUI(GameInfo gameInfo, UIElement parent = null)
	{
		try
		{
			// Var
			gameLocation = gameInfo.FileLocation;
			GameInfo = gameInfo; // Define the game info
			parentUIElement = parent; // Define the parent element
			timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) }; // Define the timer

			// Text
			PlayBtnToolTip.Content = Properties.Resources.PlayLowerCase + " " + Properties.Resources.PlayTo + gameInfo.Name; // Set the tooltip
			GameNameTxt.Text = gameInfo.Name; // Set the name of the game
			FavBtn.Content = gameInfo.IsFavorite ? "\uF71B" : "\uF710"; // Set text


			if (gameInfo.TotalTimePlayed != 0) // If the game was played
			{
				DisplayTotalTimePlayed(gameInfo.TotalTimePlayed); // Set the text
			}
			else
			{
				TotalTimePlayedTxt.Text = Properties.Resources.Never; // Set the text
			}

			if (gameInfo.LastTimePlayed != 0) // If the game was played
			{
				DateTime LastTimePlayed = Global.UnixTimeToDateTime(gameInfo.LastTimePlayed); // Get the date time
				LastTimePlayedTxt.Text = $"{LastTimePlayed.Day} {Global.NumberToMonth(LastTimePlayed.Month)} {LastTimePlayed.Year}"; // Last time played
			}
			else
			{
				LastTimePlayedTxt.Text = Properties.Resources.Never; // Set the text
			}

			DescriptionTxt.Text = gameInfo.Description;

			// Icon
			try
			{
				if (!string.IsNullOrEmpty(gameInfo.IconFileLocation)) // If a custom image is used
				{
					var bitmap = new BitmapImage();
					var stream = File.OpenRead(gameInfo.IconFileLocation);

					bitmap.BeginInit();
					bitmap.CacheOption = BitmapCacheOption.OnLoad;
					bitmap.StreamSource = stream;
					bitmap.EndInit();
					stream.Close();
					stream.Dispose();
					bitmap.Freeze();
					BackgroundImage.ImageSource = bitmap;
				}
				else
				{
					if (!gameInfo.IsUWP && !gameInfo.IsSteam) // If the game isn't UWP
					{
						System.Drawing.Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(gameInfo.FileLocation);
						BackgroundImage.ImageSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()); // Show the image 
					}
				}
			}
			catch
			{
				BackgroundImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Gavilya;component/Assets/PC.png")); // Show the default image
			}

			// Platforms
			PlatformDisplayer.Children.Clear();

			foreach (SDK.RAWG.Platform platform in gameInfo.Platforms)
			{
				PlatformDisplayer.Children.Add(new PlatformItem(platform)); // New textblock
			}

			// Stores
			StoreDisplayer.Children.Clear(); // Clear
			StoreDisplayer.Children.Add(
				new TextBlock
				{
					Foreground = new SolidColorBrush { Color = Colors.White }, // Set the foreground to {DynamicResource Foreground}
					Margin = new Thickness { Left = 1, Bottom = 1, Right = 1, Top = 1 }, // Set the the margin
					FontSize = 20, // Set the font size
					FontWeight = FontWeights.Bold, // Set the font weight
					Text = Properties.Resources.AvailableOn // Set the text
				}
			); // Add the textblock

			if (gameInfo.Stores.Count == 0)
			{
				gameInfo.Stores = await Global.GetStoresAsync(gameInfo.RAWGID);
				GameSaver.Save(Global.Games); // Save
			}

			for (int i = 0; i < gameInfo.Stores.Count; i++)
			{
				StoreDisplayer.Children.Add(new StoreItem(gameInfo.Stores[i])); // Add
			}

			// Ratings
			LoadRatings();

			// Achievments
			LoadAchievements();

			// Tags
			TagsDisplayer.Children.Clear();
			for (int i = 0; i < gameInfo.AssociatedTags.Count; i++)
			{
				TagsDisplayer.Children.Add(new TagItem(gameInfo.AssociatedTags[i]));
			}
			TagsTxt.Visibility = (TagsDisplayer.Children.Count > 0) ? Visibility.Visible : Visibility.Collapsed;
		}
		catch (Exception)
		{

		}
	}

	bool gameStarted = false;

	internal void PlayBtn_Click(object sender, RoutedEventArgs e)
	{
		if (GameInfo.IsSteam && !Global.CanLaunchSteamGame())
		{
			return; // If the user can't launch the game, stop
		}

		if (!GameInfo.IsUWP && !GameInfo.IsSteam)
		{
			if (File.Exists(gameLocation)) // If the file exist
			{
				Process p = new();
				p.StartInfo.WorkingDirectory = Path.GetDirectoryName(gameLocation);
				p.StartInfo.FileName = gameLocation;
				p.Start();

				if (parentUIElement is GameCard gameCard) // If the parent element is a game card
				{
					gameCard.GameInfo.LastTimePlayed = Sys.UnixTime; // Set the last time played
					GameSaver.Save(Global.Games); // Save the changes

					gameCard.Timer.Start(); // Start the timer

					DateTime LastTimePlayed = Global.UnixTimeToDateTime(GameInfo.LastTimePlayed); // Get the date time
					LastTimePlayedTxt.Text = $"{LastTimePlayed.Day} {Global.NumberToMonth(LastTimePlayed.Month)} {LastTimePlayed.Year}"; // Last time played
				}
				else if (parentUIElement is GameItem gameItem) // Create a game item
				{
					gameItem.GameInfo.LastTimePlayed = Sys.UnixTime; // Set the last time played
					Global.Games[Global.Games.IndexOf(gameItem.GameInfo)].LastTimePlayed = gameItem.GameInfo.LastTimePlayed; // Update the games
					GameSaver.Save(Global.Games); // Save the changes

					gameItem.Timer.Start();

					DateTime LastTimePlayed = Global.UnixTimeToDateTime(GameInfo.LastTimePlayed); // Get the date time
					LastTimePlayedTxt.Text = $"{LastTimePlayed.Day} {Global.NumberToMonth(LastTimePlayed.Month)} {LastTimePlayed.Year}"; // Last time played
				}

				Global.RecentGamesPage.LoadGames(); // Reload the games
			}
		}
		else // If is UWP
		{
			Process.Start("explorer.exe", gameLocation); // Start the game
														 // Create a game card

			if (parentUIElement is GameCard gameCard) // If the parent element is a game card
			{
				gameCard.GameInfo.LastTimePlayed = Sys.UnixTime; // Set the last time played
				GameSaver.Save(Global.Games); // Save the changes

				gameCard.Timer.Start(); // Start the timer

				DateTime LastTimePlayed = Global.UnixTimeToDateTime(GameInfo.LastTimePlayed); // Get the date time
				LastTimePlayedTxt.Text = $"{LastTimePlayed.Day} {Global.NumberToMonth(LastTimePlayed.Month)} {LastTimePlayed.Year}"; // Last time played
			}
			else if (parentUIElement is GameItem gameItem) // Create a game item
			{
				gameItem.GameInfo.LastTimePlayed = Sys.UnixTime; // Set the last time played
				Global.Games[Global.Games.IndexOf(gameItem.GameInfo)].LastTimePlayed = gameItem.GameInfo.LastTimePlayed; // Update the games
				GameSaver.Save(Global.Games); // Save the changes

				gameItem.Timer.Start();

				DateTime LastTimePlayed = Global.UnixTimeToDateTime(GameInfo.LastTimePlayed); // Get the date time
				LastTimePlayedTxt.Text = $"{LastTimePlayed.Day} {Global.NumberToMonth(LastTimePlayed.Month)} {LastTimePlayed.Year}"; // Last time played
			}

			Global.RecentGamesPage.LoadGames(); // Reload the games
		}
	}

	private void Timer_Tick(object sender, EventArgs e)
	{
		string processName = (!string.IsNullOrEmpty(GameInfo.ProcessName)) ? GameInfo.ProcessName : Path.GetFileNameWithoutExtension(GameInfo.FileLocation); // Get the process name

		if (Global.IsProcessRunning(processName)) // If the game is running
		{
			gameStarted = true; // The game has started
			GameInfo.TotalTimePlayed += 1; // Increment the time played
			MessageBox.Show(GameInfo.TotalTimePlayed.ToString());
		}
		else
		{
			if (gameStarted) // If the game has been started
			{
				GameSaver.Save(Global.Games); // Save
				DisplayTotalTimePlayed(GameInfo.TotalTimePlayed); // Update the text
			}
		}
	}

	internal void DisplayTotalTimePlayed(int timePlayed)
	{
		GameTimePlayed gameTimePlayed = GameTimePlayed.GetTimePlayed(timePlayed); // Create a GameTimePlayed
		string finalString = ""; // The final message

		string hoursPlurial = (gameTimePlayed.Hours > 1) ? "s" : ""; // Determine if a plurial is necessary
		string minutesPlurial = (gameTimePlayed.Minutes > 1) ? "s" : ""; // Determine if a plurial is necessary
		string secondsPlurial = (gameTimePlayed.Seconds > 1) ? "s" : ""; // Determine if a plurial is necessary

		if (gameTimePlayed.Hours != 0) // If the game was played more than an hour
		{
			finalString += $"{gameTimePlayed.Hours} {Properties.Resources.HourMin + hoursPlurial} "; // Add the hours
		}

		finalString += $"{gameTimePlayed.Minutes} {Properties.Resources.MinuteMin + minutesPlurial} "; // Add the minutes

		if (gameTimePlayed.Seconds != 0)
		{
			finalString += $"{gameTimePlayed.Seconds} {Properties.Resources.SecondMin + secondsPlurial}"; // Add the hours
		}

		TotalTimePlayedTxt.Text = finalString; // Set the text
	}

	internal void UpdateLastTimePlayed(int unixTime)
	{
		DateTime LastTimePlayed = Global.UnixTimeToDateTime(unixTime); // Get the date time
		LastTimePlayedTxt.Text = $"{LastTimePlayed.Day} {Global.NumberToMonth(LastTimePlayed.Month)} {LastTimePlayed.Year}"; // Last time played
	}

	private GameProperties GameProperties => new(GameInfo);
	private void PropertiesBtn_Click(object sender, RoutedEventArgs e)
	{
		GameProperties.Show();
	}

	private void FavBtn_Click(object sender, RoutedEventArgs e)
	{
		GameInfo.IsFavorite = !GameInfo.IsFavorite;
		Global.Games[Global.Games.IndexOf(GameInfo)] = GameInfo;

		FavBtn.Content = GameInfo.IsFavorite ? "\uF71B" : "\uF710"; // Set text

		GameSaver.Save(Global.Games); // Save changes
		Global.GamesCardsPages.LoadGames();
	}


	private void AboutTabBtn_Click(object sender, RoutedEventArgs e)
	{
		tabCheckedID = 0; // ID

		AboutTabBtn.Background = Global.GetSolidColor("Accent"); // Change color

		RatingsTabBtn.Background = Global.GetSolidColor("SelectedBackground"); // Change color
		AchievementsTabBtn.Background = Global.GetSolidColor("SelectedBackground"); // Change color

		AboutPage.Visibility = Visibility.Visible; // Change visibility
		RatingsPage.Visibility = Visibility.Collapsed; // Change visibility
		AchievementsPage.Visibility = Visibility.Collapsed; // Change visibility
	}

	private void RatingsTabBtn_Click(object sender, RoutedEventArgs e)
	{
		tabCheckedID = 1; // ID

		RatingsTabBtn.Background = Global.GetSolidColor("Accent"); // Change color

		AboutTabBtn.Background = Global.GetSolidColor("SelectedBackground"); // Change color
		AchievementsTabBtn.Background = Global.GetSolidColor("SelectedBackground"); // Change color

		AboutPage.Visibility = Visibility.Collapsed; // Change visibility
		RatingsPage.Visibility = Visibility.Visible; // Change visibility 
		AchievementsPage.Visibility = Visibility.Collapsed; // Change visibility
	}

	/// <summary>
	/// Loads the ratings page.
	/// </summary>
	private async void LoadRatings()
	{
		// Reset
		Pgr1.Value = 0;
		Pgr2.Value = 0;
		Pgr3.Value = 0;
		Pgr4.Value = 0;

		if (GameInfo.RAWGID != -1 && GameInfo.RAWGID != 0) // Check if the game is connected to RAWG.io
		{
			RatingsItem.Visibility = Visibility.Visible; // Show
			NoRatings.Visibility = Visibility.Collapsed; // Hide

			List<SDK.RAWG.Rating> ratings = await Global.GetGameRatingsAsync(GameInfo.RAWGID); // Get ratings

			if (ratings.Count > 0) // If there is ratings
			{
				float r = await Global.GetGameRatingAsync(GameInfo.RAWGID); // Get the average rating
				RatingTxt.Text = r.ToString(); // Set text

				int totalRatings = 0; // Total ratings
				for (int i = 0; i < ratings.Count; i++) // For each ratings
				{
					totalRatings += ratings[i].count; // Add the count of ratings
				}

				if (totalRatings == 0 || r == 0)
				{
					RatingsItem.Visibility = Visibility.Collapsed; // Hide
					NoRatings.Visibility = Visibility.Visible; // Show
					return;
				}

				for (int i = 0; i < ratings.Count; i++) // For each "rating"
				{
					switch (ratings[i].id) // Depending of the ID
					{
						case 5: Pgr4.Value = ratings[i].percent; Pgr4ToolTip.Content = $"{ratings[i].count}/{totalRatings} ({ratings[i].percent}%)"; break; // 4*
						case 4: Pgr3.Value = ratings[i].percent; Pgr3ToolTip.Content = $"{ratings[i].count}/{totalRatings} ({ratings[i].percent}%)"; break; // 3*
						case 3: Pgr2.Value = ratings[i].percent; Pgr2ToolTip.Content = $"{ratings[i].count}/{totalRatings} ({ratings[i].percent}%)"; break; // 2*
						case 1: Pgr1.Value = ratings[i].percent; Pgr1ToolTip.Content = $"{ratings[i].count}/{totalRatings} ({ratings[i].percent}%)"; break; // 1*
					}
				}

				
			}
			else
			{
				RatingsItem.Visibility = Visibility.Collapsed; // Hide
				NoRatings.Visibility = Visibility.Visible; // Show
			}
		}
		else
		{
			RatingsItem.Visibility = Visibility.Collapsed; // Hide
			NoRatings.Visibility = Visibility.Visible; // Show
		}
	}

	/// <summary>
	/// Loads the achievements page.
	/// </summary>
	private async void LoadAchievements()
	{
		AchievementsDisplayer.Children.Clear(); // Clear

		List<SDK.RAWG.Achievement> achievements = await Global.GetAchievementsAsync(GameInfo.RAWGID); // Get achievements

		if (achievements is not null && achievements.Count > 0)
		{
			foreach (SDK.RAWG.Achievement achievement in achievements)
			{
				AchievementsDisplayer.Children.Add(new AchievementItem(achievement)); // Add new achievement
			}
		}
		else
		{
			AchievementsDisplayer.Children.Add(new NoAchievementsItem()); // Add a message
		}
	}

	private async void GoToRawg_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			Process.Start("explorer.exe", await Global.GetRAWGUrl(GameInfo.RAWGID)); // Open RAWG.io
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AchievementsTabBtn_Click(object sender, RoutedEventArgs e)
	{
		tabCheckedID = 2; // ID

		AchievementsTabBtn.Background = Global.GetSolidColor("Accent"); // Change color

		RatingsTabBtn.Background = Global.GetSolidColor("SelectedBackground"); // Change color
		AboutTabBtn.Background = Global.GetSolidColor("SelectedBackground"); // Change color

		AboutPage.Visibility = Visibility.Collapsed; // Change visibility
		RatingsPage.Visibility = Visibility.Collapsed; // Change visibility
		AchievementsPage.Visibility = Visibility.Visible; // Change visibility
	}

	private async void GoToRawgAchivements_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			Process.Start("explorer.exe", await Global.GetRAWGUrl(GameInfo.RAWGID) + "/achievements"); // Open RAWG.io
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AdminBtn_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			if (!GameInfo.IsUWP && !GameInfo.IsSteam)
			{
				if (File.Exists(gameLocation)) // If the file exist
				{
					Sys.ExecuteAsAdmin(gameLocation); // Start the game
													  // Create a game card

					if (parentUIElement is GameCard gameCard) // If the parent element is a game card
					{
						gameCard.GameInfo.LastTimePlayed = Sys.UnixTime; // Set the last time played
						GameSaver.Save(Global.Games); // Save the changes

						gameCard.Timer.Start(); // Start the timer

						DateTime LastTimePlayed = Global.UnixTimeToDateTime(GameInfo.LastTimePlayed); // Get the date time
						LastTimePlayedTxt.Text = $"{LastTimePlayed.Day} {Global.NumberToMonth(LastTimePlayed.Month)} {LastTimePlayed.Year}"; // Last time played
					}
					else if (parentUIElement is GameItem gameItem) // Create a game item
					{
						gameItem.GameInfo.LastTimePlayed = Sys.UnixTime; // Set the last time played
						Global.Games[Global.Games.IndexOf(gameItem.GameInfo)].LastTimePlayed = gameItem.GameInfo.LastTimePlayed; // Update the games
						GameSaver.Save(Global.Games); // Save the changes

						gameItem.Timer.Start();

						DateTime LastTimePlayed = Global.UnixTimeToDateTime(GameInfo.LastTimePlayed); // Get the date time
						LastTimePlayedTxt.Text = $"{LastTimePlayed.Day} {Global.NumberToMonth(LastTimePlayed.Month)} {LastTimePlayed.Year}"; // Last time played
					}

					Global.RecentGamesPage.LoadGames(); // Reload the games
				}
			}
			else
			{
				MessageBox.Show(Properties.Resources.CannotLaunchAsAdminUWP, Properties.Resources.MainWindowTitle, MessageBoxButton.OK, MessageBoxImage.Information);
			}
		}
		catch { } // If the user says "No" to the admin prompt
	}
}
