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
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
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
            Uri icon = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"\Gavilya.ico"); // Define the path to the icon
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
                    return Env.GetAppDataPath() + $@"\Gavilya\Games\{gameID}\bg_img.jpg";
                }


                WebClient webClient = new WebClient(); // Create a WebClient
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
                return Env.GetAppDataPath() + $@"\Gavilya\Games\{id}\bg_img.jpg";
            }

            WebClient webClient = new WebClient(); // Create a web client
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

            return game.description;
        }
    }
}
