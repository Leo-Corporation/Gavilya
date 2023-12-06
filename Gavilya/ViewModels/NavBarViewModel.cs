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
using PeyrSharp.Enums;
using PeyrSharp.Env;
using System.Collections.Generic;
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

	private bool _isHome;
	public bool IsHome { get => _isHome; set { _isHome = value; OnPropertyChanged(nameof(IsHome)); } }

	private bool _isLibrary;
	public bool IsLibrary { get => _isLibrary; set { _isLibrary = value; OnPropertyChanged(nameof(IsLibrary)); } }

	private bool _isRecent;
	public bool IsRecent { get => _isRecent; set { _isRecent = value; OnPropertyChanged(nameof(IsRecent)); } }

	private bool _isProfile;
	public bool IsProfile { get => _isProfile; set { _isProfile = value; OnPropertyChanged(nameof(IsProfile)); } }
    private bool _isFavorites;
    public bool IsFavorites { get => _isFavorites; set { _isFavorites = value; OnPropertyChanged(nameof(IsFavorites)); } }

    private string _profilePicture = "pack://application:,,,/Gavilya;component/Assets/DefaultPP.png";
	public string ProfilePicture { get => _profilePicture; set { _profilePicture = value; OnPropertyChanged(nameof(ProfilePicture)); } }

	private Visibility _uwpAllowed;
	public Visibility UwpAllowed { get => _uwpAllowed; set { _uwpAllowed = value; OnPropertyChanged(nameof(UwpAllowed)); } }

	public ICommand HomePageCommand { get; }
	public ICommand LibraryPageCommand { get; }
	public ICommand RecentPageCommand { get; }
	public ICommand ProfilePageCommand { get; }
	public ICommand FavoritesPageCommand { get; }
	public ICommand SettingsPageCommand { get; }
	public ICommand AddCommand { get; }
	public ICommand AddWin32GameCommand { get; }
	public ICommand AddUwpGameCommand { get; }
	public ICommand AddSteamGameCommand { get; }

	public NavBarViewModel(MainViewModel mainViewModel, Profile profile, ProfileData profiles, Page? startupPage = null)
	{
		HomePageCommand = new RelayCommand(HomePage);
		LibraryPageCommand = new RelayCommand(LibraryPage);
		RecentPageCommand = new RelayCommand(RecentPage);
		ProfilePageCommand = new RelayCommand(ProfilePage);
		FavoritesPageCommand = new RelayCommand(FavoritesPage);
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

		var defaultPage = startupPage is null ? _mainViewModel.CurrentSettings.DefaultPage : startupPage;

		IsHome = defaultPage == Page.Home;
		IsLibrary = defaultPage == Page.Library;
		IsRecent = defaultPage == Page.Recent;
		IsProfile = defaultPage == Page.Profile;
		IsFavorites = defaultPage == Page.Favorites;

		UwpAllowed = (Sys.CurrentWindowsVersion == WindowsVersion.Windows10 || Sys.CurrentWindowsVersion == WindowsVersion.Windows11) ? Visibility.Visible : Visibility.Collapsed;
		ProfilePicture = string.IsNullOrEmpty(profile.ProfilePictureFilePath) ? "pack://application:,,,/Gavilya;component/Assets/DefaultPP.png" : profile.ProfilePictureFilePath;
	}

	private void HomePage(object? obj)
	{
		_mainViewModel.CurrentViewModel = new HomePageViewModel(Games, _tags, _mainViewModel);
	}

	private void LibraryPage(object? obj)
	{
		_mainViewModel.CurrentViewModel = new LibPageViewModel(Games, _tags, _mainViewModel);
	}

    private void FavoritesPage(object? obj)
    {
        _mainViewModel.CurrentViewModel = new FavPageViewModel(Games, _tags, _mainViewModel);
    }

    private void RecentPage(object? obj)
	{
		_mainViewModel.CurrentViewModel = new RecentPageViewModel(Games, _tags, _mainViewModel);
	}

	private void ProfilePage(object? obj)
	{
		_mainViewModel.CurrentViewModel = new ProfileViewModel(_profile, _profiles, Games, _mainViewModel);
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
