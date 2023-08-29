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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Gavilya.ViewModels;
public class MinimalGameViewModel : ViewModelBase
{
	private Game _game;
	private GameList _games;
	private readonly MainViewModel _mainViewModel;

	private string _name;
	public string Name { get => _name; set { _name = value; PlayText = string.Format(Properties.Resources.PlayTo, Name); OnPropertyChanged(nameof(Name)); } }

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
				bitmapImage.DecodePixelWidth = 150;
				bitmapImage.DecodePixelHeight = 85;
				bitmapImage.EndInit();
				GameImage = bitmapImage;
			}
			OnPropertyChanged(nameof(CoverFilePath));
		}
	}

	private ImageSource _gameImage;
	public ImageSource GameImage { get => _gameImage; set { _gameImage = value; OnPropertyChanged(nameof(GameImage)); } }

	public string PlayText { get; set; }

	public ICommand PlayCommand { get; }

	public MinimalGameViewModel(Game game, GameList games, MainViewModel mainViewModel)
	{
		// Fields
		_game = game;
		_games = games;
		_mainViewModel = mainViewModel;

		// Properties
		Name = _game.Name;
		CoverFilePath = _game.CoverFilePath;

		// Commands
		PlayCommand = new RelayCommand(Play);
	}

	private void Play(object? obj)
	{
		_mainViewModel.GameLauncherHelper = new(_game, _games);
		_mainViewModel.GameLauncherHelper.Launch();
	}
}
