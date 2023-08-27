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
	private Profile _profile;
	private readonly ProfileData _profiles;
	private readonly List<Tag> _tags;
	private bool _isPopupOpen = false;
	public bool IsPopupOpen { get => _isPopupOpen; set { _isPopupOpen = value; OnPropertyChanged(nameof(IsPopupOpen)); } }
	GameList Games { get; set; }

	private List<ClickableGameViewModel> _favVm;
	public List<ClickableGameViewModel> Favorites { get => _favVm; set { _favVm = value; OnPropertyChanged(nameof(Favorites)); } }
	
	private bool _isHome;
	public bool IsHome { get => _isHome; set { _isHome = value; OnPropertyChanged(nameof(IsHome)); } }

	private bool _isLibrary;
	public bool IsLibrary { get => _isLibrary; set { _isLibrary = value; OnPropertyChanged(nameof(IsLibrary)); } }

	private bool _isRecent;
	public bool IsRecent { get => _isRecent; set { _isRecent = value; OnPropertyChanged(nameof(IsRecent)); } }

	private bool _isProfile;
	public bool IsProfile { get => _isProfile; set { _isProfile = value; OnPropertyChanged(nameof(IsProfile)); } }

	public ICommand HomePageCommand { get; }
	public ICommand LibraryPageCommand { get; }
	public ICommand RecentPageCommand { get; }
	public ICommand SettingsPageCommand { get; }
	public ICommand AddCommand { get; }
	public ICommand AddWin32GameCommand { get; }
	public ICommand AddUwpGameCommand { get; }
	public ICommand AddSteamGameCommand { get; }
	public NavBarViewModel(MainViewModel mainViewModel, Profile profile, ProfileData profiles)
	{
		HomePageCommand = new RelayCommand(HomePage);
		LibraryPageCommand = new RelayCommand(LibraryPage);
		RecentPageCommand = new RelayCommand(RecentPage);
		AddCommand = new RelayCommand(AddGame);
		AddWin32GameCommand = new RelayCommand(AddWin32Game);
		AddUwpGameCommand = new RelayCommand(AddUwpGame);
		AddSteamGameCommand = new RelayCommand(AddSteamGame);
		SettingsPageCommand = new RelayCommand(SettingsPage);

		Games = profile.Games;
		_mainViewModel = mainViewModel;
		_profile = profile;
		_profiles = profiles;
		_tags = profile.Tags;

		IsHome = _mainViewModel.CurrentSettings.DefaultPage == Page.Home;
		IsLibrary = _mainViewModel.CurrentSettings.DefaultPage == Page.Library;
		IsRecent = _mainViewModel.CurrentSettings.DefaultPage == Page.Recent;
		IsProfile = _mainViewModel.CurrentSettings.DefaultPage == Page.Profile;
		Favorites = new List<ClickableGameViewModel>(Games.Where(g => g.IsFavorite && (_mainViewModel.CurrentSettings.ShowHiddenGames ? true : !g.IsHidden)).Select(g => new ClickableGameViewModel(g, Games, _tags, _mainViewModel)));

		Games.CollectionChanged += (o, e) =>
		{
			Favorites = new List<ClickableGameViewModel>(Games.Where(g => g.IsFavorite && (_mainViewModel.CurrentSettings.ShowHiddenGames ? true : !g.IsHidden)).Select(g => new ClickableGameViewModel(g, Games, _tags, _mainViewModel)));
		};
	}

	private void HomePage(object? obj)
	{
		_mainViewModel.CurrentViewModel = new HomePageViewModel(Games, _mainViewModel);
	}

	private void LibraryPage(object? obj)
	{
		_mainViewModel.CurrentViewModel = new LibPageViewModel(Games, _tags, _mainViewModel);
	}

	private void RecentPage(object? obj)
	{
		_mainViewModel.CurrentViewModel = new RecentPageViewModel(Games, _tags, _mainViewModel);
	}

	private void SettingsPage(object? obj)
	{
		_mainViewModel.CurrentViewModel = new SettingsPageViewModel(_profile, _profiles, Games, _mainViewModel);
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
