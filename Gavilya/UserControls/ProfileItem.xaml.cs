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
using Gavilya.Windows;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Gavilya.UserControls;

/// <summary>
/// Interaction logic for ProfileItem.xaml
/// </summary>
public partial class ProfileItem : UserControl
{
	Profile CurrentProfile { get; init; }
	public ProfileItem(Profile profile)
	{
		InitializeComponent();
		CurrentProfile = profile; // Set current profile

		InitUI(); // Load UI
	}

	private void InitUI()
	{
		if (CurrentProfile.PictureFilePath != "_default")
		{
			if (File.Exists(CurrentProfile.PictureFilePath))
			{
				var bitmap = new BitmapImage();
				var stream = File.OpenRead(CurrentProfile.PictureFilePath);

				bitmap.BeginInit();
				bitmap.CacheOption = BitmapCacheOption.OnLoad;
				bitmap.StreamSource = stream;
				bitmap.EndInit();
				stream.Close();
				stream.Dispose();
				bitmap.Freeze();

				ProfilePicture.ImageSource = bitmap; // Set image
			}
		}

		if (CurrentProfile == Definitions.Profiles[Definitions.Settings.CurrentProfileIndex]) // If this is the current profile
		{
			DeleteBtn.Visibility = Visibility.Collapsed; // Hide buttons
			SwitchBtn.Visibility = Visibility.Collapsed; // Hide buttons

			CurrentBorder.Visibility = Visibility.Visible; // Show
		}

		ProfileNameTxt.Text = CurrentProfile.Name; // Show name
	}

	private void EditBtn_Click(object sender, RoutedEventArgs e)
	{
		new AddEditProfileWindow(Enums.EditMode.Edit, CurrentProfile).Show(); // Edit
	}

	private void DeleteBtn_Click(object sender, RoutedEventArgs e)
	{
		if (MessageBox.Show(Properties.Resources.DeleteProfileMsg, Properties.Resources.Profiles, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
		{
			Definitions.Profiles.Remove(CurrentProfile); // Remove
			ProfileManager.SaveProfiles(); // Save changes
			Definitions.ProfilePage.LoadProfileUI();
		}
	}

	private void SwitchBtn_Click(object sender, RoutedEventArgs e)
	{
		Definitions.Settings.CurrentProfileIndex = Definitions.Profiles.IndexOf(CurrentProfile); // Set the new profile
		SettingsSaver.Save(); // Save

		Process.Start(AppDomain.CurrentDomain.BaseDirectory + @"\Gavilya.exe"); // Start Gavilya
		Environment.Exit(0); // Quit
	}
}
