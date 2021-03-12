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
using Gavilya.SDK.RAWG;
using LeoCorpLibrary;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Gavilya.Classes
{
    /// <summary>
    /// A class that contains methods and more to do global things in the app.
    /// </summary>
    public static class Global
    {
        /// <summary>
        /// Sets the window icon using <c>\Gavilya.ico</c>.
        /// </summary>
        /// <param name="window"><see cref="Window"/> to set the icon to.</param>
        public static void SetWindowIcon(Window window)
        {
            Uri icon = new(AppDomain.CurrentDomain.BaseDirectory + @"\Gavilya.ico"); // Define the path to the icon
            window.Icon = BitmapFrame.Create(icon); // Set the icon
        }

        /// <summary>
        /// Gets the game's image.
        /// </summary>
        /// <param name="gameName">Name of the game to get the image from.</param>
        /// <returns>A <seealso cref="Task{string}"/> value.</returns>
        public static async Task<string> GetCoverImageAsync(string gameName)
        {
            var client = new RestClient(); // Create a REST Client
            client.BaseUrl = new Uri("https://api.rawg.io/api/games?"); // Configure the client
            var request = new RestRequest(Method.GET); // Create a request
            request.AddQueryParameter("search", gameName); // Config the request
            var response = await client.ExecuteAsync(request); // Execute the request and store the result

            var gameResults = JsonSerializer.Deserialize<GamesResults>(response.Content); // Deserialize the content of the reponse

            if (gameResults.results.Count > 0) // If there is results
            {
                if (gameResults.results[0].background_image == null)
                {
                    return string.Empty;
                }

                int gameID = gameResults.results[0].id; // Get the firts result's id

                if (!Directory.Exists(Env.GetAppDataPath() + @"\Gavilya\Games")) // If the directory doesn't exist
                {
                    Directory.CreateDirectory(Env.GetAppDataPath() + @"\Gavilya\Games"); // Create the directory
                }

                if (!Directory.Exists(Env.GetAppDataPath() + @$"\Gavilya\games\{gameID}")) // If the directory doesn't exist
                {
                    Directory.CreateDirectory(Env.GetAppDataPath() + $@"\Gavilya\Games\{gameID}"); // Create the game directory
                }
                else
                {
                    if (File.Exists(Env.GetAppDataPath() + $@"\Gavilya\Games\{gameID}\bg_img.jpg")) // If the image exist
                    {
                        return Env.GetAppDataPath() + $@"\Gavilya\Games\{gameID}\bg_img.jpg";
                    }
                    else
                    {
                        WebClient webClient1 = new(); // Create a web client
                        await webClient1.DownloadFileTaskAsync(gameResults.results[0].background_image, Env.GetAppDataPath() + @$"\Gavilya\Games\{gameID}\bg_img.jpg"); // Download the "background_image"
                        webClient1.Dispose(); // Release all used ressources

                        return Env.GetAppDataPath() + @$"\Gavilya\Games\{gameID}\bg_img.jpg"; // Return the result
                    }
                }


                WebClient webClient = new(); // Create a WebClient
                await webClient.DownloadFileTaskAsync(gameResults.results[0].background_image, Env.GetAppDataPath() + @$"\Gavilya\Games\{gameID}\bg_img.jpg"); // Download the "background_image"
                webClient.Dispose(); // Release used ressources

                return Env.GetAppDataPath() + @$"\Gavilya\Games\{gameID}\bg_img.jpg"; // Return the path
            }
            else // If there isn't any results
            {
                return string.Empty; // Empty means that Gavilya will use the executable icon
            }
        }

        /// <summary>
        /// Gets the game's image.
        /// </summary>
        /// <param name="id">The game's Id.</param>
        /// <returns>A <see cref="Task{string}"/> value.</returns>
        public static async Task<string> GetCoverImageAsync(int id)
        {
            var client = new RestClient(); // Create a REST Client
            client.BaseUrl = new Uri($"https://api.rawg.io/api/games/{id}"); // Configure the client
            var request = new RestRequest(Method.GET); // Create a request
            var response = await client.ExecuteAsync(request); // Execute the request and store the result

            var game = JsonSerializer.Deserialize<Game>(response.Content); // Deserialize the content of the reponse

            if (game.background_image == null)
            {
                return string.Empty;
            }

            if (!Directory.Exists(Env.GetAppDataPath() + @"\Gavilya\Games")) // If the directory doesn't exist
            {
                Directory.CreateDirectory(Env.GetAppDataPath() + @"\Gavilya\Games"); // Create the directory
            }

            if (!Directory.Exists(Env.GetAppDataPath() + @$"\Gavilya\games\{id}")) // If the directory doesn't exist
            {
                Directory.CreateDirectory(Env.GetAppDataPath() + $@"\Gavilya\Games\{id}"); // Create the game directory
            }
            else
            {
                if (File.Exists(Env.GetAppDataPath() + $@"\Gavilya\Games\{id}\bg_img.jpg")) // If the image exist
                {
                    return Env.GetAppDataPath() + $@"\Gavilya\Games\{id}\bg_img.jpg";
                }
                else
                {
                    WebClient webClient1 = new(); // Create a web client
                    await webClient1.DownloadFileTaskAsync(game.background_image, Env.GetAppDataPath() + @$"\Gavilya\Games\{id}\bg_img.jpg"); // Download the "background_image"
                    webClient1.Dispose(); // Release all used ressources

                    return Env.GetAppDataPath() + @$"\Gavilya\Games\{id}\bg_img.jpg"; // Return the result
                }
            }

            WebClient webClient = new(); // Create a web client
            await webClient.DownloadFileTaskAsync(game.background_image, Env.GetAppDataPath() + @$"\Gavilya\Games\{id}\bg_img.jpg"); // Download the "background_image"
            webClient.Dispose(); // Release all used ressources

            return Env.GetAppDataPath() + @$"\Gavilya\Games\{id}\bg_img.jpg"; // Return the result
        }

        /// <summary>
        /// Gets the descirption of a game from it's ID.
        /// </summary>
        /// <param name="id">The id of the game.</param>
        /// <returns></returns>
        public static async Task<string> GetGameDescriptionAsync(int id)
        {
            var client = new RestClient(); // Create a REST Client
            client.BaseUrl = new Uri($"https://api.rawg.io/api/games/{id}"); // Configure the client
            var request = new RestRequest(Method.GET); // Create a request
            var response = await client.ExecuteAsync(request); // Execute the request and store the result

            var game = JsonSerializer.Deserialize<Game>(response.Content); // Deserialize the content of the reponse

            return game.description_raw;
        }

        /// <summary>
        /// Gets the game id from it's name.
        /// </summary>
        /// <param name="gameName">The name of the game.</param>
        /// <returns></returns>
        public static async Task<int> GetGameId(string gameName)
        {
            var client = new RestClient(); // Create a REST Client
            client.BaseUrl = new Uri("https://api.rawg.io/api/games?"); // Configure the client
            var request = new RestRequest(Method.GET); // Create a request
            request.AddQueryParameter("search", gameName); // Config the request
            var response = await client.ExecuteAsync(request); // Execute the request and store the result

            var gameResults = JsonSerializer.Deserialize<GamesResults>(response.Content); // Deserialize the content of the reponse

            if (gameResults.count > 0) // If there is results
            {
                return gameResults.results[0].id; // Return the id
            }
            else
            {
                return -1; // Return -1 which means that no results has been found
            }
        }

        /// <summary>
        /// Gets the platforms of the game.
        /// </summary>
        /// <param name="id">The id of the game.</param>
        /// <returns></returns>
        public static async Task<List<Platform>> GetGamePlatformsAsync(int id)
        {
            var client = new RestClient(); // Create a REST Client
            client.BaseUrl = new Uri($"https://api.rawg.io/api/games/{id}"); // Configure the client
            var request = new RestRequest(Method.GET); // Create a request
            var response = await client.ExecuteAsync(request); // Execute the request and store the result

            var game = JsonSerializer.Deserialize<Game>(response.Content); // Deserialize the content of the reponse

            List<Platform> platforms = new(); // Create a new list of platform

            for (int i = 0; i < game.platforms.Count; i++) // For each platforms
            {
                platforms.Add(game.platforms[i].platform); // Add the platform
            }

            return platforms; // Return the result
        }

        /// <summary>
        /// Gets the game ratings from it's id.
        /// </summary>
        /// <param name="id">The id of the game.</param>
        /// <returns></returns>
        public static async Task<List<Rating>> GetGameRatingsAsync(int id)
        {
            var client = new RestClient(); // Create a REST Client
            client.BaseUrl = new Uri($"https://api.rawg.io/api/games/{id}"); // Configure the client
            var request = new RestRequest(RestSharp.Method.GET); // Create a request
            var response = await client.ExecuteAsync(request); // Execute the request and store the result

            var game = JsonSerializer.Deserialize<Game>(response.Content); // Deserialize the content of the reponse

            return game.ratings;
        }

        /// <summary>
        /// Gets the game rating from it's id.
        /// </summary>
        /// <param name="id">The id of the game.</param>
        /// <returns></returns>
        public static async Task<float> GetGameRatingAsync(int id)
        {
            var client = new RestClient(); // Create a REST Client
            client.BaseUrl = new Uri($"https://api.rawg.io/api/games/{id}"); // Configure the client
            var request = new RestRequest(RestSharp.Method.GET); // Create a request
            var response = await client.ExecuteAsync(request); // Execute the request and store the result

            var game = JsonSerializer.Deserialize<Game>(response.Content); // Deserialize the content of the reponse

            return game.rating;
        }

        /// <summary>
        /// Convert a unix time to a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="unixTime">The unix time to convert.</param>
        /// <returns></returns>
        public static DateTime UnixTimeToDateTime(double unixTime)
        {
            DateTime dtDateTime = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc); // Create a date
            dtDateTime = dtDateTime.AddSeconds(unixTime).ToLocalTime(); // Add the seconds
            return dtDateTime; // Return the result
        }

        public static string NumberToMonth(int number)
        {
            string[] months = Properties.Resources.Months.Split(new string[] { "," }, StringSplitOptions.None); // Get all the months
            switch (number)
            {
                case 1: // If January
                    return months[0]; // Return correct value
                case 2: // If Febuary
                    return months[1]; // Return correct value
                case 3: // If March
                    return months[2]; // Return correct value
                case 4: // If April
                    return months[3]; // Return correct value
                case 5: // If May
                    return months[4]; // Return correct value
                case 6: // If June
                    return months[5]; // Return correct value
                case 7: // If July
                    return months[6]; // Return correct value
                case 8: // If August
                    return months[7]; // Return correct value
                case 9: // If September
                    return months[8]; // Return correct value
                case 10: // If October
                    return months[9]; // Return correct value
                case 11: // If November
                    return months[10]; // Return correct value
                case 12: // If December
                    return months[11]; // Return correct value
                default: // If the number doesn't match
                    return "Unknown month"; // Return
            }
        }

        /// <summary>
        /// Gets if a specified process is running.
        /// </summary>
        /// <param name="processName">The name of the process to search.</param>
        /// <returns>A <see cref="bool"/> value.</returns>
        public static bool IsProcessRunning(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName); // Get the process(es) that match the name

            if (processes.Length == 0) // If the process is not running
            {
                return false; // Return false
            }
            else // If the process is running
            {
                return true; // Return true
            }
        }

        /// <summary>
        /// Reloads all the pages.
        /// </summary>
        internal static void ReloadAllPages()
        {
            Definitions.GamesCardsPages.LoadGames(); // Reload the page
            Definitions.GamesListPage.LoadGames(); // Reload the page
            Definitions.RecentGamesPage.LoadGames(); // Reload the page
        }

        /// <summary>
        /// Changes the language of Gavilya
        /// </summary>
        internal static void ChangeLanguage()
        {
            switch (Definitions.Settings.Language) // For each case
            {
                case "_default": // No language
                    break;
                case "en-US": // English (US)
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US"); // Change
                    break;

                case "fr-FR": // French (FR)
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fr-FR"); // Change
                    break;
                default: // No language
                    break;
            }
        }

        /// <summary>
        /// Removes the Welcome Screen.
        /// </summary>
        internal static void RemoveWelcomeScreen()
        {
            Definitions.GamesCardsPages.WelcomeHost.Visibility = Visibility.Collapsed; // Hide
            Definitions.GamesCardsPages.GamePresenter.Visibility = Visibility.Visible; // Visible
        }

        /// <summary>
        /// Sorts the games: A-Z.
        /// </summary>
        internal static void SortGames()
        {
            List<string> gamesNames = new(); // New list

            for (int i = 0; i < Definitions.Games.Count; i++)
            {
                gamesNames.Add(Definitions.Games[i].Name);
            }

            List<string> sortedGames = new(); // New list
            sortedGames.AddRange(gamesNames); // Create the list

            sortedGames.Sort(); // Sort the games

            List<GameInfo> sortedFinal = new(); // Create a new list
            sortedFinal.AddRange(Definitions.Games); // Assign the list

            for (int i = 0; i < sortedGames.Count; i++)
            {
                sortedFinal[i] = Definitions.Games[gamesNames.IndexOf(sortedGames[i])]; // Add the game
            }

            Definitions.Games = sortedFinal; // Save the changes
            new GameSaver().Save(Definitions.Games); // Save in file
        }
    }
}
