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

namespace Gavilya.ViewModels.Settings;

public class StartupViewModel : ViewModelBase
{
	private readonly Profile _profile;
	private readonly ProfileData _profileData;
	private readonly MainViewModel _mainViewModel;

	private bool _isCardSelected;
	public bool IsCardSelected { get => _isCardSelected; set { _isCardSelected = value; if (value) _view = View.Card; OnPropertyChanged(nameof(IsCardSelected)); } }

	private bool _isTagSelected;
	public bool IsTagSelected { get => _isTagSelected; set { _isTagSelected = value; if (value) _view = View.Tag; OnPropertyChanged(nameof(IsTagSelected)); } }

	private bool _isListSelected;
	public bool IsListSelected { get => _isListSelected; set { _isListSelected = value; if (value) _view = View.List; OnPropertyChanged(nameof(IsListSelected)); } }

	private bool _isHome;
	public bool IsHome { get => _isHome; set { _isHome = value; if (value) _page = Page.Home; OnPropertyChanged(nameof(IsHome)); } }

	private bool _isLibrary;
	public bool IsLibrary { get => _isLibrary; set { _isLibrary = value; if (value) _page = Page.Library; OnPropertyChanged(nameof(IsLibrary)); } }

    private bool _isFavorite;
    public bool IsFavorite { get => _isFavorite; set { _isFavorite = value; if (value) _page = Page.Favorites; OnPropertyChanged(nameof(IsFavorite)); } }

    private bool _isRecent;
	public bool IsRecent { get => _isRecent; set { _isRecent = value; if (value) _page = Page.Recent; OnPropertyChanged(nameof(IsRecent)); } }

	private bool _isProfile;
	public bool IsProfile { get => _isProfile; set { _isProfile = value; if (value) _page = Page.Profile; OnPropertyChanged(nameof(IsProfile)); } }

	private View _view;
	private Page _page;

	public ICommand ViewCommand { get; }
	public ICommand PageCommand { get; }

	public StartupViewModel(Profile profile, ProfileData profileData, MainViewModel mainViewModel)
	{
		_profile = profile;
		_profileData = profileData;
		_mainViewModel = mainViewModel;

		IsCardSelected = profile.Settings.DefaultView == View.Card;
		IsTagSelected = profile.Settings.DefaultView == View.Tag;
		IsListSelected = profile.Settings.DefaultView == View.List;

		IsHome = profile.Settings.DefaultPage == Page.Home;
		IsLibrary = profile.Settings.DefaultPage == Page.Library;
		IsFavorite = profile.Settings.DefaultPage == Page.Favorites;
		IsRecent = profile.Settings.DefaultPage == Page.Recent;
		IsProfile = profile.Settings.DefaultPage == Page.Profile;

		ViewCommand = new RelayCommand(SetView);
		PageCommand = new RelayCommand(SetPage);
	}

	private void SetView(object? obj)
	{
		_profileData.Profiles[_profileData.Profiles.IndexOf(_profile)].Settings.DefaultView = _view;
		_mainViewModel.CurrentSettings = _profileData.Profiles[_profileData.Profiles.IndexOf(_profile)].Settings;
		_profileData.Save();
	}

	private void SetPage(object? obj)
	{
		_profileData.Profiles[_profileData.Profiles.IndexOf(_profile)].Settings.DefaultPage = _page;
		_mainViewModel.CurrentSettings = _profileData.Profiles[_profileData.Profiles.IndexOf(_profile)].Settings;
		_profileData.Save();
	}
}
