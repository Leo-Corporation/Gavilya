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
using Gavilya.SDK.RAWG;
using Gavilya.UserControls;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Gavilya.Windows;

/// <summary>
/// Logique d'interaction pour SearchGameCover.xaml
/// </summary>
public partial class SearchGameCover : Window
{
	readonly AddEditPage AddEditPage;
	readonly AddEditPage2 AddEditPage2;
	readonly GameAssociationActions associationActions;

	public SearchGameCover(UIElement parent, GameAssociationActions gameAssociationActions)
	{
		InitializeComponent();
		associationActions = gameAssociationActions; // Define the var
		if (parent is AddEditPage page)
		{
			AddEditPage = page; // Set
		}
		else if (parent is AddEditPage2 page1)
		{
			AddEditPage2 = page1; // Set
		}
	}
	public SearchGameCover(AddEditPage page, AddEditPage2 page1, GameAssociationActions gameAssociationActions)
	{
		InitializeComponent();
		associationActions = gameAssociationActions; // Define the var
		AddEditPage = page; // Set
		AddEditPage2 = page1; // Set
	}

	private async void Button_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			ResultPresenter.Children.Clear(); // Remove all the controls
			var client = new RestClient(new Uri("https://api.rawg.io/api/games?")); // Create a REST Client
			var request = new RestRequest
			{
				Method = Method.Get
			}; // Create a request
			request.AddQueryParameter("search", GameSearchName.Text); // Config the request
			request.AddQueryParameter("key", APIKeys.RAWGAPIKey);
			var response = await client.ExecuteAsync(request); // Execute the request and store the result

			var gameResults = JsonSerializer.Deserialize<GamesResults>(response.Content); // Deserialize the content of the reponse

			foreach (Gavilya.SDK.RAWG.Game game in gameResults.results) // For each result
			{
				ResultPresenter.Children.Add(new GameResult(game.name, game.id)); // Add the game
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}

	private void Button_Click_1(object sender, RoutedEventArgs e)
	{
		WindowState = WindowState.Minimized; // Minimize the window
	}

	private void Button_Click_2(object sender, RoutedEventArgs e)
	{
		Close(); // Close the window
	}

	private async void SelectGame_Click(object sender, RoutedEventArgs e)
	{
		List<GameResult> gameResults = new(); // Results
		List<GameResult> selectedGameResults = new(); // Results
		GameResult selectedGame; // The selected GameResult

		foreach (UIElement uIElement in ResultPresenter.Children) // For each result
		{
			if (uIElement is GameResult result) // Check if the element is a GameResult
			{
				gameResults.Add(result); // Add the result
			}
		}

		foreach (GameResult gameResult in gameResults)
		{
			if (gameResult.GameCheck.IsChecked ?? true) // If the game is selected
			{
				selectedGameResults.Add(gameResult); // Add the game
			}
		}

		if (selectedGameResults.Count > 0)
		{
			selectedGame = selectedGameResults[0]; // Define the selected game
			switch (associationActions)
			{
				case GameAssociationActions.Associate: // If the action is to associate
					Associate(selectedGame.Id); // Associate the game
					break;
				case GameAssociationActions.Search: // If the action is to get the cover (search)
					string bgImage = await Global.GetCoverImageAsync(selectedGame.Id, selectedGame.SelectedScreenshot); // File name
					LoadImageInWindow(bgImage, selectedGame.Id); // Load the image
					break;
				case GameAssociationActions.Both:
					Associate(selectedGame.Id); // Associate the game
					string bgImage2 = await Global.GetCoverImageAsync(selectedGame.Id, selectedGame.SelectedScreenshot); // File name
					LoadImageInWindow(bgImage2, selectedGame.Id); // Load the image
					break;
			}
			Close(); // Close the window
		}
	}

	private async void Associate(int id)
	{
		AddEditPage2.RAWGID = id; // Set the id
		AddEditPage2.GameDescription = await Global.GetGameDescriptionAsync(id); // Get the description
		AddEditPage2.Platforms = await Global.GetGamePlatformsAsync(id); // Get the platforms
		AddEditPage2.Stores = await Global.GetStoresAsync(id); // Get stores

		// UI
		AddEditPage2.DescriptionTextBox.Text = AddEditPage2.GameDescription; // Set
		AddEditPage2.AssociateTxt.Text = Properties.Resources.Associated; // Set
		AddEditPage2.AssociateIconTxt.Text = "\uE98E"; // Set
	}

	private void LoadImageInWindow(string fileName, int id = -1)
	{
		var bitmap = new BitmapImage(); // Create a bitmapo
		var stream = File.OpenRead(fileName); // Open the image

		bitmap.BeginInit(); // Init bitmap
		bitmap.CacheOption = BitmapCacheOption.OnLoad; // Config bitmap
		bitmap.StreamSource = stream; // Define the data
		bitmap.EndInit(); // End init
		stream.Close(); // Close streame
		stream.Dispose(); // Releasen stream's used ressources
		bitmap.Freeze(); // Freeze the bitmap
						 //addGame1.GameImg.Source = bitmap; // Set the image source
		AddEditPage.GameIconLocation = fileName; // Set the icon location
		AddEditPage.RAWGID = id; // Set the game id
		AddEditPage.Image.ImageSource = bitmap; // Set image source
	}
}
