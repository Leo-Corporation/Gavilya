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

namespace Gavilya.UserControls
{
	/// <summary>
	/// Interaction logic for FavoriteSideBarItem.xaml
	/// </summary>
	public partial class FavoriteSideBarItem : UserControl
	{
		UIElement Parent { get; init; }
		public FavoriteSideBarItem(GameInfo gameInfo, UIElement parent)
		{
			InitializeComponent();
			Parent = parent;

			InitUI(gameInfo);
		}

		private void InitUI(GameInfo gameInfo)
		{
			// Tooltip
			GameNameToolTip.Content = gameInfo.Name;

			try
			{
				if (!string.IsNullOrEmpty(gameInfo.IconFileLocation)) // If there is an image
				{
					var bitmap = new BitmapImage();
					var stream = File.OpenRead(gameInfo.IconFileLocation);

					bitmap.BeginInit();
					bitmap.CacheOption = BitmapCacheOption.OnLoad;
					bitmap.StreamSource = stream;
					bitmap.DecodePixelWidth = 80;
					bitmap.EndInit();
					stream.Close();
					stream.Dispose();
					bitmap.Freeze();
					GameIcon.ImageSource = bitmap; // Put the icon of the game
				}
				else // If the image is the app icon
				{
					if (!gameInfo.IsUWP && !gameInfo.IsSteam) // If the game isn't UWP
					{
						Icon icon = Icon.ExtractAssociatedIcon(gameInfo.FileLocation); // Grab the icon of the game
						GameIcon.ImageSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()); // Show the image
					}
				}
			}
			catch
			{
				GameIcon.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Gavilya;component/Assets/PC.png")); // Show the default image
			}
		}

		private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (Parent is GameCard)
			{
				((GameCard)Parent).GameCardBorder_MouseLeftButtonUp(this, e);
			}
		}
	}
}
