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
using Gavilya.Models.Rawg;
using PeyrSharp.Core.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Gavilya.ViewModels;
public class GamePageViewModel : ViewModelBase
{
	private readonly Game _game;
	private readonly GameList _games;
	private readonly List<Tag> _tags;
	private readonly MainViewModel _mainViewModel;

	private List<Platform> _platforms;
	public List<Platform> Platforms { get => _platforms; set { _platforms = value; OnPropertyChanged(nameof(Platforms)); } }

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

	private int _lastTimePlayed;
	public int LastTimePlayed
	{
		get => _lastTimePlayed;
		set
		{
			_lastTimePlayed = value;
			OnPropertyChanged(nameof(LastTimePlayed));
			if (value == 0) { LastTimeText = Properties.Resources.Never; return; }
			var date = Time.UnixTimeToDateTime(value);
			string[] months = Properties.Resources.Months.Split(new string[] { "," }, StringSplitOptions.None); // Get all the months
			LastTimeText = $"{date.Day} {months[date.Month - 1]} {date.Year}";
		}
	}

	private int _totalTimePlayed;
	public int TotalTimePlayed
	{
		get => _totalTimePlayed;
		set
		{
			_totalTimePlayed = value;
			OnPropertyChanged(nameof(TotalTimePlayed));
			TotalTimeText = TotalTimePlayed == 0 ? Properties.Resources.Never : $"{value}{Properties.Resources.HourShort}";
		}
	}

	private string _totalTimeText;
	public string TotalTimeText { get => _totalTimeText; set { _totalTimeText = value; OnPropertyChanged(nameof(TotalTimeText)); } }

	private string _lastTimeText;
	public string LastTimeText { get => _lastTimeText; set { _lastTimeText = value; OnPropertyChanged(nameof(LastTimeText)); } }

	private string _command;
	public string Command
	{
		get => _command;
		set
		{
			_command = value;
			OnPropertyChanged(nameof(Command));
		}
	}

	private bool _isFavorite;
	public bool IsFavorite
	{
		get => _isFavorite;
		set
		{
			_isFavorite = value;
			OnPropertyChanged(nameof(IsFavorite));
		}
	}

	public ICommand PlayCommand { get; }
	public ICommand EditCommand { get; }

	public GamePageViewModel(Game game, GameList games, List<Tag> tags, MainViewModel mainViewModel)
	{
		_game = game;
		_games = games;
		_tags = tags;
		_mainViewModel = mainViewModel;

		// Load properties
		Name = game.Name;
		Description = game.Description;
		CoverFilePath = game.CoverFilePath;
		LastTimePlayed = game.LastTimePlayed;
		TotalTimePlayed = game.TotalTimePlayed;
		Command = game.Command;
		IsFavorite = game.IsFavorite;

		// Commands
		EditCommand = new RelayCommand(Edit);
		if (game.RawgId != -1) LoadRawg();
	}

	private async void LoadRawg()
	{
		var rawgGame = await new RawgClient(_game.RawgId).GetGameAsync();
		Platforms = rawgGame?.Platforms.Select(p => p.Platform).ToList() ?? new();
	}

	private void Edit(object? obj)
	{
		_mainViewModel.CurrentViewModel = new GameEditionViewModel(_game, _games, _tags, _mainViewModel);
	}
}
