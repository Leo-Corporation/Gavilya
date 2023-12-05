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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Gavilya.ViewModels;

public class HomePageViewModel : ViewModelBase
{
	private readonly GameList _games;
	private readonly List<Tag> _tags;
	private readonly MainViewModel _mainViewModel;
	public string GreetingMessage => $"{Properties.Resources.Hello} {Environment.UserName}{Properties.Resources.ExclamationMark}";
	public List<MinimalGameViewModel> Favorites => _games.Where(g => g.IsFavorite && (_mainViewModel.CurrentSettings.ShowHiddenGames ? true : !g.IsHidden)).Select(g => new MinimalGameViewModel(g, _games, _mainViewModel)).ToList();
	public List<MinimalGameViewModel> Recents => _games.Where(g => _mainViewModel.CurrentSettings.ShowHiddenGames ? true : !g.IsHidden).OrderByDescending(g => g.LastTimePlayed).Take(_mainViewModel.CurrentSettings.MaxNumberRecentGamesShown).Select(g => new MinimalGameViewModel(g, _games, _mainViewModel)).ToList();
	public List<MinimalGameViewModel> Recommended => _games.GetRecommandedGames().Where(g => _mainViewModel.CurrentSettings.ShowHiddenGames ? true : !g.IsHidden).Select(g => new MinimalGameViewModel(g, _games, _mainViewModel)).ToList();

	private StatsViewModel _statView;
	public StatsViewModel StatsView { get => _statView; set { _statView = value; OnPropertyChanged(nameof(StatsView)); } }

	private Visibility _contentVis = Visibility.Visible;
	public Visibility ContentVis { get => _contentVis; set { _contentVis = value; OnPropertyChanged(nameof(ContentVis)); } }
	
	private Visibility _placeholderVis = Visibility.Collapsed;
	public Visibility PlaceholderVis { get => _placeholderVis; set { _placeholderVis = value; OnPropertyChanged(nameof(PlaceholderVis)); } }

	public ICommand RandomGameCommand { get; }

	public HomePageViewModel(GameList games, List<Tag> tags, MainViewModel mainViewModel)
	{
		_games = games;
		_tags = tags;
		_mainViewModel = mainViewModel;

		if (_games.Count == 0)
		{
			PlaceholderVis = Visibility.Visible;
			ContentVis = Visibility.Collapsed;
		}

		StatsView = new(_games, _mainViewModel.CurrentSettings.ShowHiddenGames);
		RandomGameCommand = new RelayCommand(GetRandomGame);
	}

	private void GetRandomGame(object? obj)
	{
		var game = _games.GetRandomGame();
		_mainViewModel.CurrentViewModel = new GamePageViewModel(game, _games, _tags, _mainViewModel);
	}
}
