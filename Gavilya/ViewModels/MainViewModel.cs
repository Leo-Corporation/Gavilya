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
using Gavilya.Helpers;
using Gavilya.Models;
using PeyrSharp.Core;
using PeyrSharp.Env;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Gavilya.ViewModels;
public class MainViewModel : ViewModelBase
{
	private NavBarViewModel _navBarViewModel;
	public NavBarViewModel Nav { get => _navBarViewModel; set { _navBarViewModel = value; OnPropertyChanged(); } }

	private object _currentView;
	private readonly Window _window;
	private readonly Profile _profile;
	private readonly ProfileData _profiles;
	private readonly List<Tag> _tags;
	private readonly WindowHelper _windowHelper;
	private List<ClickableGameViewModel> _searchResults;
	internal GameList ItemsToRemove { get; set; } = new();

	private Models.Settings _currentSettings;
	public Models.Settings CurrentSettings { get => _currentSettings; set { _currentSettings = value; LoadSettings(value); } }
	public GameLauncherHelper GameLauncherHelper { get; set; }
	public List<ClickableGameViewModel> SearchResults { get => _searchResults; set { _searchResults = value; OnPropertyChanged(nameof(SearchResults)); } }
	public GameList Games { get; set; }

	public object CurrentViewModel
	{
		get { return _currentView; }
		set { _currentView = value; OnPropertyChanged(nameof(CurrentViewModel)); }
	}

	private string _maxiIcon = "\uFA40";
	public string MaxiIcon
	{
		get => _maxiIcon;
		set { _maxiIcon = value; OnPropertyChanged(nameof(MaxiIcon)); }
	}

	private string _query = "";
	public string Query
	{
		get => _query;
		set
		{
			_query = value;
			SearchResults = Games.Where(g => g.Name.ToLower().Contains(Query.ToLower())).Select(g => new ClickableGameViewModel(g, Games, _tags, this)).ToList(); OnPropertyChanged(nameof(Query));
			NoResults = SearchResults.Count > 0 ? Visibility.Collapsed : Visibility.Visible;
		}
	}

	private double _maxiIconFontSize = 12;
	public double MaxiIconFontSize
	{
		get => _maxiIconFontSize;
		set { _maxiIconFontSize = value; OnPropertyChanged(nameof(MaxiIconFontSize)); }
	}

	private double _maxHeight;
	private double _maxWidth;

	public double MaxHeight { get => _maxHeight; set { _maxHeight = value; OnPropertyChanged(nameof(MaxHeight)); } }
	public double MaxWidth { get => _maxWidth; set { _maxWidth = value; OnPropertyChanged(nameof(MaxWidth)); } }

	private double _searchHeight;
	public double SearchHeight { get => _searchHeight; set { _searchHeight = value; OnPropertyChanged(nameof(SearchHeight)); } }

	private int _sideBarPosition;
	public int SideBarPosition { get => _sideBarPosition; set { _sideBarPosition = value; OnPropertyChanged(nameof(SideBarPosition)); } }

	private Thickness _borderMargin = new(10);
	public Thickness BorderMargin { get => _borderMargin; set { _borderMargin = value; OnPropertyChanged(nameof(BorderMargin)); } }

	private bool _searchOpen;
	public bool SearchOpen { get => _searchOpen; set { _searchOpen = value; OnPropertyChanged(nameof(SearchOpen)); } }

	private Visibility _noResults;
	public Visibility NoResults { get => _noResults; set { _noResults = value; OnPropertyChanged(nameof(NoResults)); } }

	public ICommand MinimizeCommand { get; }
	public ICommand MaximizeRestoreCommand { get; }
	public ICommand CloseCommand { get; }
	public ICommand SearchClickCommand { get; }
	public ICommand DeleteCommand { get; }

