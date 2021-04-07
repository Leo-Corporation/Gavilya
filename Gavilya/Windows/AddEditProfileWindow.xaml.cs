﻿/*
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
using LeoCorpLibrary;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Gavilya.Windows
{
	/// <summary>
	/// Interaction logic for AddEditProfileWindow.xaml
	/// </summary>
	public partial class AddEditProfileWindow : Window
	{
		EditMode EditMode { get; init; }
		Profile CurrentProfile { get; set; }
		Profile EditProfile { get; init; }
		Profile BaseProfile { get; init; }
		public AddEditProfileWindow(EditMode editMode, Profile profile = null)
		{
			InitializeComponent();
			EditMode = editMode;
			BaseProfile = profile;
			EditProfile = profile;

			if (EditMode == EditMode.Edit && profile is not null)
			{
				InitUIEdit(); // Launch the UI
			}
			else
			{
				CurrentProfile = new();
			}
		}

		private void InitUIEdit()
		{
			OKBtn.Content = Properties.Resources.Edit; // Change the text
			nameTxt.Text = EditProfile.Name; // Set text

			if (EditProfile.PictureFilePath != "_default")
{
				BitmapImage image = new(new Uri(EditProfile.PictureFilePath)); // Create the image
				ProfilePicture.ImageSource = image; // Set the GameImg's source to the image
			}
		}

		private void CloseBtn_Click(object sender, RoutedEventArgs e)
		{
			Close(); // Closes the window
		}

		private void OKBtn_Click(object sender, RoutedEventArgs e)
		{
			if (!string.IsNullOrEmpty(nameTxt.Text) || !string.IsNullOrWhiteSpace(nameTxt.Text))
			{
				if (EditMode == EditMode.Edit) // If edit
				{
					Definitions.Profiles[Definitions.Profiles.IndexOf(BaseProfile)] = EditProfile; // Edit profile
				}
				else
				{
					Random random = new();
					CurrentProfile.Name = nameTxt.Text;
					CurrentProfile.SaveFilePath = $@"{Env.AppDataPath}\Gavilya\Games-{CurrentProfile.Name}-{random.Next(0, 9999999)}";
					Definitions.Profiles.Add(CurrentProfile); // Add profile
				}
				ProfileManager.SaveProfiles();
				Close(); // Closes the window 
			}
		}

		private void CancelBtn_Click(object sender, RoutedEventArgs e)
		{
			Close(); // Closes the window
		}

		private void BrowseBtn_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new(); // OpenFileDialog
			openFileDialog.Filter = "PNG|*.png|JPG|*.jpg|Bitmap|*.bmp|All Files|*.*"; // Filter

			if (openFileDialog.ShowDialog() ?? true) // If the user selected a file
			{
				try
				{
					BitmapImage image = new(new Uri(openFileDialog.FileName)); // Create the image
					ProfilePicture.ImageSource = image; // Set the GameImg's source to the image

					if (EditMode == EditMode.Add)
					{
						CurrentProfile.PictureFilePath = openFileDialog.FileName; // Set the path to the image 
					}
					else
					{
						EditProfile.PictureFilePath = openFileDialog.FileName; // Set the path to the image 
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); // Show the error
				}
			}
		}
	}
}
