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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace Gavilya.Models;
public class GameList : ObservableCollection<Game>
{
	public string? Title { get; init; }
	public string? TagColor { get; init; }
	public GameList() : base()
	{

	}

	public GameList(IEnumerable<Game> games) : base(games)
	{
	}

	public GameList(string title) : base()
	{
		Title = title;
	}

	public GameList(string title, string tagColor) : this(title)
	{
		TagColor = tagColor;
	}

	public void Refresh() => OnCollectionChanged(new(System.Collections.Specialized.NotifyCollectionChangedAction.Reset));

	public GameList GetRange(int start, int end)
	{
		var items = this.Take(start..end);
		GameList results = new();
		foreach (var item in items)
		{
			results.Add(item);
		}
		return results;
	}

	public GameList SortByPlayTime(bool sortByMostPlayed)
	{
		List<Game> sortedGames;

		if (sortByMostPlayed)
		{
			sortedGames = this.OrderByDescending(game => game.TotalTimePlayed).ToList();
		}
		else
		{
			sortedGames = this.OrderBy(game => game.TotalTimePlayed).ToList();
		}

		return new GameList(sortedGames);
	}

	public List<GameList> GetSortedGameLists()
	{
		DateTime now = DateTime.Now;
		DateTime todayStart = now.Date;
		DateTime yesterdayStart = todayStart.AddDays(-1);
		DateTime thisMonthStart = new(now.Year, now.Month, 1);

		GameList todayList = new(Properties.Resources.Today);
		GameList yesterdayList = new(Properties.Resources.Yesterday);
		GameList thisMonthList = new(Properties.Resources.ThisMonth);
		GameList otherList = new(Properties.Resources.LongTimeAgo);

		foreach (Game game in this)
		{
			DateTime lastPlayTime = DateTimeOffset.FromUnixTimeSeconds(game.LastTimePlayed).DateTime;

			if (lastPlayTime >= todayStart)
			{
				todayList.Add(game);
			}
			else if (lastPlayTime >= yesterdayStart)
			{
				yesterdayList.Add(game);
			}
			else if (lastPlayTime >= thisMonthStart)
			{
				thisMonthList.Add(game);
			}
			else
			{
				otherList.Add(game);
			}
		}

		return new List<GameList>
		{
			todayList, yesterdayList, thisMonthList, otherList
		};
	}

	public List<GameList> GetSortedGameByTag()
	{
		Dictionary<Tag, GameList> tagGamesMap = new();
		GameList noTags = new(Properties.Resources.Other, "#dddddd");
		// Associate games with their corresponding tags
		foreach (var game in this)
		{
			if (game.Tags != null)
			{
				if (game.Tags.Count == 0) noTags.Add(game);
				foreach (var tag in game.Tags)
				{
					if (!tagGamesMap.ContainsKey(tag))
					{
						tagGamesMap[tag] = new GameList();
					}

					tagGamesMap[tag].Add(game);
				}
			}
		}

		// Create GameList instances for each tag and add associated games
		List<GameList> sortedGameLists = new();
		foreach (var kvp in tagGamesMap)
		{
			var tag = kvp.Key;
			var games = kvp.Value;

			var gameList = new GameList(tag.Name, tag.HexColorCode.Contains("#") ? tag.HexColorCode : $"#{tag.HexColorCode}");
			foreach (var game in games)
			{
				gameList.Add(game);
			}
			sortedGameLists.Add(gameList);
		}
		sortedGameLists.Add(noTags);

		return sortedGameLists;
	}

	public void Export(string filePath)
	{
		try
		{
			XmlSerializer xmlSerializer = new(GetType()); // XML Serializer
			StreamWriter streamWriter = new(filePath); // The place where the file is going to be exported
			xmlSerializer.Serialize(streamWriter, this); // Create the file
			streamWriter.Dispose();
			MessageBox.Show(Properties.Resources.ExportSuccess, Properties.Resources.MainWindowTitle, MessageBoxButton.OK, MessageBoxImage.Information); // Success
		}
		catch (Exception ex)
		{
			MessageBox.Show($"{Properties.Resources.ErrorOccurred}:\n{ex.Message}", Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error); // Error
		}
	}

	public void Import(string filePath)
	{
		try
		{
			if (File.Exists(filePath))
			{
				XmlSerializer xmlSerializer = new(GetType()); // XML Serializer
				StreamReader streamReader = new(filePath); // The path of the file

				var games = (GameList)xmlSerializer.Deserialize(streamReader) ?? new(); // Re-create each GameInfo
				foreach (Game game in games)
				{
					if (!Contains(game)) Add(game);
				}

				streamReader.Dispose();
				MessageBox.Show(Properties.Resources.ImportSuccess, Properties.Resources.MainWindowTitle, MessageBoxButton.OK, MessageBoxImage.Information); // Success
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show($"{Properties.Resources.ErrorOccurred}:\n{ex.Message}", Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error); // Error
		}
	}

	public GameList GetRecommandedGames()
	{
		try
		{
			Dictionary<int, int> gameScores = new();

			for (int i = 0; i < Count; i++)
			{
				gameScores.Add(i, this[i].LastTimePlayed / (this[i].TotalTimePlayed + 1));
			}

			var sort = from pair in gameScores orderby pair.Value descending select pair;		
			GameList recommandedGames = new();

			foreach (KeyValuePair<int, int> keyValuePair in sort)
			{
				recommandedGames.Add(this[keyValuePair.Key]);
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
}
