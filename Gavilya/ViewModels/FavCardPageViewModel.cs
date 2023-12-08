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
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Gavilya.ViewModels;

class FavCardPageViewModel : ViewModelBase
{
	public GameList Games { get; set; }

	private readonly List<Tag> _tags;
	readonly MainViewModel _mainViewModel;
	public List<GameCardViewModel> GamesVm => Games.Where(g => _mainViewModel.CurrentSettings.ShowHiddenGames ? true : !g.IsHidden).Select(g => new GameCardViewModel(g, Games, _tags, _mainViewModel)).ToList();

	private Visibility _placeholderVis;
	public Visibility PlaceholderVis { get => _placeholderVis; set { _placeholderVis = value; OnPropertyChanged(nameof(PlaceholderVis)); } }

	public ICommand AddCommand { get; }

	public FavCardPageViewModel(GameList games, List<Tag> tags, MainViewModel mainViewModel)
	{
		Games = games;
		_mainViewModel = mainViewModel;
		_tags = tags;

		PlaceholderVis = Games.Count > 0 ? Visibility.Collapsed : Visibility.Visible;
		AddCommand = new RelayCommand((o) => _mainViewModel.CurrentViewModel = new GameEditionViewModel(Enums.GameType.Win32, games, _tags, _mainViewModel));
	}
}
