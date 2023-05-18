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

namespace Gavilya.Pages;

/// <summary>
/// Interaction logic for ProfilePage.xaml
/// </summary>
public partial class ProfilePage : Page
{
	Profile CurrentProfile { get; init; }
	int VisibleBadges { get; set; }

	public ProfilePage()
	{
		InitializeComponent();
		CurrentProfile = Definitions.Profiles[Definitions.Settings.CurrentProfileIndex]; // Current profile

		CheckedButton = SpotlightTabBtn;
		InitUI();
	}

	List<GameInfo> mostPlayed, favorites;
	internal void InitUI()
	{
		mostPlayed = new(); // Create new list
		favorites = new(); // Create new list

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
		TotalTimePlayedTxt.Text = $"{Global.GetTotalTimePlayed() / 3600}{Properties.Resources.HourShort} • {Definitions.Games.Count} {(Definitions.Games.Count > 1 ? Properties.Resources.GamesL : Properties.Resources.GameL)}"; // Set text

		// Get top 3 most played games
		// Values
		Dictionary<GameInfo, int> gameTimes = new(); // Create dictionnary

		for (int i = 0; i < Definitions.Games.Count; i++)
		{
			if (Definitions.Games[i].IsHidden ?? false && !Definitions.DisplayHiddenGames) continue;
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
		if (mostPlayed.Count >= 3)
		{
			int longestPlayed = mostPlayed[0].TotalTimePlayed;
			double h = mostPlayed[0].TotalTimePlayed * (GraphPanel.Height - 50) / longestPlayed;
			double h1 = mostPlayed[1].TotalTimePlayed * (GraphPanel.Height - 50) / longestPlayed;
			double h2 = mostPlayed[2].TotalTimePlayed * (GraphPanel.Height - 50) / longestPlayed;

			Top1Rect.Height = h; // Set height
			Top2Rect.Height = h1; // Set height
			Top3Rect.Height = h2; // Set height

			// Load tooltips
			Top1ToolTip.Content = $"{mostPlayed[0].Name} ({mostPlayed[0].TotalTimePlayed / 3600}{Properties.Resources.HourShort})"; // Set tooltip
			Top2ToolTip.Content = $"{mostPlayed[1].Name} ({mostPlayed[1].TotalTimePlayed / 3600}{Properties.Resources.HourShort})"; // Set tooltip
			Top3ToolTip.Content = $"{mostPlayed[2].Name} ({mostPlayed[2].TotalTimePlayed / 3600}{Properties.Resources.HourShort})"; // Set tooltip

			GameName1.Text = mostPlayed[0].Name; // Set text
			GameName2.Text = mostPlayed[1].Name; // Set text
			GameName3.Text = mostPlayed[2].Name; // Set text

			Game1TimeTxt.Text = $"{mostPlayed[0].TotalTimePlayed / 3600}{Properties.Resources.HourShort}";
			Game2TimeTxt.Text = $"{mostPlayed[1].TotalTimePlayed / 3600}{Properties.Resources.HourShort}";
			Game3TimeTxt.Text = $"{mostPlayed[2].TotalTimePlayed / 3600}{Properties.Resources.HourShort}";

			List<ImageBrush> imgs = new() { GameImg1, GameImg2, GameImg3 };

			for (int i = 0; i < mostPlayed.Count; i++)
			{
				try
				{
					if (mostPlayed[i].IconFileLocation != string.Empty && mostPlayed[i].IconFileLocation != null) // If a custom image is used
					{
						var bitmap = new BitmapImage();
						var stream = File.OpenRead(mostPlayed[i].IconFileLocation);

						bitmap.BeginInit();
						bitmap.CacheOption = BitmapCacheOption.OnLoad;
						bitmap.StreamSource = stream;
						bitmap.DecodePixelWidth = 256;
						bitmap.EndInit();
						stream.Close();
						stream.Dispose();
						bitmap.Freeze();
						imgs[i].ImageSource = bitmap;
					}
					else
					{
						if (!mostPlayed[i].IsUWP && !mostPlayed[i].IsSteam) // If the game isn't UWP
						{
							System.Drawing.Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(mostPlayed[i].FileLocation);
							imgs[i].ImageSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()); // Show the image 
						}
					}
				}
				catch
				{
					imgs[i].ImageSource = new BitmapImage(new Uri("pack://application:,,,/Gavilya;component/Assets/PC.png")); // Show the default image
				}
			}

			// Favorites tab
			FavoritesTab.Children.Clear();

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
		else
		{
			NothingToShow.Visibility = Visibility.Visible; // Hide
			SpotlightPage.Visibility = Visibility.Collapsed; // Hide
			FavoritesTab.Visibility = Visibility.Collapsed; // Hide
		}
		LoadBadges();
		LoadProfileUI();
	}

	internal void LoadProfileUI()
	{
		ProfileDisplayer.Children.Clear();

		ProfileDisplayer.Children.Add(new ProfileItem(Definitions.Profiles[Definitions.Settings.CurrentProfileIndex])); // Add profile 


		// Load the profile displayer
		if (Definitions.Profiles.Count > 1) // If there is more than one profile
		{
			for (int i = 0; i < Definitions.Profiles.Count; i++)
			{
				if (Definitions.Profiles[Definitions.Settings.CurrentProfileIndex] != Definitions.Profiles[i]) // If not the current profile
				{
					ProfileDisplayer.Children.Add(new ProfileItem(Definitions.Profiles[i])); // Add profile 
				}
			}
		}
		else
		{
			ProfileDisplayer.Children.Add(new NoProfileItem()); // Add a message
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
		SpotlightTabBtn.Background = new SolidColorBrush { Color = Color.FromRgb(40, 40, 60) }; // Change color 
		FavoriteTabBtn.Background = new SolidColorBrush { Color = Color.FromRgb(40, 40, 60) }; // Change color 
		BadgesTabBtn.Background = new SolidColorBrush { Color = Color.FromRgb(40, 40, 60) }; // Change color 

		CheckedButton.Background = Global.GetSolidColor("Accent"); // Change color
	}

	private void HideAll()
	{
		SpotlightPage.Visibility = Visibility.Collapsed; // Hide 
		FavoritesTab.Visibility = Visibility.Collapsed; // Hide 
		BadgesTab.Visibility = Visibility.Collapsed; // Hide 
		NothingToShow.Visibility = Visibility.Collapsed; // Hide
	}

	private void HideAllBadges()
	{
		VisibleBadges = 0;

		TheStartImg.Visibility = Visibility.Collapsed; // Hide
		CrazyAboutImg.Visibility = Visibility.Collapsed; // Hide
		NoobPlayerImg.Visibility = Visibility.Collapsed; // Hide
		StarterGamerImg.Visibility = Visibility.Collapsed; // Hide
		AdvancedGamerImg.Visibility = Visibility.Collapsed; // Hide
		TrueGamerImg.Visibility = Visibility.Collapsed; // Hide
		LegendaryImg.Visibility = Visibility.Collapsed; // Hide
		NeedSpaceOnTheShelvesImg.Visibility = Visibility.Collapsed; // Hide
	}

	private void LoadBadges()
	{
		HideAllBadges(); // Reset badges

		if (Definitions.Games.Count > 0)
		{
			TheStartImg.Visibility = Visibility.Visible; // Show
			VisibleBadges++; // Increment by 1
		}

		if (Global.GetFavoriteCount() >= 1)
		{
			CrazyAboutImg.Visibility = Visibility.Visible; // Show
			VisibleBadges++; // Increment by 1
		}

		if (Global.GetTotalTimePlayed() / 3600 >= 1)
		{
			NoobPlayerImg.Visibility = Visibility.Visible; // Show
			VisibleBadges++; // Increment by 1
		}

		NoobProgressTxt.Text = $"{Properties.Resources.Progress} {(Global.GetTotalTimePlayed() / 3600 > 1 ? 1 : Global.GetTotalTimePlayed() / 3600d) * 100d:0.00}%"; // Set text

		if (Global.GetTotalTimePlayed() / 3600 >= 10)
		{
			StarterGamerImg.Visibility = Visibility.Visible; // Show
			VisibleBadges++; // Increment by 1
		}

		StarterGarmerProgressTxt.Text = $"{Properties.Resources.Progress} {(Global.GetTotalTimePlayed() / 3600 > 10 ? 1 : Global.GetTotalTimePlayed() / 3600d / 10d) * 100d:0.00}%"; // Set text

		if (Global.GetTotalTimePlayed() / 3600 >= 100)
		{
			AdvancedGamerImg.Visibility = Visibility.Visible; // Show
			VisibleBadges++; // Increment by 1
		}

		AdvancedProgressTxt.Text = $"{Properties.Resources.Progress} {(Global.GetTotalTimePlayed() / 3600 > 100 ? 1 : Global.GetTotalTimePlayed() / 3600d / 100d) * 100d:0.00}%"; // Set text

		if (Global.GetTotalTimePlayed() / 3600 >= 1000)
		{
			TrueGamerImg.Visibility = Visibility.Visible; // Show
			VisibleBadges++; // Increment by 1
		}

		TrueGamerProgressTxt.Text = $"{Properties.Resources.Progress} {(Global.GetTotalTimePlayed() / 3600 > 1000 ? 1 : Global.GetTotalTimePlayed() / 3600d / 1000d) * 100d:0.00}%"; // Set text

		if (Global.GetTotalTimePlayed() / 3600 >= 5000)
		{
			LegendaryImg.Visibility = Visibility.Visible; // Show
			VisibleBadges++; // Increment by 1
		}

		LegendaryProgressTxt.Text = $"{Properties.Resources.Progress} {(Global.GetTotalTimePlayed() / 3600 > 5000 ? 1 : Global.GetTotalTimePlayed() / 3600d / 5000d) * 100d:0.00}%"; // Set text

		if (Definitions.Games.Count >= 50)
		{
			NeedSpaceOnTheShelvesImg.Visibility = Visibility.Visible; // Show
			VisibleBadges++; // Increment by 1
		}

		ShelvesProgressTxt.Text = $"{Properties.Resources.Progress} {(Definitions.Games.Count >= 50 ? 1 : Definitions.Games.Count / 50d) * 100d:0.00}%"; // Set text

		MyBadgesTxt.Visibility = (VisibleBadges == 0) ? Visibility.Collapsed : Visibility.Visible;
	}

	private void SpotlightTabBtn_Click(object sender, RoutedEventArgs e)
	{
		CheckedButton = SpotlightTabBtn; // Check
		HideAll();

		if (mostPlayed.Count >= 3)
		{
			SpotlightPage.Visibility = Visibility.Visible; // Show 
		}
		else
		{
			NothingToShow.Visibility = Visibility.Visible;
		}
		RefreshTabUI();
	}

	private void FavoriteTabBtn_Click(object sender, RoutedEventArgs e)
	{
		CheckedButton = FavoriteTabBtn; // Check
		HideAll();

		if (favorites.Count > 0)
		{
			FavoritesTab.Visibility = Visibility.Visible; // Show 
		}
		else
		{
			NothingToShow.Visibility = Visibility.Visible;
		}
		RefreshTabUI();
	}

	private void EditBtn_Click(object sender, RoutedEventArgs e)
	{
		new AddEditProfileWindow(EditMode.Edit, CurrentProfile).Show(); // Edit
	}

	private void AddProfileBtn_Click(object sender, RoutedEventArgs e)
	{
		new AddEditProfileWindow(EditMode.Add).Show(); // Add
	}

	private void BadgesTabBtn_Click(object sender, RoutedEventArgs e)
	{
		CheckedButton = BadgesTabBtn; // Check
		HideAll();

		BadgesTab.Visibility = Visibility.Visible;
		RefreshTabUI();
	}
}
