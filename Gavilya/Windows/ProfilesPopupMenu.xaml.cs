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
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Gavilya.Windows;

/// <summary>
/// Interaction logic for ProfilesPopupMneu.xaml
/// </summary>
public partial class ProfilesPopupMenu : Window
{
	Profile CurrentProfile { get; init; }
	public ProfilesPopupMenu()
	{
		InitializeComponent();
		CurrentProfile = Definitions.Profiles[Definitions.Settings.CurrentProfileIndex]; // Current profile

		InitUI(); // Load the UI
	}

	internal void InitUI()
	{
		ProfileDisplayer.Children.Clear(); // Clear all profile items

		if (CurrentProfile.PictureFilePath != "_default")
		{
			if (File.Exists(CurrentProfile.PictureFilePath))
			{
				var bitmap = new BitmapImage();
				var stream = File.OpenRead(CurrentProfile.PictureFilePath);

				bitmap.BeginInit();
				bitmap.DecodePixelWidth = 50;
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

	private void Window_Deactivated(object sender, EventArgs e)
	{
		if (Definitions.IsProfileMenuVisible)
		{
			Hide(); // Close
			Definitions.IsProfileMenuVisible = false; // Define
		}
	}

	private void AddProfileBtn_Click(object sender, RoutedEventArgs e)
	{
		new AddEditProfileWindow(EditMode.Add).Show(); // Add
	}

	private void EditProfileBtn_Click(object sender, RoutedEventArgs e)
	{
		new AddEditProfileWindow(EditMode.Edit, CurrentProfile).Show(); // Edit
	}

	private void StatsBtn_Click(object sender, RoutedEventArgs e)
	{
		Definitions.MainWindow.PageContent.Content = Definitions.HomePage; // Navigate to home
	}
}
