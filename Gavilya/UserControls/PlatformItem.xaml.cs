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
using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Gavilya.UserControls
{
	/// <summary>
	/// Interaction logic for PlatformItem.xaml
	/// </summary>
	public partial class PlatformItem : UserControl
	{
		SDK.RAWG.Platform Platform { get; init; }
		public PlatformItem(SDK.RAWG.Platform platform)
		{
			InitializeComponent();
			Platform = platform; // Set

			InitUI(); // Load the UI
		}

		private void InitUI()
		{
			string imgPath = Platform.name switch
			{
				"Xbox" => "pack://application:,,,/Gavilya;component/Assets/Xbox.png",
				"Xbox 360" => "pack://application:,,,/Gavilya;component/Assets/Xbox.png",
				"Xbox One" => "pack://application:,,,/Gavilya;component/Assets/Xbox.png",
				"Xbox Series S/X" => "pack://application:,,,/Gavilya;component/Assets/Xbox.png",
				"PlayStation" => "pack://application:,,,/Gavilya;component/Assets/PlayStation.png",
				"PlayStation 2" => "pack://application:,,,/Gavilya;component/Assets/PlayStation.png",
				"PlayStation 3" => "pack://application:,,,/Gavilya;component/Assets/PlayStation.png",
				"PlayStation 4" => "pack://application:,,,/Gavilya;component/Assets/PlayStation.png",
				"PlayStation 5" => "pack://application:,,,/Gavilya;component/Assets/PlayStation.png",
				"PS Vita" => "pack://application:,,,/Gavilya;component/Assets/PlayStation.png",
				"PSP" => "pack://application:,,,/Gavilya;component/Assets/PlayStation.png",
				"PC" => "pack://application:,,,/Gavilya;component/Assets/PC.png",
				"Linux" => "pack://application:,,,/Gavilya;component/Assets/Linux.png",
				"macOS" => "pack://application:,,,/Gavilya;component/Assets/Apple.png",
				"iOS" => "pack://application:,,,/Gavilya;component/Assets/Apple.png",
				"Apple II" => "pack://application:,,,/Gavilya;component/Assets/Apple.png",
				"Android" => "pack://application:,,,/Gavilya;component/Assets/Android.png",
				"Nintendo DS" => "pack://application:,,,/Gavilya;component/Assets/Nintendo.png",
				"Nintendo DSi" => "pack://application:,,,/Gavilya;component/Assets/Nintendo.png",
				"Nintendo 2DS" => "pack://application:,,,/Gavilya;component/Assets/Nintendo.png",
				"Nintendo 3DS" => "pack://application:,,,/Gavilya;component/Assets/Nintendo.png",
				"Nintendo 64" => "pack://application:,,,/Gavilya;component/Assets/Nintendo.png",
				"Nintendo Switch" => "pack://application:,,,/Gavilya;component/Assets/Nintendo.png",
				"GameCube" => "pack://application:,,,/Gavilya;component/Assets/Nintendo.png",
				"GameBoy" => "pack://application:,,,/Gavilya;component/Assets/Nintendo.png",
				"GameBoy Advance" => "pack://application:,,,/Gavilya;component/Assets/Nintendo.png",
				"GameBoy Color" => "pack://application:,,,/Gavilya;component/Assets/Nintendo.png",
				"NES" => "pack://application:,,,/Gavilya;component/Assets/Nintendo.png",
				"SNES" => "pack://application:,,,/Gavilya;component/Assets/Nintendo.png",
				"Wii" => "pack://application:,,,/Gavilya;component/Assets/Nintendo.png",
				"Wii U" => "pack://application:,,,/Gavilya;component/Assets/Nintendo.png",
				_ => "pack://application:,,,/Gavilya;component/Assets/PC.png",
			};

			IconImg.Source = new BitmapImage(new Uri(imgPath));

			// Text
			PlatformNameTxt.Text = Platform.name; // Set text
		}
	}
}
