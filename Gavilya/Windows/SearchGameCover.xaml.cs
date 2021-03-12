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
using Gavilya.SDK.RAWG;
using Gavilya.UserControls;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Gavilya.Windows
{
    /// <summary>
    /// Logique d'interaction pour SearchGameCover.xaml
    /// </summary>
    public partial class SearchGameCover : Window
    {
        AddGame addGame1 = null;
        EditGame editGame1 = null;
        GameAssociationActions associationActions;
        public SearchGameCover(AddGame addGame, GameAssociationActions gameAssociationActions)
        {
            InitializeComponent();
            addGame1 = addGame;
            associationActions = gameAssociationActions; // Define the var
        }

        public SearchGameCover(EditGame editGame, GameAssociationActions gameAssociationActions)
        {
            InitializeComponent();
            editGame1 = editGame;
            associationActions = gameAssociationActions; // Define the var
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            ResultPresenter.Children.Clear(); // Remove all the controls
            var client = new RestClient(); // Create a REST Client
            client.BaseUrl = new Uri("https://api.rawg.io/api/games?"); // Configure the client
            var request = new RestRequest(Method.GET); // Create a request
            request.AddQueryParameter("search", GameSearchName.Text); // Config the request
            var response = await client.ExecuteAsync(request); // Execute the request and store the result

            var gameResults = JsonSerializer.Deserialize<GamesResults>(response.Content); // Deserialize the content of the reponse

            foreach (Gavilya.SDK.RAWG.Game game in gameResults.results) // For each result
            {
                ResultPresenter.Children.Add(new GameResult(game.name, game.id)); // Add the game
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
                if (uIElement is GameResult) // Check if the element is a GameResult
                {
                    gameResults.Add((GameResult)uIElement); // Add the result
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
                        string bgImage = await Global.GetCoverImageAsync(selectedGame.Id); // File name
                        LoadImageInWindow(bgImage, selectedGame.Id); // Load the image
                        break;
                }
                Close(); // Close the window
            }
        }

        private async void Associate(int id)
        {
            if (addGame1 != null) // If is from AddGame
            {
                addGame1.RAWGID = id; // Set the id
                addGame1.descriptionTxt.Text = await Global.GetGameDescriptionAsync(id); // Get the description
                addGame1.Platforms = await Global.GetGamePlatformsAsync(id); // Get the platforms
            }
            else // If is from EditGame
            {
                string description = await Global.GetGameDescriptionAsync(id); // Get the description
                editGame1.RAWGID = id; // Set the id
                editGame1.GameDescription = description;
                editGame1.descriptionTxt.Text = description;
                editGame1.Platforms = await Global.GetGamePlatformsAsync(id); // Get the platforms
            }
        }

        private async void LoadImageInWindow(string fileName, int id = -1)
        {
            if (addGame1 != null) // If is from AddGame
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
                addGame1.GameImg.Source = bitmap; // Set the image source
                addGame1.GameIconLocation = fileName; // Set the icon location
                addGame1.RAWGID = id; // Set the game id
                addGame1.GameDescription = await Global.GetGameDescriptionAsync(id); // Get the game's description
            }
            else // If is from EditGame
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
                editGame1.GameImg.Source = bitmap; // Set the image source
                editGame1.iconLocation = fileName; // Set the icon location
                editGame1.RAWGID = id; // Set the game id
                editGame1.GameDescription = await Global.GetGameDescriptionAsync(id); // Get the game's description
            }
        }
    }
}
