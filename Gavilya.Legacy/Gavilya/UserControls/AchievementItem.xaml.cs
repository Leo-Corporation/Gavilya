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
using Gavilya.SDK.RAWG;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Gavilya.UserControls;

/// <summary>
/// Interaction logic for AchievementItem.xaml
/// </summary>
public partial class AchievementItem : UserControl
{
	public AchievementItem(Achievement achievement)
	{
		InitializeComponent();
		InitUI(achievement); // Loads the UI
	}

	private void InitUI(Achievement achievement)
	{
		try
		{
			// Load text
			AchievementNameTxt.Text = achievement.name; // Set text
			AchievementDescriptionTxt.Text = achievement.description; // Set text
			AchievementPourcentTxt.Text = $"{achievement.percent}% {Properties.Resources.AchievementPlayerUnlocked}"; // Set text

			// Rare
			if (double.Parse(achievement.percent, CultureInfo.InvariantCulture) < 1)
			{
				ItemBorder.BorderBrush = (LinearGradientBrush)Application.Current.Resources["GoldGradient"];
			}

			// Load the image
			var image = new BitmapImage();
			image.BeginInit();
			image.UriSource = new Uri(achievement.image);
			image.DecodePixelWidth = 100;
			image.EndInit();

			AchievementImg.Source = image; // Set the image

		}
		catch { } // In case the image isn't loaded properly
	}
}
