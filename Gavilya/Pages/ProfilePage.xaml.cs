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
using Gavilya.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Gavilya.Pages
{
	/// <summary>
	/// Interaction logic for ProfilePage.xaml
	/// </summary>
	public partial class ProfilePage : Page
	{
		Profile CurrentProfile { get; init; }
		public ProfilePage()
		{
			InitializeComponent();
			CurrentProfile = Definitions.Profiles[Definitions.Settings.CurrentProfileIndex]; // Current profile

			InitUI();
		}

		internal void InitUI()
		{
			if (CurrentProfile.PictureFilePath != "_default")
			{
				if (File.Exists(CurrentProfile.PictureFilePath))
				{
					var bitmap = new BitmapImage();
					var stream = File.OpenRead(CurrentProfile.PictureFilePath);

					bitmap.BeginInit();
					bitmap.DecodePixelWidth = 100;
					bitmap.CacheOption = BitmapCacheOption.OnLoad;
					bitmap.StreamSource = stream;
					bitmap.EndInit();
					stream.Close();
					stream.Dispose();
					bitmap.Freeze();

					ProfilePicture.ImageSource = bitmap; // Set image
				}
			}
			else
			{
				ProfilePicture.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Assets/DefaultPP.png")); // Set image
			}

			ProfileNameTxt.Text = CurrentProfile.Name; // Show name
			TotalTimePlayedTxt.Text = $"{Global.GetTotalTimePlayed() / 3600}{Properties.Resources.HourShort}"; // Set text

			// Get top 3 most played games
			// Values
			Dictionary<GameInfo, int> gameTimes = new(); // Create dictionnary
			List<GameInfo> mostPlayed = new(); // Create list

			for (int i = 0; i < Definitions.Games.Count; i++)
			{
				gameTimes.Add(Definitions.Games[i], Definitions.Games[i].TotalTimePlayed); // Add item
			}

			var items = from pair in gameTimes orderby pair.Value descending select pair; // Sort

			// Top 3 most played games
			int c = 0; // Counter
			foreach (KeyValuePair<GameInfo, int> keyValuePair in items)
			{
				if (c < 3)
				{
					mostPlayed.Add(keyValuePair.Key); // Add to the list
					c++; // Increment counter
				}
				else
				{
					break;
				}
			}

			// Get values
			int longestPlayed = mostPlayed[0].TotalTimePlayed;
			double h = mostPlayed[0].TotalTimePlayed * (GraphPanel.Height - 29.6) / longestPlayed;
			double h1 = mostPlayed[1].TotalTimePlayed * (GraphPanel.Height - 29.6) / longestPlayed;
			double h2 = mostPlayed[2].TotalTimePlayed * (GraphPanel.Height - 29.6) / longestPlayed;

			Top1Rect.Height = h; // Set height
			Top2Rect.Height = h1; // Set height
			Top3Rect.Height = h2; // Set height

			GameName1.Text = mostPlayed[0].Name; // Set text
			GameName2.Text = mostPlayed[1].Name; // Set text
			GameName3.Text = mostPlayed[2].Name; // Set text

			Game1TimeTxt.Text = $"{mostPlayed[0].TotalTimePlayed / 3600}{Properties.Resources.HourShort}";
			Game2TimeTxt.Text = $"{mostPlayed[1].TotalTimePlayed / 3600}{Properties.Resources.HourShort}";
			Game3TimeTxt.Text = $"{mostPlayed[2].TotalTimePlayed / 3600}{Properties.Resources.HourShort}";

			// Favorites tab
			FavoritesTab.Children.Clear();

			List<GameInfo> favorites = new();
			for (int i = 0; i < Definitions.Games.Count; i++)
			{
				if (Definitions.Games[i].IsFavorite)
				{
					favorites.Add(Definitions.Games[i]); // Add to favorites
				}
			}

			for (int i = 0; i < favorites.Count; i++)
			{
				FavoritesTab.Children.Add(new FavoriteListItem(favorites[i]));
			}
		}

		private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
		{
			ScrollViewer scv = (ScrollViewer)sender;
			scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta / 2);
			e.Handled = true;
		}

		internal Button CheckedButton { get; set; }
		internal void RefreshTabUI()
		{
			SpotlightTabBtn.BorderBrush = new SolidColorBrush { Color = Colors.Transparent }; // Change color 
			FavoriteTabBtn.BorderBrush = new SolidColorBrush { Color = Colors.Transparent }; // Change color 

			if (SpotlightPage.Visibility == Visibility.Visible)
			{
				SpotlightPage.Visibility = Visibility.Collapsed; // Hide
				FavoritesTab.Visibility = Visibility.Visible; // Show
			}
			else
			{
				SpotlightPage.Visibility = Visibility.Visible; // Show
				FavoritesTab.Visibility = Visibility.Collapsed; // Show
			}

			CheckedButton.BorderBrush = new SolidColorBrush { Color = Color.FromRgb(102, 0, 255) }; // Change color
		}

		private void SpotlightTabBtn_Click(object sender, RoutedEventArgs e)
		{
			CheckedButton = SpotlightTabBtn; // Check

			RefreshTabUI();
		}

		private void FavoriteTabBtn_Click(object sender, RoutedEventArgs e)
		{
			CheckedButton = FavoriteTabBtn; // Check

			RefreshTabUI();
		}

		private void SpotlightTabBtn_MouseEnter(object sender, MouseEventArgs e)
		{
			Button button = (Button)sender; // Create button

			button.BorderBrush = new SolidColorBrush { Color = Color.FromRgb(102, 0, 255) }; // Change color
		}

		private void SpotlightTabBtn_MouseLeave(object sender, MouseEventArgs e)
		{
			Button button = (Button)sender; // Create button

			if (CheckedButton != button)
			{
				button.BorderBrush = new SolidColorBrush { Color = Colors.Transparent }; // Change color 
			}
		}

		private void EditBtn_Click(object sender, RoutedEventArgs e)
		{
			new AddEditProfileWindow(EditMode.Edit, CurrentProfile).Show(); // Edit
		}
	}
}
