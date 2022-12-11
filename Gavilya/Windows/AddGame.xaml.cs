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
	public string GameIconLocation = string.Empty;
	public string GameDescription = string.Empty; // The description of the game
	public List<SDK.RAWG.Platform> Platforms = new();
	public List<SDK.RAWG.Store> Stores = new();
	public int RAWGID = -1;
	public string GameName = string.Empty;
	public string GameVersion = string.Empty;
	public string GameLocation = string.Empty;
	internal bool? Hidden { get; set; }
	public bool IsUWP { get; set; }
	public bool IsSteam { get; set; }

	readonly AddEditPage AddEditPage;
	readonly AddEditPage2 AddEditPage2;
	public AddGame(bool isUWP, bool isSteam)
	{
		InitializeComponent();
		IsUWP = isUWP; // Set
		IsSteam = isSteam; // Set
		AddEditPage = new(this);
		AddEditPage2 = new(this);

		ChangePage(0);
	}

	internal void ChangePage(int id)
	{
		Content.Content = id switch
		{
			0 => AddEditPage,
			1 => AddEditPage2,
			_ => AddEditPage
		}; // Set

		if (id == 1)
		{
			LineBorder.Background = Definitions.HomeButtonBackColor;
			NumberBorder.Background = Definitions.HomeButtonBackColor;
			Page2Btn.Foreground = Definitions.HomeButtonBackColor;

			_1Txt.Visibility = Visibility.Collapsed;
			CheckTxt.Visibility = Visibility.Visible;
		}
		else
		{
			LineBorder.Background = new SolidColorBrush { Color = Color.FromRgb(50, 50, 70) };
			NumberBorder.Background = new SolidColorBrush { Color = Color.FromRgb(50, 50, 70) };
			Page2Btn.Foreground = new SolidColorBrush { Color = Color.FromRgb(50, 50, 70) };

			_1Txt.Visibility = Visibility.Visible;
			CheckTxt.Visibility = Visibility.Collapsed;
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
}
