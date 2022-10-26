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
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Gavilya.UserControls;

/// <summary>
/// Interaction logic for FavoriteListItem.xaml
/// </summary>
public partial class FavoriteListItem : UserControl
{
	GameInfo GameInfo { get; init; }
	public FavoriteListItem(GameInfo gameInfo)
	{
		InitializeComponent();
		GameInfo = gameInfo;

		InitUI();
	}

	private void InitUI()
	{
		DateTime lastTimePlayed = Global.UnixTimeToDateTime(GameInfo.LastTimePlayed); // Get the date time

		GameNameTxt.Text = GameInfo.Name;
		TimePlayedTxt.Text = $"{Math.Round(GameInfo.TotalTimePlayed / 3600d)}{Properties.Resources.HourShort} \u00B7 {lastTimePlayed.Day} {Global.NumberToMonth(lastTimePlayed.Month)} {lastTimePlayed.Year}";

		DescTxt.Text = !string.IsNullOrEmpty(GameInfo.Description) && GameInfo.Description.Length > 121
			? GameInfo.Description[0..120].Replace("\n\n", "\n") + "..."
			: GameInfo.Description;


		if (!string.IsNullOrEmpty(GameInfo.IconFileLocation)) // If there is an image
		{
			var bitmap = new BitmapImage();
			var stream = File.OpenRead(GameInfo.IconFileLocation);

			bitmap.BeginInit();
			bitmap.CacheOption = BitmapCacheOption.OnLoad;
			bitmap.StreamSource = stream;
			bitmap.DecodePixelWidth = 249;
			bitmap.EndInit();
			stream.Close();
			stream.Dispose();
			bitmap.Freeze();
			Image.ImageSource = bitmap; // Put the icon of the game
		}
		else // If the image is the app icon
		{
			if (!GameInfo.IsUWP && !GameInfo.IsSteam) // If the game isn't UWP
			{
				Icon icon = Icon.ExtractAssociatedIcon(GameInfo.FileLocation); // Grab the icon of the game
				Image.ImageSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()); // Show the image

			}
		}
	}
}
