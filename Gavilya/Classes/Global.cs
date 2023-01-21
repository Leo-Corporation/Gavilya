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
using Microsoft.Win32;
using PeyrSharp.Env;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Shell;

namespace Gavilya.Classes;

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
		try
		{
			var client = new RestClient(new Uri("https://api.rawg.io/api/games?")); // Configure the client
			var request = new RestRequest
			{
				Method = Method.Get
			}; // Create a request
			request.AddQueryParameter("search", gameName); // Config the request
			request.AddQueryParameter("key", APIKeys.RAWGAPIKey);
			var response = await client.ExecuteAsync(request); // Execute the request and store the result

			var gameResults = JsonSerializer.Deserialize<GamesResults>(response.Content); // Deserialize the content of the reponse

			if (gameResults.results.Count > 0) // If there is results
			{
				if (gameResults.results[0].background_image == null)
				{
					return string.Empty;
				}

				int gameID = gameResults.results[0].id; // Get the firts result's id

				if (!Directory.Exists(FileSys.AppDataPath + @"\Gavilya\Games")) // If the directory doesn't exist
				{
					Directory.CreateDirectory(FileSys.AppDataPath + @"\Gavilya\Games"); // Create the directory
				}

				if (!Directory.Exists(FileSys.AppDataPath + @$"\Gavilya\games\{gameID}")) // If the directory doesn't exist
				{
					Directory.CreateDirectory(FileSys.AppDataPath + $@"\Gavilya\Games\{gameID}"); // Create the game directory
				}
				else
				{
					if (File.Exists(FileSys.AppDataPath + $@"\Gavilya\Games\{gameID}\bg_img.jpg")) // If the image exist
					{
						return FileSys.AppDataPath + $@"\Gavilya\Games\{gameID}\bg_img.jpg";
					}
					else
					{
						await DownloadFileAsync(new Uri(gameResults.results[0].background_image), FileSys.AppDataPath + $@"\Gavilya\Games\{gameID}\bg_img.jpg"); // Download the image

						return FileSys.AppDataPath + @$"\Gavilya\Games\{gameID}\bg_img.jpg"; // Return the result
					}
				}

				await DownloadFileAsync(new Uri(gameResults.results[0].background_image), FileSys.AppDataPath + $@"\Gavilya\Games\{gameID}\bg_img.jpg"); // Download the image

				return FileSys.AppDataPath + @$"\Gavilya\Games\{gameID}\bg_img.jpg"; // Return the path
			}
			else // If there isn't any results
			{
				return string.Empty; // Empty means that Gavilya will use the executable icon
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Properties.Resources.MainWindowTitle, MessageBoxButton.OK, MessageBoxImage.Error); // Error
			return string.Empty;
		}
	}

	/// <summary>
	/// Gets the game's image.
	/// </summary>
	/// <param name="id">The game's Id.</param>
	/// <returns>A <see cref="Task{string}"/> value.</returns>
	public static async Task<string> GetCoverImageAsync(int id, int screenshotId = 0)
	{
		try
		{
			string strId = "";
			var client = new RestClient(new Uri($"https://api.rawg.io/api/games/{id}")); // Configure the client
			var request = new RestRequest
			{
				Method = Method.Get
			}; // Create a request
			request.AddQueryParameter("key", APIKeys.RAWGAPIKey);
			var response = await client.ExecuteAsync(request); // Execute the request and store the result

			var game = JsonSerializer.Deserialize<Game>(response.Content); // Deserialize the content of the reponse

			if (screenshotId == 0)
			{
				if (game.background_image == null)
				{
					return string.Empty;
				}
			}
			else if (screenshotId == 1)
			{
				if (game.background_image_additional == null)
				{
					return string.Empty;
				}
				strId = "1"; // Set text
			}

			if (!Directory.Exists(FileSys.AppDataPath + @"\Gavilya\Games")) // If the directory doesn't exist
			{
				Directory.CreateDirectory(FileSys.AppDataPath + @"\Gavilya\Games"); // Create the direspctory
			}

			if (!Directory.Exists(FileSys.AppDataPath + @$"\Gavilya\games\{id}")) // If the directory doesn't exist
			{
				Directory.CreateDirectory(FileSys.AppDataPath + $@"\Gavilya\Games\{id}"); // Create the game directory
			}
			else
			{
				if (File.Exists(FileSys.AppDataPath + $@"\Gavilya\Games\{id}\bg_img{strId}.jpg")) // If the image exist
				{
					File.Delete(FileSys.AppDataPath + $@"\Gavilya\Games\{id}\bg_img{strId}.jpg");
				}
				if (screenshotId == 0)
				{
					await DownloadFileAsync(new Uri(game.background_image), FileSys.AppDataPath + $@"\Gavilya\Games\{id}\bg_img{strId}.jpg"); // Download the image
				}
				else
				{
					await DownloadFileAsync(new Uri(game.background_image_additional), FileSys.AppDataPath + $@"\Gavilya\Games\{id}\bg_img{strId}.jpg"); // Download the image
				}

				return FileSys.AppDataPath + @$"\Gavilya\Games\{id}\bg_img{strId}.jpg"; // Return the result
			}

			await DownloadFileAsync(new Uri(game.background_image), FileSys.AppDataPath + $@"\Gavilya\Games\{id}\bg_img{strId}.jpg"); // Download the image

			return FileSys.AppDataPath + @$"\Gavilya\Games\{id}\bg_img{strId}.jpg"; // Return the result
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Properties.Resources.MainWindowTitle, MessageBoxButton.OK, MessageBoxImage.Error); // Error
			return string.Empty;
		}
	}

	public static async Task<List<string>> GetCoverImageURLsAsync(int id)
	{
		try
		{
			var client = new RestClient(new Uri($"https://api.rawg.io/api/games/{id}")); // Configure the client
			var request = new RestRequest
			{
				Method = Method.Get
			}; // Create a request
			request.AddQueryParameter("key", APIKeys.RAWGAPIKey);
			var response = await client.ExecuteAsync(request); // Execute the request and store the result

			var game = JsonSerializer.Deserialize<Game>(response.Content); // Deserialize the content of the reponse

			if (game.background_image == null)
			{
				return new List<string>();
			}
			else
			{
				return new List<string>() { game.background_image, game.background_image_additional }; // Return all screenshots
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Properties.Resources.MainWindowTitle, MessageBoxButton.OK, MessageBoxImage.Error); // Error
			return new List<string>();
		}
	}

	/// <summary>
	/// Gets the descirption of a game from it's ID.
	/// </summary>
	/// <param name="id">The id of the game.</param>
	/// <returns></returns>
	public static async Task<string> GetGameDescriptionAsync(int id)
	{
		try
		{
			var client = new RestClient(new Uri($"https://api.rawg.io/api/games/{id}")); // Configure the client

			var request = new RestRequest
			{
				Method = Method.Get
			}; // Create a request
			request.AddQueryParameter("key", APIKeys.RAWGAPIKey);
			var response = await client.ExecuteAsync(request); // Execute the request and store the result

			var game = JsonSerializer.Deserialize<Game>(response.Content); // Deserialize the content of the reponse

			return game.description_raw;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Properties.Resources.MainWindowTitle, MessageBoxButton.OK, MessageBoxImage.Error); // Error
			return string.Empty;
		}
	}

	/// <summary>
	/// Gets the game id from it's name.
	/// </summary>
	/// <param name="gameName">The name of the game.</param>
	/// <returns></returns>
	public static async Task<int> GetGameId(string gameName)
	{
		try
		{
			var client = new RestClient(new Uri("https://api.rawg.io/api/games?")); // Configure the client
			var request = new RestRequest
			{
				Method = Method.Get
			}; // Create a request
			request.AddQueryParameter("search", gameName); // Config the request
			request.AddQueryParameter("key", APIKeys.RAWGAPIKey);
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
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Properties.Resources.MainWindowTitle, MessageBoxButton.OK, MessageBoxImage.Error); // Error
			return -1;
		}
	}

	/// <summary>
	/// Gets the platforms of the game.
	/// </summary>
	/// <param name="id">The id of the game.</param>
	/// <returns></returns>
	public static async Task<List<Platform>> GetGamePlatformsAsync(int id)
	{
		try
		{
			var client = new RestClient(new Uri($"https://api.rawg.io/api/games/{id}")); // Configure the client
			var request = new RestRequest
			{
				Method = Method.Get
			}; // Create a request
			request.AddQueryParameter("key", APIKeys.RAWGAPIKey);
			var response = await client.ExecuteAsync(request); // Execute the request and store the result

			var game = JsonSerializer.Deserialize<Game>(response.Content); // Deserialize the content of the reponse

			List<Platform> platforms = new(); // Create a new list of platform

			for (int i = 0; i < game.platforms.Count; i++) // For each platforms
			{
				platforms.Add(game.platforms[i].platform); // Add the platform
			}

			return platforms; // Return the result
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Properties.Resources.MainWindowTitle, MessageBoxButton.OK, MessageBoxImage.Error); // Error
			return new List<Platform>();
		}
	}

	/// <summary>
	/// Gets the game ratings from it's id.
	/// </summary>
	/// <param name="id">The id of the game.</param>
	/// <returns></returns>
	public static async Task<List<Rating>> GetGameRatingsAsync(int id)
	{
		try
		{
			var client = new RestClient(new Uri($"https://api.rawg.io/api/games/{id}")); // Configure the client
			var request = new RestRequest
			{
				Method = Method.Get
			}; // Create a request
			request.AddQueryParameter("key", APIKeys.RAWGAPIKey);
			var response = await client.ExecuteAsync(request); // Execute the request and store the result

			var game = JsonSerializer.Deserialize<Game>(response.Content); // Deserialize the content of the reponse

			return game.ratings;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Properties.Resources.MainWindowTitle, MessageBoxButton.OK, MessageBoxImage.Error); // Error
			return new List<Rating>();
		}
	}

	/// <summary>
	/// Gets the game rating from it's id.
	/// </summary>
	/// <param name="id">The id of the game.</param>
	/// <returns></returns>
	public static async Task<float> GetGameRatingAsync(int id)
	{
		try
		{
			var client = new RestClient(new Uri($"https://api.rawg.io/api/games/{id}")); // Create a REST Client
			var request = new RestRequest
			{
				Method = Method.Get
			}; // Create a request
			request.AddQueryParameter("key", APIKeys.RAWGAPIKey);
			var response = await client.ExecuteAsync(request); // Execute the request and store the result

			var game = JsonSerializer.Deserialize<Game>(response.Content); // Deserialize the content of the reponse

			return game.rating;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Properties.Resources.MainWindowTitle, MessageBoxButton.OK, MessageBoxImage.Error); // Error
			return 0f;
		}
	}

	/// <summary>
	/// Gets the RAWG.io url to the game from it's id.
	/// </summary>
	/// <param name="id">The id of the game.</param>
	/// <returns></returns>
	public static async Task<string> GetRAWGUrl(int id)
	{
		try
		{
			var client = new RestClient(new Uri($"https://api.rawg.io/api/games/{id}")); // Create a REST Client
			var request = new RestRequest
			{
				Method = Method.Get
			}; // Create a request
			request.AddQueryParameter("key", APIKeys.RAWGAPIKey);
			var response = await client.ExecuteAsync(request); // Execute the request and store the result

			var game = JsonSerializer.Deserialize<Game>(response.Content); // Deserialize the content of the reponse

			return $"https://rawg.io/games/{game.slug}";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Properties.Resources.MainWindowTitle, MessageBoxButton.OK, MessageBoxImage.Error); // Error
			return "https://rawg.io";
		}
	}

	/// <summary>
	/// Gets the game achivements from its id.
	/// </summary>
	/// <param name="id">The id of the game.</param>
	/// <returns></returns>
	public static async Task<List<Achievement>> GetAchievementsAsync(int id)
	{
		try
		{
			var client = new RestClient(new Uri($"https://api.rawg.io/api/games/{id}/achievements?")); // Create a REST Client
			var request = new RestRequest
			{
				Method = Method.Get
			}; // Create a request
			request.AddQueryParameter("key", APIKeys.RAWGAPIKey);
			request.AddQueryParameter("page_size", "20");
			var response = await client.ExecuteAsync(request); // Execute the request and store the result

			var achievementsResults = JsonSerializer.Deserialize<AchievementsResults>(response.Content); // Deserialize the content of the reponse
			return achievementsResults.results ?? new(); // Return the results
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Properties.Resources.MainWindowTitle, MessageBoxButton.OK, MessageBoxImage.Error); // Error
			return new();
		}
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
		return number switch
		{
			// If January
			1 => months[0],// Return correct value
						   // If Febuary
			2 => months[1],// Return correct value
						   // If March
			3 => months[2],// Return correct value
						   // If April
			4 => months[3],// Return correct value
						   // If May
			5 => months[4],// Return correct value
						   // If June
			6 => months[5],// Return correct value
						   // If July
			7 => months[6],// Return correct value
						   // If August
			8 => months[7],// Return correct value
						   // If September
			9 => months[8],// Return correct value
						   // If October
			10 => months[9],// Return correct value
							// If November
			11 => months[10],// Return correct value
							 // If December
			12 => months[11],// Return correct value
							 // If the number doesn't match
			_ => "Unknown month",// Return
		};
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
		Definitions.MainWindow.FavoriteSideBar.Children.Clear();
		Definitions.GamesCardsPages.LoadGames(); // Reload the page
		Definitions.GamesListPage.LoadGames(); // Reload the page
		Definitions.RecentGamesPage.LoadGames(); // Reload the page
		Definitions.TagPage.InitUI(); // Reload the page
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

			case "zh-CN": // Chinese (CN)
				Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("zh-CN"); // Change
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
	internal static void SortGames(bool alpha = true)
	{
		try
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

			if (!alpha)
			{
				sortedFinal.Reverse();
			}

			Definitions.Games = sortedFinal; // Save the changes
			GameSaver.Save(Definitions.Games); // Save in file
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Properties.Resources.MainWindowTitle, MessageBoxButton.OK, MessageBoxImage.Error); // Error
		}
	}

	/// <summary>
	/// Returns in seconds the toal time played of all games
	/// </summary>
	/// <returns></returns>
	internal static int GetTotalTimePlayed()
	{
		if (Definitions.Games.Count > 0)
		{
			int result = 0;
			for (int i = 0; i < Definitions.Games.Count; i++)
			{
				result += Definitions.Games[i].TotalTimePlayed; // Add time played
			}

			return result; // Return the total
		}
		else
		{
			return 0;
		}
	}

	/// <summary>
	/// Gets a game stores asynchronously.
	/// </summary>
	/// <param name="id">The id of the game.</param>
	/// <returns></returns>
	public static async Task<List<Store>> GetStoresAsync(int id)
	{
		try
		{
			var client = new RestClient(new Uri($"https://api.rawg.io/api/games/{id}")); // Create a REST Client
			var request = new RestRequest
			{
				Method = Method.Get
			}; // Create a request
			request.AddQueryParameter("key", APIKeys.RAWGAPIKey);
			var response = await client.ExecuteAsync(request); // Execute the request and store the result

			var game = JsonSerializer.Deserialize<Game>(response.Content); // Deserialize the content of the reponse
			List<Store> stores = new();

			game.stores.ForEach(parent => stores.Add(parent.store));
			return stores;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Properties.Resources.MainWindowTitle, MessageBoxButton.OK, MessageBoxImage.Error); // Error
			return new List<Store>();
		}
	}

	public static string UserName => Environment.UserName;

	public static void AutoSave()
	{
		try
		{
			if (!Directory.Exists(Definitions.Settings.SavePath))
			{
				Directory.CreateDirectory(Definitions.Settings.SavePath); // Create directory
			}

			bool autoSaveAlreadyDone = false; // True if the save was already done
			int dayOfSave = (Definitions.Settings.AutoSaveDay > DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)) ? DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) : Definitions.Settings.AutoSaveDay.Value;

			foreach (string file in Directory.GetFiles(Definitions.Settings.SavePath))
			{
				var dC = File.GetCreationTime(file);
				if (dC.Year == DateTime.Now.Year && dC.Month == DateTime.Now.Month && dC.Day == dayOfSave)
				{
					autoSaveAlreadyDone = true;
				}
				else
				{
					autoSaveAlreadyDone = false;
				}
			}

			if (!autoSaveAlreadyDone)
			{
				if (dayOfSave == DateTime.Now.Day)
				{
					if (Definitions.Games.Count > 0 && Directory.Exists(Definitions.Settings.SavePath))
					{
						string fL = $@"{Definitions.Settings.SavePath}\GavilyaGames_{Definitions.Profiles[Definitions.Settings.CurrentProfileIndex].Name}_{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.gav";
						GameSaver.Export(Definitions.Games, fL); // Export 
					}
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Properties.Resources.ErrorOccurred, MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}

	internal static void CreateJumpLists()
	{
		JumpList jumpList = new();
		for (int i = 0; i < Definitions.Games.Count; i++)
		{
			if (Definitions.Games[i].IsFavorite)
			{
				bool isExe = !Definitions.Games[i].IsSteam && !Definitions.Games[i].IsUWP;

				JumpTask task = new()
				{
					Title = Definitions.Games[i].Name,
					Arguments = $"{Definitions.Games[i].FileLocation}",
					Description = Definitions.Games[i].FileLocation,
					CustomCategory = Properties.Resources.Favorites,
					IconResourcePath = isExe ? Definitions.Games[i].FileLocation : Assembly.GetEntryAssembly().Location,
					ApplicationPath = "explorer.exe"
				};

				jumpList.JumpItems.Add(task);
			}
		}

		jumpList.JumpItems.Add(new JumpTask()
		{
			Title = Properties.Resources.Home,
			Arguments = "/page 0",
			Description = Properties.Resources.Home,
			CustomCategory = Properties.Resources.Tasks,
			IconResourcePath = Assembly.GetEntryAssembly().Location
		});

		jumpList.JumpItems.Add(new JumpTask()
		{
			Title = Properties.Resources.Library,
			Arguments = "/page 1",
			Description = Properties.Resources.Library,
			CustomCategory = Properties.Resources.Tasks,
			IconResourcePath = Assembly.GetEntryAssembly().Location
		});

		jumpList.JumpItems.Add(new JumpTask()
		{
			Title = Properties.Resources.MyProfile,
			Arguments = "/page 2",
			Description = Properties.Resources.MyProfile,
			CustomCategory = Properties.Resources.Tasks,
			IconResourcePath = Assembly.GetEntryAssembly().Location
		});


		jumpList.ShowFrequentCategory = false;
		jumpList.ShowRecentCategory = false;

		JumpList.SetJumpList(Application.Current, jumpList);
	}


	/// <summary>
	/// Gets the number of games that are favorites.
	/// </summary>
	/// <returns></returns>
	public static int GetFavoriteCount()
	{
		int favorites = 0;

		for (int i = 0; i < Definitions.Games.Count; i++)
		{
			if (Definitions.Games[i].IsFavorite)
			{
				favorites++;
			}
		}

		return favorites; // Return final count
	}

	/// <summary>
	/// Returns asynchronously a list of <see cref="SDK.UwpApp"/> that are installed on the user's computer.
	/// </summary>
	/// <returns>A <see cref="List{T}"/> of <see cref="SDK.UwpApp"/>.</returns>
	public static async Task<List<SDK.UwpApp>> GetUwpAppsAsync()
	{
		try
		{
			ProcessStartInfo processInfo = new()
			{
				FileName = "powershell.exe",
				Arguments = $@"& Get-StartApps | ConvertTo-Json > $env:appdata\Gavilya\UwpApps.json",
				UseShellExecute = false,
				CreateNoWindow = true
			};

			Process process = new()
			{
				StartInfo = processInfo
			};
			process.Start();
			await process.WaitForExitAsync();

			string text = await File.ReadAllTextAsync($@"{FileSys.AppDataPath}\Gavilya\UWPapps.json"); // Deserialize

			List<SDK.UwpApp> apps = JsonSerializer.Deserialize<List<SDK.UwpApp>>(text); // Get apps
			List<SDK.UwpApp> sortedApps = new(); // Create final list
			Dictionary<SDK.UwpApp, string> uwpApps = new(); // Create a dictionnary

			// Sort apps to only have UWP apps (they have a "!" in the AppID property)
			for (int i = 0; i < apps.Count; i++)
			{
				if (apps[i].AppID.Contains('!'))
				{
					uwpApps.Add(apps[i], apps[i].Name);
				}
			}

			// Sort alphabetically
			var sorted = from pair in uwpApps orderby pair.Value ascending select pair; // Sort

			foreach (KeyValuePair<SDK.UwpApp, string> pair1 in sorted)
			{
				sortedApps.Add(pair1.Key);
			}

			return sortedApps; // Return
		}
		catch (Exception ex)
		{
			return new() { new(ex.Message, ex.StackTrace) };
		}
	}

	public static List<GameInfo> GetRecommandedGames()
	{
		try
		{
			Dictionary<int, int> gameScores = new();

			for (int i = 0; i < Definitions.Games.Count; i++)
			{
				gameScores.Add(i, Definitions.Games[i].LastTimePlayed / Definitions.Games[i].TotalTimePlayed);
			}

			var sort = Definitions.Settings.ShowMoreUnplayedGamesRecommanded.Value
				? from pair in gameScores orderby pair.Value descending select pair
				: from pair in gameScores orderby pair.Value ascending select pair; // Sort
			List<GameInfo> recommandedGames = new();

			foreach (KeyValuePair<int, int> keyValuePair in sort)
			{
				recommandedGames.Add(Definitions.Games[keyValuePair.Key]);
			}

			return recommandedGames.Count > 5
				? recommandedGames.GetRange(0, 5)
				: recommandedGames;
		}
		catch (DivideByZeroException)
		{
			return new();
		}
	}

	public static bool CanLaunchSteamGame()
	{
		try
		{
			RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Valve\Steam\ActiveProcess");
			string result = key.GetValue("ActiveUser").ToString();
			if (result == "0")
			{
				MessageBox.Show(Properties.Resources.NotLoggedToSteam, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error); // If the user isn't logged to steam
				return false; // Return false, Steam cannot launch game.
			}

			// Check if Steam is not running
			if (!IsProcessRunning("Steam"))
			{
				// Show a message box
				MessageBox.Show(Properties.Resources.SteamNotRunning, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
				return false; // Return false, Steam cannot launch game.
			}
		}
		catch
		{
			return false;
		}

		return true; // Return true, Steam can launch game.
	}

	public static async Task DownloadFileAsync(Uri uri, string filePath)
	{
		using var s = await new System.Net.Http.HttpClient().GetStreamAsync(uri);
		using var fs = new FileStream(filePath, FileMode.CreateNew);
		await s.CopyToAsync(fs);
	}
}
