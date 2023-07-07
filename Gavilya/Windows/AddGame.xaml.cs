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
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace Gavilya.Windows;

/// <summary>
/// Logique d'interaction pour AddGame.xaml
/// </summary>
public partial class AddGame : Window
{
	public GameInfo GameInfo { get; set; }

	readonly AddEditPage AddEditPage;
	readonly AddEditPage2 AddEditPage2;
	readonly AddEditPage3 AddEditPage3;
	public AddGame(bool isUWP, bool isSteam)
	{
		InitializeComponent();

		GameInfo = new()
		{
			IsUWP = isUWP,
			IsSteam = isSteam,
			IsHidden = false,
			IconFileLocation = string.Empty,
			Description = string.Empty,
			Platforms = new(),
			Stores = new(),
			Name = string.Empty,
			Version = string.Empty,
			FileLocation = string.Empty,
			IsFavorite = false,
			RAWGID = -1,
			AssociatedTags = new(),
			ProcessName = string.Empty,
			TotalTimePlayed = 0,
			LastTimePlayed = 0,
			AlwaysCheckIfRunning = false
		};


		AddEditPage2 = new(this);
		AddEditPage = new(this, AddEditPage2);
		AddEditPage3 = new(this);

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

	private void Window_Loaded(object sender, RoutedEventArgs e)
	{
		Title = Thread.CurrentThread.CurrentUICulture.ToString() switch // Language of the user
		{
			// Language: French (France)
			"fr-FR" => "Ajouter un jeu",// Change the title
										// Language: English (United States)
			"en-US" => "Add a game",// Change the title
									// Language: English (United States)
			_ => "Add a game",// Change the title
		};
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
