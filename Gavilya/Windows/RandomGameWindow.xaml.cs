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
using System.Windows.Shapes;

namespace Gavilya.Windows;
/// <summary>
/// Interaction logic for RandomGameWindow.xaml
/// </summary>
public partial class RandomGameWindow : Window
{
	public GameInfo GameInfo { get; init; }
	public int Index { get; init; }
	public RandomGameWindow(GameInfo gameInfo, int i)
	{
		InitializeComponent();
		GameInfo = gameInfo;
		Index = i;
		InitUI();
	}

	private void InitUI()
	{
		// Name
		GameNameTxt.Text = GameInfo.Name;

		// Icon
		try
		{
			if (GameInfo.IconFileLocation != string.Empty && GameInfo.IconFileLocation != null) // If a custom image is used
			{
				var bitmap = new BitmapImage();
				var stream = File.OpenRead(GameInfo.IconFileLocation);

				bitmap.BeginInit();
				bitmap.CacheOption = BitmapCacheOption.OnLoad;
				bitmap.StreamSource = stream;
				bitmap.DecodePixelWidth = 256;
				bitmap.EndInit();
				stream.Close();
				stream.Dispose();
				bitmap.Freeze();
				GameIcon.ImageSource = bitmap;
			}
			else
			{
				if (!GameInfo.IsUWP && !GameInfo.IsSteam) // If the game isn't UWP
				{
					System.Drawing.Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(GameInfo.FileLocation);
					GameIcon.ImageSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()); // Show the image 
				}
			}
		}
		catch
		{
			GameIcon.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Gavilya;component/Assets/PC.png")); // Show the default image
		}
	}

	private void PlayBtn_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			GameCard? gameCard = Global.GamesCardsPages.GamePresenter.Children[Index] as GameCard;
			Global.GameInfoPage.InitializeUI(GameInfo, gameCard);
			Global.MainWindow.PageContent.Navigate(Global.GameInfoPage);
			Global.GameInfoPage.PlayBtn_Click(this, e);
			Close();
		}
		catch { }
	}

	private void CloseBtn_Click(object sender, RoutedEventArgs e)
	{
		Close();
	}
}
