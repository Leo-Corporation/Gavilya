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
using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;

namespace Gavilya.Models;
public class GameList : ObservableCollection<Game>
{
	public string? Title { get; init; }
	public string? TagColor { get; init; }
    public GameList() : base()
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
}
