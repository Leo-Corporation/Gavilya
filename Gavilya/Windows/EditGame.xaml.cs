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
using Gavilya.Pages;
using Gavilya.UserControls;
using System.Collections.Generic;
using System.Windows;

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

	/// <summary>
	/// Window where the user can edit a game
	/// </summary>
	public EditGame(GameCard gameCard)
	{
		InitializeComponent();
		GameCard = gameCard; // Pass the arg

		AddEditPage = new(this, GameCard);
		AddEditPage2 = new(this, GameCard);

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
	}

	private void Button_Click(object sender, RoutedEventArgs e)
	{
		WindowState = WindowState.Minimized; // Minimize the window
	}

	private void Button_Click_1(object sender, RoutedEventArgs e)
	{
		Close(); // Close the window
	}
}
