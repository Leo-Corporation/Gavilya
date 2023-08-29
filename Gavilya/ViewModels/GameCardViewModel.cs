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
using Gavilya.Enums;
using Gavilya.Models;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Gavilya.ViewModels;

public class GameCardViewModel : ViewModelBase
{
	readonly MainViewModel _mainViewModel;
	private string _name;
	private readonly Game _game;
	private readonly GameList _games;

	public string Name
	{
		get => _name;
		set
		{
			_name = value;
			PlayText = string.Format(Properties.Resources.PlayTo, Name);
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
			if (!string.IsNullOrEmpty(value))
			{
				BitmapImage bitmapImage = new();
				bitmapImage.BeginInit();
				bitmapImage.UriSource = new(value);
				bitmapImage.DecodePixelWidth = 256;
				bitmapImage.DecodePixelHeight = 144;
				bitmapImage.EndInit();
				GameImage = bitmapImage; 
			}
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
		}
	}

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

	private string _favIcon;
	public string FavIcon
	{
		get => _favIcon;
		set
		{
			_favIcon = value;
			OnPropertyChanged(nameof(FavIcon));
		}
	}

	private GameType _gameType;
	public GameType GameType
	{
		get => _gameType;
		set
		{
			_gameType = value;
			OnPropertyChanged(nameof(GameType));
		}
	}

	private bool _isFavorite;
	public bool IsFavorite
	{
		get => _isFavorite;
		set
		{
			_isFavorite = value;
			FavIcon = value ? "\uF71B" : "\uF710";
			OnPropertyChanged(nameof(IsFavorite));
		}
	}

	private bool _isHidden;
	public bool IsHidden
	{
		get => _isHidden;
		set
		{
			_isHidden = value;
			OnPropertyChanged(nameof(IsHidden));
		}
	}

	private bool _isSelected = false;
	public bool IsSelected { get => _isSelected; set { _isSelected = value; OnPropertyChanged(nameof(IsSelected)); } }

	private Visibility _mouseHoverVis = Visibility.Hidden;
	private readonly List<Tag> _tags;

	public Visibility MouseHoverVis { get => _mouseHoverVis; set { _mouseHoverVis = value; OnPropertyChanged(nameof(MouseHoverVis)); } }

	private ImageSource _gameImage;
	public ImageSource GameImage { get => _gameImage; set { _gameImage = value; OnPropertyChanged(nameof(GameImage)); } }

	public string PlayText { get; set; }

	public ICommand MouseHoverCommand { get; }

	public ICommand PlayCommand { get; }
	public ICommand EditCommand { get; }
	public ICommand ClickCommand { get; }
	public ICommand FavCommand { get; }
	public ICommand CheckCommand { get; }

	public GameCardViewModel(Game game, GameList games, List<Tag> tags, MainViewModel mainViewModel)
	{
		_game = game;
		_games = games;
		_tags = tags;
		_mainViewModel = mainViewModel;

		Name = game.Name;
		Description = game.Description;
		CoverFilePath = game.CoverFilePath;
		LastTimePlayed = game.LastTimePlayed;
		TotalTimePlayed = game.TotalTimePlayed;
		Command = game.Command;
		GameType = game.GameType;
		IsFavorite = game.IsFavorite;
		IsHidden = game.IsHidden;

		MouseHoverCommand = new RelayCommand(HandleMouseHover);
		EditCommand = new RelayCommand(Edit);
		ClickCommand = new RelayCommand(Click);
		FavCommand = new RelayCommand(Fav);
		CheckCommand = new RelayCommand(Check);
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

	private void Edit(object? obj)
	{
		_mainViewModel.CurrentViewModel = new GameEditionViewModel(_game, _mainViewModel.Games, _tags, _mainViewModel);
	}

	private void Click(object? obj)
	{
		_mainViewModel.CurrentViewModel = new GamePageViewModel(_game, _mainViewModel.Games, _tags, _mainViewModel);
	}

	private void Fav(object? obj)
	{
		IsFavorite = !IsFavorite;
		_game.IsFavorite = IsFavorite;
		_games[_games.IndexOf(_game)] = _game;
	}

	private void Check(object? obj)
	{
		if (_mainViewModel.ItemsToRemove.Contains(_game))
		{
			_mainViewModel.ItemsToRemove.Remove(_game);

			return;
		}
		_mainViewModel.ItemsToRemove.Add(_game);
	}
}
