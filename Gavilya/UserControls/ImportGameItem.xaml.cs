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
using LeoCorpLibrary;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

namespace Gavilya.UserControls
{
	/// <summary>
	/// Interaction logic for ImportGameItem.xaml
	/// </summary>
	public partial class ImportGameItem : UserControl
	{
		public GameInfo GameInfo { get; set; }

		public ImportGameItem(GameInfo gameInfo)
		{
			InitializeComponent();
			GameInfo = gameInfo;

			InitUI(); // Load the UI
		}

		private void InitUI()
		{
			NameTxt.Text = GameInfo.Name; // Set text
			LocationTxt.Text = (GameInfo.FileLocation.Length > 32) ? GameInfo.FileLocation.Substring(0, 32) + "..." : GameInfo.FileLocation; // Set text
			LocationToolTip.Content = GameInfo.FileLocation; // Set content
			IconLocationTxt.Text = (GameInfo.IconFileLocation.Length > 32) ? GameInfo.IconFileLocation.Substring(0, 32) + "..." : GameInfo.IconFileLocation; // Set text
			IconLocationToolTip.Content = GameInfo.IconFileLocation; // Set content

			GetRAWGImageBtn.Visibility = (GameInfo.RAWGID != -1) ? Visibility.Visible : Visibility.Collapsed; // Set
			LocationWarningTxt.Visibility = File.Exists(GameInfo.FileLocation) ? Visibility.Collapsed : Visibility.Visible; // Set

			if (!string.IsNullOrEmpty(GameInfo.IconFileLocation))
			{
				IconLocationWarningTxt.Visibility = File.Exists(GameInfo.IconFileLocation) ? Visibility.Collapsed : Visibility.Visible; // Set 
			}
		}

		private void BrowseBtn_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new(); // OpenFileDialog
			openFileDialog.Filter = "EXE|*.exe"; // Filter

			if (openFileDialog.ShowDialog() ?? true)
			{
				GameInfo.FileLocation = openFileDialog.FileName; // Set value
				InitUI();
			}
		}

		private void IconBrowseBtn_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new(); // OpenFileDialog
			openFileDialog.Filter = "PNG|*.png|JPG|*.jpg|Bitmap|*.bmp|All Files|*.*"; // Filter

			if (openFileDialog.ShowDialog() ?? true)
			{
				try
				{
					GameInfo.IconFileLocation = openFileDialog.FileName; // Set
					InitUI();
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); // Show the error
				}
			}
		}

		private async void GetRAWGImageBtn_Click(object sender, RoutedEventArgs e)
		{
			if (await NetworkConnection.IsAvailableAsync())
			{
				GameInfo.IconFileLocation = await Global.GetCoverImageAsync(GameInfo.RAWGID); // Set path
				InitUI(); // Refresh the UI 
			}
			else
			{
				MessageBox.Show(Properties.Resources.FeatureNeedsInternet, Properties.Resources.MainWindowTitle, MessageBoxButton.OK, MessageBoxImage.Information);
			}
		}
	}
}
