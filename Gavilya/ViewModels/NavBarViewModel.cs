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
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Gavilya.ViewModels;
public class NavBarViewModel : ViewModelBase
{
	private readonly MainViewModel _mainViewModel;
	private readonly List<Tag> _tags;
	private bool _isPopupOpen = false;
	public bool IsPopupOpen { get => _isPopupOpen; set { _isPopupOpen = value; OnPropertyChanged(nameof(IsPopupOpen)); } }
	GameList Games { get; set; }

	private List<ClickableGameViewModel> _favVm;
	public List<ClickableGameViewModel> Favorites { get => _favVm; set { _favVm = value; OnPropertyChanged(nameof(Favorites)); } }

	public ICommand HomePageCommand { get; }
	public ICommand LibraryPageCommand { get; }
	public ICommand RecentPageCommand { get; }
	public ICommand AddCommand { get; }
	public ICommand AddWin32GameCommand { get; }
	public ICommand AddUwpGameCommand { get; }
	public ICommand AddSteamGameCommand { get; }
	public NavBarViewModel(MainViewModel mainViewModel, GameList games, List<Tag> tags)
	{
		HomePageCommand = new RelayCommand(HomePage);
		LibraryPageCommand = new RelayCommand(LibraryPage);
		RecentPageCommand = new RelayCommand(RecentPage);
		AddCommand = new RelayCommand(AddGame);
		AddWin32GameCommand = new RelayCommand(AddWin32Game);
		AddUwpGameCommand = new RelayCommand(AddUwpGame);
		AddSteamGameCommand = new RelayCommand(AddSteamGame);
		Games = games;
		_mainViewModel = mainViewModel;
		_tags = tags;
		Games.CollectionChanged += (o, e) =>
		{
			Favorites = new List<ClickableGameViewModel>(Games.Where(g => g.IsFavorite).Select(g => new ClickableGameViewModel(g, Games, _tags, _mainViewModel)));
		};
	}

	private void HomePage(object? obj)
	{
		_mainViewModel.CurrentViewModel = new HomePageViewModel();
	}

	private void LibraryPage(object? obj)
	{
		_mainViewModel.CurrentViewModel = new LibPageViewModel(Games, _tags, _mainViewModel);
	}

	private void RecentPage(object? obj)
	{
		_mainViewModel.CurrentViewModel = new RecentPageViewModel(Games, _tags, _mainViewModel);
	}

	private void AddGame(object? obj)
	{
		IsPopupOpen = true;
	}

	private void AddWin32Game(object? obj)
	{
		_mainViewModel.CurrentViewModel = new GameEditionViewModel(GameType.Win32, Games, _tags, _mainViewModel);
		IsPopupOpen = false;
	}

	private void AddUwpGame(object? obj)
	{
		_mainViewModel.CurrentViewModel = new GameEditionViewModel(GameType.UWP, Games, _tags, _mainViewModel);
		IsPopupOpen = false;
	}

	private void AddSteamGame(object? obj)
	{
		_mainViewModel.CurrentViewModel = new GameEditionViewModel(GameType.Steam, Games, _tags, _mainViewModel);
		IsPopupOpen = false;
	}
}
