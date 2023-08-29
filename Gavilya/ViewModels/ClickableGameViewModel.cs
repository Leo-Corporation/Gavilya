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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Gavilya.ViewModels;

public class ClickableGameViewModel : ViewModelBase
{
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
	private ImageSource _gameImage;
	public ImageSource GameImage { get => _gameImage; set { _gameImage = value; OnPropertyChanged(nameof(GameImage)); } }

	public string Name { get; }

	public ICommand ClickCommand { get; }
	private readonly Game _game;
	private readonly GameList _games;
	private readonly List<Tag> _tags;
	private readonly MainViewModel _mainViewModel;

	public ClickableGameViewModel(Game game, GameList games, List<Tag> tags, MainViewModel mainViewModel)
	{
		CoverFilePath = game.CoverFilePath ?? "";
		Name = game.Name;
		ClickCommand = new RelayCommand(Click);
		_game = game;
		_games = games;
		_tags = tags;
		_mainViewModel = mainViewModel;
	}

	private void Click(object? obj)
	{
		_mainViewModel.CurrentViewModel = new GamePageViewModel(_game, _games, _tags, _mainViewModel);
		_mainViewModel.SearchOpen = false;
	}

	public override string ToString()
	{
		return Name;
	}
}
