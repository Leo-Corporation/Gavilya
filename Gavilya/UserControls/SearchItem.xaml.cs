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

using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Gavilya.UserControls;
/// <summary>
/// Interaction logic for SearchItem.xaml
/// </summary>
public partial class SearchItem : UserControl
{
	internal GameCard ParentGameCard { get; init; }
	public SearchItem(GameCard parent)
	{
		InitializeComponent();
		ParentGameCard = parent; // Define the parent element

		InitUI(); // Load the UI
	}

	private void InitUI()
	{
		// Load the Image
		if (ParentGameCard.GameInfo.IconFileLocation != string.Empty && ParentGameCard.GameInfo.IconFileLocation != null) // If a custom image is used
		{
			var bitmap = new BitmapImage();
			var stream = File.OpenRead(ParentGameCard.GameInfo.IconFileLocation);

			bitmap.BeginInit();
			bitmap.CacheOption = BitmapCacheOption.OnLoad;
			bitmap.StreamSource = stream;
			bitmap.DecodePixelWidth = 80;
			bitmap.EndInit();
			stream.Close();
			stream.Dispose();
			bitmap.Freeze();
			GameImg.Source = bitmap;
		}
		else
		{
			if (!ParentGameCard.GameInfo.IsUWP && !ParentGameCard.GameInfo.IsSteam) // If the game isn't UWP
			{
				System.Drawing.Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(ParentGameCard.GameInfo.FileLocation);
				GameImg.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()); // Show the image 
			}
		}

		// Set the Game name
		GameName.Text = ParentGameCard.GameInfo.Name;
	}

	public override string ToString()
	{
		return ParentGameCard.GameInfo.Name;
	}

	internal void UserControl_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
	{
		ParentGameCard.GameCardBorder_MouseLeftButtonUp(this, e);
	}
}
