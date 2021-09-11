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
using System;
using System.Collections.Generic;
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
	/// Logique d'interaction pour GameResult.xaml
	/// </summary>
	public partial class GameResult : UserControl
	{
		public GameResult(string gameName, int id)
		{
			InitializeComponent();
			GameName.Text = gameName; // Put the name of the game
			GameNameToolTip.Content = gameName; // Set the name of the game in the tool tip
			Id = id; // Define the Game Id

			Screen1Border.BorderBrush = new SolidColorBrush { Color = Color.FromRgb(102, 0, 255) }; // Set border brush
			CheckedBorder = Screen1Border; // Set value

			ScreenshotsViewer.Visibility = Visibility.Collapsed;
		}

		private async void InitScreenshotsUI()
		{
			// Load screenshots
			List<string> screenshots = await Global.GetCoverImageURLsAsync(Id);

			var image = new BitmapImage();
			image.BeginInit(); // Init image
			image.UriSource = new(screenshots[0]); // Get image
			image.EndInit(); // Stop init image

			Screen1Img.ImageSource = image; // Show image

			if (screenshots.Count > 1)
			{
				var image1 = new BitmapImage();
				image1.BeginInit(); // Init image
				image1.UriSource = new(screenshots[1]); // Get image
				image1.EndInit(); // Stop init image 

				Screen2Img.ImageSource = image1; // Show image
			}
			else
			{
				Screen2Border.Visibility = Visibility.Collapsed; // Hide
			}

			// Show "viewer"
			ScreenshotsViewer.Visibility = Visibility.Visible; // Show

			Screen1RadioBtn.IsChecked = true; // Check
		}

		/// <summary>
		/// The game's Id.
		/// </summary>
		public int Id { get; set; }

		Border CheckedBorder { get; set; }

		private void ResetBorders()
		{
			Screen1Border.BorderBrush = new SolidColorBrush { Color = Colors.Transparent }; // Set border brush 
			Screen2Border.BorderBrush = new SolidColorBrush { Color = Colors.Transparent }; // Set border brush 
		}

		private void Screen1RadioBtn_Checked(object sender, RoutedEventArgs e)
		{
			ResetBorders(); // Reset
			Screen1Border.BorderBrush = new SolidColorBrush { Color = Color.FromRgb(102, 0, 255) }; // Set border brush
			CheckedBorder = Screen1Border; // Set value
		}

		private void Screen2RadioBtn_Checked(object sender, RoutedEventArgs e)
		{
			ResetBorders(); // Reset
			Screen2Border.BorderBrush = new SolidColorBrush { Color = Color.FromRgb(102, 0, 255) }; // Set border brush
			CheckedBorder = Screen2Border; // Set value
		}

		private void Border_MouseEnter(object sender, MouseEventArgs e)
		{
			Border border = (Border)sender;
			border.BorderBrush = new SolidColorBrush { Color = Color.FromRgb(102, 0, 255) }; // Set border brush
		}

		private void Border_MouseLeave(object sender, MouseEventArgs e)
		{
			Border border = (Border)sender;
			if (CheckedBorder != border)
			{
				border.BorderBrush = new SolidColorBrush { Color = Colors.Transparent }; // Set border brush 
			}
		}

		private void Screen1Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			Screen1RadioBtn.IsChecked = true; // Check
			ResetBorders(); // Reset
			CheckedBorder = Screen1Border; // Set value
			Screen1Border.BorderBrush = new SolidColorBrush { Color = Color.FromRgb(102, 0, 255) }; // Set border brush
		}

		private void Screen2Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			Screen2RadioBtn.IsChecked = true; // Check
			ResetBorders(); // Reset
			CheckedBorder = Screen2Border; // Set value
			Screen2Border.BorderBrush = new SolidColorBrush { Color = Color.FromRgb(102, 0, 255) }; // Set border brush
		}

		private void ShowBtn_Click(object sender, RoutedEventArgs e)
		{
			InitScreenshotsUI();
			ShowBtn.Visibility = Visibility.Collapsed; // Hide
		}
	}
}
