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
using Gavilya.Pages;
using Gavilya.UserControls;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Gavilya.Windows;

/// <summary>
/// Logique d'interaction pour EditGame.xaml
/// </summary>
public partial class EditGame : Window
{
	public string iconLocation; // Icon location
	public int RAWGID = -1; // The Game RAWG Id
	public string GameDescription = string.Empty; // The description of the game

	public List<SDK.RAWG.Platform> Platforms = new(); // Create a new list
	public List<SDK.RAWG.Store> Stores = new();
	internal GameCard GameCard; // The game card

	public string GameName = string.Empty;
	public string GameVersion = string.Empty;
	public string GameLocation = string.Empty;

	readonly AddEditPage AddEditPage;
	readonly AddEditPage2 AddEditPage2;
	readonly AddEditPage3 AddEditPage3;

	/// <summary>
	/// Window where the user can edit a game
	/// </summary>
	public EditGame(GameCard gameCard)
	{
		InitializeComponent();
		GameCard = gameCard; // Pass the arg

		AddEditPage2 = new(this, GameCard);
		AddEditPage3 = new(this, GameCard);
		AddEditPage = new(this, GameCard, AddEditPage2);

		ChangePage(0);
	}

	internal void ChangePage(int id)
	{
		Content.Content = id switch
		{
			0 => AddEditPage,
			1 => AddEditPage2,
			2 => AddEditPage3,
			_ => AddEditPage
		}; // Set

		if (id == 0)
		{
			LineBorder.Background = Global.GetSolidColor("LightForeground");
			NumberBorder.Background = Global.GetSolidColor("LightForeground");
			Page2Btn.Foreground = Global.GetSolidColor("LightForeground");

			LineBorder2.Background = Global.GetSolidColor("LightForeground");
			LineBorder3.Background = Global.GetSolidColor("LightForeground");
			NumberBorder3.Background = Global.GetSolidColor("LightForeground");
			Page3Btn.Foreground = Global.GetSolidColor("LightForeground");


			_1Txt.Visibility = Visibility.Visible;
			CheckTxt.Visibility = Visibility.Collapsed;

			_2Txt.Visibility = Visibility.Visible;
			CheckTxt2.Visibility = Visibility.Collapsed;

		}
		else if (id == 1)
		{
			LineBorder2.Background = Global.GetSolidColor("LightForeground");
			LineBorder3.Background = Global.GetSolidColor("LightForeground");
			NumberBorder3.Background = Global.GetSolidColor("LightForeground");
			Page3Btn.Foreground = Global.GetSolidColor("LightForeground");

			LineBorder.Background = Global.GetSolidColor("Accent");
			NumberBorder.Background = Global.GetSolidColor("Accent");
			Page2Btn.Foreground = Global.GetSolidColor("Accent");

			_1Txt.Visibility = Visibility.Collapsed;
			CheckTxt.Visibility = Visibility.Visible;

			_2Txt.Visibility = Visibility.Visible;
			CheckTxt2.Visibility = Visibility.Collapsed;
		}
		else
		{
			LineBorder.Background = Global.GetSolidColor("Accent");
			NumberBorder.Background = Global.GetSolidColor("Accent");
			Page2Btn.Foreground = Global.GetSolidColor("Accent");

			LineBorder2.Background = Global.GetSolidColor("Accent");
			LineBorder3.Background = Global.GetSolidColor("Accent");
			NumberBorder3.Background = Global.GetSolidColor("Accent");
			Page3Btn.Foreground = Global.GetSolidColor("Accent");

			_1Txt.Visibility = Visibility.Collapsed;
			CheckTxt.Visibility = Visibility.Visible;

			_2Txt.Visibility = Visibility.Collapsed;
			CheckTxt2.Visibility = Visibility.Visible;
		}
	}

	private void Button_Click(object sender, RoutedEventArgs e)
	{
		WindowState = WindowState.Minimized; // Minimize the window
	}

	private void Button_Click_1(object sender, RoutedEventArgs e)
	{
		Close(); // Close the window
	}

	private void Page2Btn_Click(object sender, RoutedEventArgs e)
	{
		AddEditPage.NextBtn_Click(this, e);
	}

	private void Page1Btn_Click(object sender, RoutedEventArgs e)
	{
		ChangePage(0);
	}

	private void Page3Btn_Click(object sender, RoutedEventArgs e)
	{
		AddEditPage2.NextBtn_Click(this, e);
	}
}
