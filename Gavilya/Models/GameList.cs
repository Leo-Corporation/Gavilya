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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
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
		GameList results = [.. items];

		return results;
	}

	public GameList SortByPlayTime(bool sortByMostPlayed, bool showHiddenGames)
	{
		List<Game> sortedGames;

		if (sortByMostPlayed)
		{
			sortedGames = this.Where(game => showHiddenGames || !game.IsHidden).OrderByDescending(game => game.TotalTimePlayed).ToList();
		}
		else
		{
			sortedGames = this.Where(game => showHiddenGames || !game.IsHidden).OrderBy(game => game.TotalTimePlayed).ToList();
		}

		return new GameList(sortedGames);
	}

	public List<GameList> GetSortedGameLists()
	{
		DateTime now = DateTime.Now;
		DateTime todayStart = now.Date;
		DateTime yesterdayStart = todayStart.AddDays(-1);
		DateTime thisWeekStart = now.AddDays((int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek - (int)now.DayOfWeek);
		DateTime thisMonthStart = new(now.Year, now.Month, 1);

		GameList todayList = new(Properties.Resources.Today);
		GameList yesterdayList = new(Properties.Resources.Yesterday);
		GameList thisWeekList = new(Properties.Resources.ThisWeek);
		GameList thisMonthList = new(Properties.Resources.ThisMonth);

		// Dictionary to hold games for specific dates outside of today, yesterday, this week, and this month.
		Dictionary<DateTime, GameList> otherDateLists = [];


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
			else if (lastPlayTime >= thisWeekStart)
			{
				thisWeekList.Add(game);
			}
			else if (lastPlayTime >= thisMonthStart)
			{
				thisMonthList.Add(game);
			}
			else
			{
				// Create a key based on the date part only (ignoring time).
				DateTime otherDate = lastPlayTime.Date;

				if (!otherDateLists.ContainsKey(otherDate))
				{
					// Create a new list for the specific date.
					otherDateLists[otherDate] = new GameList(otherDate.ToString("D"));
				}
				otherDateLists[otherDate].Add(game);
			}
		}
		var sorted = otherDateLists.OrderByDescending(x => x.Key);

		List<GameList> sortedGames = [];

		if (todayList.Count > 0) sortedGames.Add(todayList);
		if (yesterdayList.Count > 0) sortedGames.Add(yesterdayList);
		if (thisWeekList.Count > 0) sortedGames.Add(thisWeekList);
		if (thisMonthList.Count > 0) sortedGames.Add(thisMonthList);
		// Add the dynamically created date-specific lists in descending order.
		foreach (var dateList in sorted)
		{
			sortedGames.Add(dateList.Value);
		}

		return sortedGames;
	}

	public List<GameList> GetSortedGameByTag()
	{
		Dictionary<Tag, GameList> tagGamesMap = [];
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
						tagGamesMap[tag] = [];
					}

					tagGamesMap[tag].Add(game);
				}
			}
		}

		// Create GameList instances for each tag and add associated games
		List<GameList> sortedGameLists = [];

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

				var games = (GameList)xmlSerializer.Deserialize(streamReader) ?? []; // Re-create each GameInfo

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
			Dictionary<int, int> gameScores = [];

			for (int i = 0; i < Count; i++)
			{
				gameScores.Add(i, this[i].LastTimePlayed / (this[i].TotalTimePlayed + 1));
			}

			var sort = from pair in gameScores orderby pair.Value descending select pair;
			GameList recommandedGames = [];

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
			return [];
		}
	}

	public Game GetRandomGame()
	{
		Random random = new();
		int i = random.Next(0, Count);
		return this[i];
	}
}
