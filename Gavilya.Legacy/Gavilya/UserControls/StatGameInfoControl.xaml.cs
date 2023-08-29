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
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Gavilya.UserControls;

/// <summary>
/// Interaction logic for StatGameInfoControl.xaml
/// </summary>
public partial class StatGameInfoControl : UserControl
{
	public StatGameInfoControl()
	{
		InitializeComponent();
	}

	internal void InitUI(GameInfo gameInfo)
	{
		// Text
		GameNameTxt.Text = gameInfo.Name; // Set text
		DescriptionTxt.Text = gameInfo.Description; // Set text

		double timePlayed = (double)gameInfo.TotalTimePlayed / 3600;
		TotalTimePlayedTxt.Text = $"{string.Format("{0:0.#}", timePlayed)}{Properties.Resources.HourShort}";

		if (gameInfo.LastTimePlayed != 0) // If the game was played
		{
			DateTime LastTimePlayed = Global.UnixTimeToDateTime(gameInfo.LastTimePlayed); // Get the date time
			LastTimePlayedTxt.Text = $"{LastTimePlayed.Day} {Global.NumberToMonth(LastTimePlayed.Month)} {LastTimePlayed.Year}"; // Last time played
		}
		else
		{
			LastTimePlayedTxt.Text = Properties.Resources.Never; // Set the text
		}

		// Icon
		try
		{
			if (!string.IsNullOrEmpty(gameInfo.IconFileLocation)) // If a custom image is used
			{
				var bitmap = new BitmapImage();
				var stream = File.OpenRead(gameInfo.IconFileLocation);

				bitmap.BeginInit();
				bitmap.CacheOption = BitmapCacheOption.OnLoad;
				bitmap.StreamSource = stream;
				bitmap.EndInit();
				stream.Close();
				stream.Dispose();
				bitmap.Freeze();
				BackgroundImage.ImageSource = bitmap;
			}
			else
			{
				System.Drawing.Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(gameInfo.FileLocation);
				BackgroundImage.ImageSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()); // Show the image
			}
		}
		catch
		{
			BackgroundImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Gavilya;component/Assets/PC.png")); // Show the default image
		}
	}
}
