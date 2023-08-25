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

using RestSharp;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace Gavilya.Models.Rawg;

public class RawgClient
{
	private readonly string _gameName;
	private int _gameId;

	public RawgClient(string name)
	{
		_gameName = name;
	}

	public RawgClient(int id)
	{
		_gameId = id;
	}

	public static async Task<RawgClient> CreateAsync(string name)
	{
		var instance = new RawgClient(name);
		await instance.InitializeAsync();
		return instance;
	}

	private async Task InitializeAsync()
	{
		_gameId = await GetId();
	}

	public async Task<int> GetId()
	{
		try
		{
			var client = new RestClient(new Uri("https://api.rawg.io/api/games?")); // Configure the client
			var request = new RestRequest
			{
				Method = Method.Get
			}; // Create a request
			request.AddQueryParameter("search", _gameName); // Config the request
			request.AddQueryParameter("key", ApiKeys.RawgApiKey);
			var response = await client.ExecuteAsync(request); // Execute the request and store the result

			var gameResults = JsonSerializer.Deserialize<GamesResults>(response.Content); // Deserialize the content of the reponse

			if (gameResults is not null && gameResults.Count > 0) // If there is results
			{
				return gameResults.Results[0].Id; // Return the id
			}
			else
			{
				return -1; // Return -1 which means that no results has been found
			}
		}
		catch
		{
			return -1;
		}
	}

	public async Task<RawgGame?> GetGameAsync()
	{
		try
		{
			var client = new RestClient(new Uri($"https://api.rawg.io/api/games/{_gameId}")); // Configure the client
			var request = new RestRequest
			{
				Method = Method.Get
			}; // Create a request
			request.AddQueryParameter("key", ApiKeys.RawgApiKey);
			var response = await client.ExecuteAsync(request); // Execute the request and store the result

			var game = JsonSerializer.Deserialize<RawgGame>(response.Content); // Deserialize the content of the reponse

			return game;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Properties.Resources.MainWindowTitle, MessageBoxButton.OK, MessageBoxImage.Error); // Error
			return null;
		}
	}

	public async Task<List<Achievement>> GetAchievementsAsync()
	{
		try
		{
			var client = new RestClient(new Uri($"https://api.rawg.io/api/games/{_gameId}/achievements?")); // Create a REST Client
			var request = new RestRequest
			{
				Method = Method.Get
			}; // Create a request
			request.AddQueryParameter("key", ApiKeys.RawgApiKey);
			request.AddQueryParameter("page_size", "20");
			var response = await client.ExecuteAsync(request); // Execute the request and store the result

			var achievementsResults = JsonSerializer.Deserialize<AchievementsResults>(response.Content); // Deserialize the content of the reponse
			return achievementsResults?.Results ?? new(); // Return the results
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Properties.Resources.MainWindowTitle, MessageBoxButton.OK, MessageBoxImage.Error); // Error
			return new();
		}
	}

	public async Task<GamesResults?> GetResultsAsync()
	{
		try
		{
			var client = new RestClient(new Uri("https://api.rawg.io/api/games?")); // Configure the client
			var request = new RestRequest
			{
				Method = Method.Get
			}; // Create a request
			request.AddQueryParameter("search", _gameName); // Config the request
			request.AddQueryParameter("key", ApiKeys.RawgApiKey);
			var response = await client.ExecuteAsync(request); // Execute the request and store the result

			var gameResults = JsonSerializer.Deserialize<GamesResults>(response.Content); // Deserialize the content of the reponse

			if (gameResults is not null && gameResults.Count > 0) // If there is results
			{
				return gameResults; // Return the id
			}
			else
			{
				return null; // Return -1 which means that no results has been found
			}
		}
		catch
		{
			return null;
		}
	}
}
