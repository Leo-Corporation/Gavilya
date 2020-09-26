using Gavilya.SDK.RAWG;
using Gavilya.UserControls;
using RestSharp;
using System;
using System.Collections.Generic;
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
        public SearchGameCover()
        {
            InitializeComponent();
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
    }
}
