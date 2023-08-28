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

using Gavilya.Commands;
using Gavilya.Models;
using PeyrSharp.Core.Converters;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Gavilya.ViewModels;

public class StatsViewModel : ViewModelBase
{
	private string _totalTime;
	private readonly GameList _games;

	private string _name;
	public string Name
	{
		get => _name;
		set
		{
			_name = value;
			OnPropertyChanged(nameof(Name));
		}
	}

	private string _description;
	public string Description
	{
		get => _description;
		set
		{
			_description = value;
			OnPropertyChanged(nameof(Description));
		}
	}

	private string _coverFilePath;
	public string CoverFilePath
	{
		get => _coverFilePath;
		set
		{
			_coverFilePath = value;
			OnPropertyChanged(nameof(CoverFilePath));
		}
	}

	private string _lastTimePlayed;
	public string LastTimePlayed
	{
		get => _lastTimePlayed;
		set
		{
			_lastTimePlayed = value;
			OnPropertyChanged(nameof(LastTimePlayed));
		}
	}

	private string _totalTimePlayed;
	public string TotalTimePlayed
	{
		get => _totalTimePlayed;
		set
		{
			_totalTimePlayed = value;
			OnPropertyChanged(nameof(TotalTimePlayed));
		}
	}

	private string _sortIcon = "\uF19C";
	public string SortIcon { get => _sortIcon; set { _sortIcon = value; OnPropertyChanged(nameof(SortIcon)); } }

	private string _sortText = Properties.Resources.MostPlayed;
	public string SortText { get => _sortText; set { _sortText = value; OnPropertyChanged(nameof(SortText)); } }

	public string TotalTime { get => _totalTime; set { _totalTime = value; OnPropertyChanged(nameof(TotalTime)); } }

	private bool _sortByMostPlayed = true;
	public bool SortByMostPlayed { get => _sortByMostPlayed; set { _sortByMostPlayed = value; OnPropertyChanged(nameof(SortByMostPlayed)); } }

	private GameList SortedGames { get; set; }

	private List<StatGameViewModel> _sortedGamesVm;
	public List<StatGameViewModel> SortedGamesVm { get => _sortedGamesVm; set { _sortedGamesVm = value; OnPropertyChanged(nameof(SortedGamesVm)); } }

	public ObservableCollection<RecInfo> Rectangles { get; set; }

	public ICommand SortCommand { get; }
	public StatsViewModel(GameList games)
	{
		_games = games;
		SortedGames = _games.SortByPlayTime(SortByMostPlayed);
		SortedGamesVm = SortedGames.Take(10).Select((g, i) => new StatGameViewModel(i, g, this)).ToList();

		int total = 0;
		for (int i = 0; i < SortedGames.Count; i++)
		{
			total += SortedGames[i].TotalTimePlayed;
		}
		TotalTime = $"{total / 3600d:0.0}{Properties.Resources.HourShort}";

		if (SortedGames.Count > 0)
		{
			Refresh(SortedGames[0]);
			int max = SortedGames[0].TotalTimePlayed;
			Rectangles = new(SortedGames.Take(10).Select(g => new RecInfo(g.TotalTimePlayed / (double)max * 110)));
		}

		SortCommand = new RelayCommand(Sort);
	}

	private void Sort(object? obj)
	{
		SortByMostPlayed = !SortByMostPlayed;
		SortIcon = SortByMostPlayed ? "\uF19C" : "\uF149";
		SortText = SortByMostPlayed ? Properties.Resources.MostPlayed : Properties.Resources.LeastPlayed;
		SortedGames = _games.SortByPlayTime(SortByMostPlayed);
		SortedGamesVm = SortedGames.Take(10).Select((g, i) => new StatGameViewModel(i, g, this)).ToList();

		if (SortedGames.Count > 0)
			Refresh(SortedGames[0]);
	}

	void Refresh(Game game)
	{
		Name = game.Name;
		Description = game.Description;
		TotalTimePlayed = $"{game.TotalTimePlayed / 3600d:0.0}{Properties.Resources.HourShort}";
		CoverFilePath = game.CoverFilePath;

		if (game.LastTimePlayed == 0)
		{
			LastTimePlayed = Properties.Resources.Never;
			return;
		}
		var date = Time.UnixTimeToDateTime(game.LastTimePlayed);
		string[] months = Properties.Resources.Months.Split(",");
		LastTimePlayed = $"{date.Day} {months[date.Month - 1]} {date.Year}";
	}
}

public class RecInfo : ViewModelBase
{
	public double RecHeight { get; }
	public RecInfo(double h)
	{
		RecHeight = h;
	}
}
