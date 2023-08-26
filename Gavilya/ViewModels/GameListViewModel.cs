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
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace Gavilya.ViewModels;

public class GameListViewModel : ViewModelBase
{
	private readonly Game _game;
	private readonly GameList _games;
	private readonly ListPageViewModel _listPageViewModel;
	private readonly MainViewModel _mainViewModel;
	private string _name;
	public string Name { get => _name; set { _name = value; OnPropertyChanged(nameof(Name)); } }


	private bool _isFavorite;
	public bool IsFavorite { get => _isFavorite; set { _isFavorite = value; OnPropertyChanged(nameof(IsFavorite)); } }

	private Visibility _mouseHoverVis = Visibility.Hidden;
	private readonly List<Tag> _tags;

	public Visibility MouseHoverVis { get => _mouseHoverVis; set { _mouseHoverVis = value; OnPropertyChanged(nameof(MouseHoverVis)); } }

	public ICommand MouseHoverCommand { get; }
	public ICommand ClickCommand { get; }
	public ICommand PlayCommand { get; }


	public GameListViewModel(Game game, GameList games, List<Tag> tags, ListPageViewModel listPageViewModel, MainViewModel mainViewModel)
	{
		_game = game;
		_games = games;
		_tags = tags;
		_listPageViewModel = listPageViewModel;
		_mainViewModel = mainViewModel;
		Name = _game.Name;
		IsFavorite = _game.IsFavorite;

		// Commands
		MouseHoverCommand = new RelayCommand(HandleMouseHover);
		ClickCommand = new RelayCommand(Click);
		PlayCommand = new RelayCommand(Play);
	}

	private void Play(object? obj)
	{
		_mainViewModel.GameLauncherHelper = new(_game, _games);
		_mainViewModel.GameLauncherHelper.Launch();
	}

	private void HandleMouseHover(object parameter)
	{
		MouseHoverVis = MouseHoverVis == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
	}

	private void Click(object? obj)
	{
		_listPageViewModel.CurrentGameView = new GamePageViewModel(_game, _games, _tags, _mainViewModel);
	}
}
