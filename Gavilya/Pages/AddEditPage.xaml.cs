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
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Gavilya.Pages
{
	/// <summary>
	/// Interaction logic for AddEditPage.xaml
	/// </summary>
	public partial class AddEditPage : Page
	{
		internal AddGame AddGame { get; set; }
		internal EditGame EditGame { get; set; }
		internal string GameIconLocation { get; set; }
		internal int RAWGID { get; set; }
		internal string GameLocation { get; set; }

		bool isFromAdd;
		GameCard GameCard { get; set; }
		public AddEditPage(AddGame addGame)
		{
			InitializeComponent();
			AddGame = addGame; // Set
			isFromAdd = true;
			RAWGID = -1;
		}

		public AddEditPage(EditGame editGame, GameCard gameCard)
		{
			InitializeComponent();
			EditGame = editGame; // Set
			isFromAdd = false;
			RAWGID = gameCard.GameInfo.RAWGID; // Set
			GameCard = EditGame.GameCard; // Set
			InitUI();
		}

		private void InitUI()
		{
			// Text
			LocationTxt.Text = GameCard.GameInfo.FileLocation; // Set
			NameTextBox.Text = GameCard.GameInfo.Name; // Set
			VersionTextBox.Text = GameCard.GameInfo.Version; // Set

			GameIconLocation = GameCard.GameInfo.IconFileLocation; // Set
			
			// Image
			if (GameCard.GameInfo.IconFileLocation != string.Empty && GameCard.GameInfo.IconFileLocation != null)
			{
				Image.ImageSource = new BitmapImage(new Uri(GameCard.GameInfo.IconFileLocation));
			}
			else
			{
				Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(GameCard.GameInfo.FileLocation); // Grab the icon of the game
				Image.ImageSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()); // Show the image
			}
		}

		private void NextBtn_Click(object sender, RoutedEventArgs e)
		{
			if (!string.IsNullOrEmpty(NameTextBox.Text) && !string.IsNullOrEmpty(LocationTxt.Text))
			{
				if (isFromAdd)
				{
					AddGame.RAWGID = RAWGID; // Set
					AddGame.GameVersion = VersionTextBox.Text; // Set
					AddGame.GameName = NameTextBox.Text; // Set
					AddGame.GameLocation = GameLocation; // Set
					AddGame.GameIconLocation = GameIconLocation; // Set
					AddGame.ChangePage(1); // Change page
				}
				else
				{
					GameCard.GameInfo.Name = NameTextBox.Text; // Set
					GameCard.GameInfo.Version = VersionTextBox.Text; // Set
					GameCard.GameInfo.IconFileLocation = GameIconLocation; // Set
					GameCard.GameInfo.FileLocation = LocationTxt.Text; // Set

					EditGame.GameCard = GameCard; // Set

					EditGame.ChangePage(1); // Change page
				} 
			}
			else
			{
				MessageBox.Show(Properties.Resources.GameNeedsName, Properties.Resources.AddGame, MessageBoxButton.OK, MessageBoxImage.Exclamation);
			}
		}

		private void CancelBtn_Click(object sender, RoutedEventArgs e)
		{
			if (isFromAdd)
			{
				AddGame.Close();
			}
			else
			{
				EditGame.Close();
			}
		}

		private void ImageBrowseBtn_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new(); // OpenFileDialog
			openFileDialog.Filter = "PNG|*.png|JPG|*.jpg|Bitmap|*.bmp|All Files|*.*"; // Filter

			if (openFileDialog.ShowDialog() ?? true) // If the user selected a file
			{
				try
				{
					BitmapImage image = new(new Uri(openFileDialog.FileName)); // Create the image
					Image.ImageSource = image; // Set the GameImg's source to the image
					GameIconLocation = openFileDialog.FileName; // Set the path to the image
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); // Show the error
				}
			}
		}

		private async void LocationBorder_Drop(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				string[] files = (string[])e.Data.GetData(DataFormats.FileDrop); // Get all the files droped

				if (System.IO.Path.GetExtension(files[0]) == ".exe")
				{
					FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(files[0]);
					
					VersionTextBox.Text = fileVersionInfo.FileVersion; // Get the version
					LocationTxt.Text = files[0];
					NameTextBox.Text = string.IsNullOrEmpty(fileVersionInfo.ProductName) ? System.IO.Path.GetFileNameWithoutExtension(files[0]) : fileVersionInfo.ProductName;

					GameLocation = files[0]; // Set
					if (isFromAdd)
					{
						AddGame.GameDescription = string.Empty; // Reset
						GameIconLocation = string.Empty; // Reset 
					}

					if (isFromAdd && GameIconLocation == string.Empty) // If there is no image
					{
						try
						{
							GameIconLocation = await Global.GetCoverImageAsync(string.IsNullOrEmpty(fileVersionInfo.ProductName) ? System.IO.Path.GetFileNameWithoutExtension(files[0]) : fileVersionInfo.ProductName);

							if (GameIconLocation == string.Empty)
							{
								Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(files[0]); // Grab the icon of the game
								Image.ImageSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()); // Show the image
							}
							else
							{
								var bitmap = new BitmapImage(); // Create Bitmap
								var stream = File.OpenRead(GameIconLocation); // Create a stream

								bitmap.BeginInit(); // Init bitmap
								bitmap.CacheOption = BitmapCacheOption.OnLoad;
								bitmap.StreamSource = stream;
								bitmap.EndInit(); // End init bitmap
								stream.Close(); // Close the stream
								stream.Dispose(); // Release ressources
								bitmap.Freeze(); // Freeze bitmap

								Image.ImageSource = bitmap; // Show the image
							}
						}
						catch
						{

						}
					}
				}
			}
		}

		private async void LocationBorder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			OpenFileDialog openFileDialog = new(); // OpenFileDialog
			openFileDialog.Filter = "EXE|*.exe"; // Filter

			if (openFileDialog.ShowDialog() ?? true) // If the user selected a file
			{
				FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(openFileDialog.FileName); // Get the version

				NameTextBox.Text = string.IsNullOrEmpty(fileVersionInfo.ProductName) ? System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName) : fileVersionInfo.ProductName; // Name of the file
				VersionTextBox.Text = fileVersionInfo.FileVersion; // Version of the file
				LocationTxt.Text = openFileDialog.FileName; // Location of the file

				GameLocation = openFileDialog.FileName; // Set
				if (isFromAdd && GameIconLocation == string.Empty) // If there is no image
				{
					try
					{
						GameIconLocation = await Global.GetCoverImageAsync(string.IsNullOrEmpty(fileVersionInfo.ProductName) ? System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName) : fileVersionInfo.ProductName);
						RAWGID = await Global.GetGameId(string.IsNullOrEmpty(fileVersionInfo.ProductName) ? System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName) : fileVersionInfo.ProductName);

						if (GameIconLocation == string.Empty)
						{
							Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(openFileDialog.FileName); // Grab the icon of the game
							Image.ImageSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()); // Show the image
						}
						else
						{
							var bitmap = new BitmapImage(); // Create Bitmap
							var stream = File.OpenRead(GameIconLocation); // Create a stream

							bitmap.BeginInit(); // Init bitmap
							bitmap.CacheOption = BitmapCacheOption.OnLoad;
							bitmap.StreamSource = stream;
							bitmap.EndInit(); // End init bitmap
							stream.Close(); // Close the stream
							stream.Dispose(); // Release ressources
							bitmap.Freeze(); // Freeze bitmap

							Image.ImageSource = bitmap; // Show the image
						}
					}
					catch
					{

					}
				}
			}
		}

		private void RAWGImageBrowseBtn_Click(object sender, RoutedEventArgs e)
		{
			new SearchGameCover(this, GameAssociationActions.Search).Show(); // Show
		}
	}
}