	public MainViewModel(Window window, Profile profile, ProfileData profiles)
	{
		// Properties
		Games = profile.Games;
		CurrentSettings = profile.Settings;
		CurrentViewModel = profile.Settings.DefaultPage switch
		{
			Page.Library => new LibPageViewModel(Games, profile.Tags, this),
			Page.Recent => new RecentPageViewModel(Games, profile.Tags, this),
			Page.Profile => new ProfileViewModel(profile, profiles, Games, this),
			_ => new HomePageViewModel(Games, this)
		};
		Query = "";

		// Fields
		_window = window;
		_profile = profile;
		_profiles = profiles;
		_tags = profile.Tags;
		_navBarViewModel = new(this, profile, profiles);
		_windowHelper = new(window);

		// Commands
		MinimizeCommand = new RelayCommand(Minimize);
		MaximizeRestoreCommand = new RelayCommand(Maximize);
		CloseCommand = new RelayCommand(Close);
		DeleteCommand = new RelayCommand(DeleteGames);
		SearchClickCommand = new RelayCommand((o) =>
		{
			SearchOpen = !SearchOpen;
			SearchHeight = CurrentSettings.NumberOfSearchResultsToDisplay * 45;
		});

		// Window System
		(MaxHeight, MaxWidth) = _windowHelper.GetMaximumSize();

		// Events
		_window.StateChanged += (o, e) =>
		{
			(MaxHeight, MaxWidth) = _windowHelper.GetMaximumSize();
			MaxiIcon = _window.WindowState == WindowState.Maximized ? "\uF670" : "\uFA40";
			MaxiIconFontSize = _window.WindowState == WindowState.Maximized ? 16 : 12;
			BorderMargin = _window.WindowState == WindowState.Maximized ? new(0) : new(10); // Set
		};

		_window.LocationChanged += (o, e) =>
		{
			(MaxHeight, MaxWidth) = _windowHelper.GetMaximumSize();
		};

		Games.CollectionChanged += (o, e) =>
		{
			SearchResults = Games.Where(g => g.Name.Contains(Query)).Select(g => new ClickableGameViewModel(g, Games, _tags, this)).ToList();
			profiles.Save();
		};

		CheckUpdateOnStart();
	}

	private void LoadSettings(Models.Settings settings)
	{
		SideBarPosition = settings.SidebarPosition switch
		{
			Position.Right => 3,
			_ => 0
		};
	}

	private void Minimize(object parameter)
	{
		_window.WindowState = WindowState.Minimized;
	}

	private void Maximize(object parameter)
	{
		if (_window.WindowState == WindowState.Maximized)
		{
			_window.WindowState = WindowState.Normal;
			MaxiIcon = "\uFA40";
			MaxiIconFontSize = 12;
		}
		else
		{
			(MaxHeight, MaxWidth) = _windowHelper.GetMaximumSize();
			_window.WindowState = WindowState.Maximized;
			MaxiIcon = "\uF670";
			MaxiIconFontSize = 16;
		}
	}

	private void Close(object parameter)
	{
		_window.Close();
	}

	private void DeleteGames(object obj)
	{
		if (ItemsToRemove.Count > 0 && MessageBox.Show(Properties.Resources.DeleteConfirmMessage, Properties.Resources.MainWindowTitle, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
		{
			foreach (var item in ItemsToRemove)
			{
				Games.Remove(item);
			}
		}
	}

	private async void CheckUpdateOnStart()
	{
		if (!CurrentSettings.UpdatesAvNotification) return;
		System.Windows.Forms.NotifyIcon notifyIcon = new();
		if (await Internet.IsAvailableAsync())
		{
			notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(AppDomain.CurrentDomain.BaseDirectory + @"\Gavilya.exe");

			if (Update.IsAvailable(await Update.GetLastVersionAsync(Context.LastVersionLink), Context.Version))
			{
				notifyIcon.Visible = true; // Show
				notifyIcon.ShowBalloonTip(5000, Properties.Resources.MainWindowTitle, Properties.Resources.UpdateAvMessageNotify, System.Windows.Forms.ToolTipIcon.Info);
				notifyIcon.Visible = false; // Hide
			}
		}
	}
}
