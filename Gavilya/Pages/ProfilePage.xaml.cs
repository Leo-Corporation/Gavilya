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

		private void InitUI()
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
		}

		private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
		{
			ScrollViewer scv = (ScrollViewer)sender;
			scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta / 2);
			e.Handled = true;
		}

		internal Button CheckedButton { get; set; }

		private void SpotlightTabBtn_Click(object sender, RoutedEventArgs e)
		{

		}

		private void FavoriteTabBtn_Click(object sender, RoutedEventArgs e)
		{

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
	}
}
