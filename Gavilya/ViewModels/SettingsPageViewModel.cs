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
using Gavilya.ViewModels.Settings;
using System.Windows.Input;

namespace Gavilya.ViewModels;
public class SettingsPageViewModel : ViewModelBase
{
	private ViewModelBase _currentViewModel;
	public ViewModelBase CurrentViewModel { get => _currentViewModel; set { _currentViewModel = value; OnPropertyChanged(nameof(CurrentViewModel)); } }

	public ICommand AboutCommand { get; }
	public ICommand DataCommand { get; }
	public ICommand FpsCommand { get; }
	public ICommand GameOptionsCommand { get; }
	public ICommand HomeCommand { get; }
	public ICommand LanguageCommand { get; }
	public ICommand NotificationsCommand { get; }
	public ICommand SaveOptionsCommand { get; }
	public ICommand StartupCommand { get; }
	public ICommand SearchCommand { get; }
	public ICommand ThemeCommand { get; }

	public SettingsPageViewModel(Profile profile, ProfileData profiles, GameList games, MainViewModel mainViewModel)
	{
		AboutCommand = new RelayCommand((o) => CurrentViewModel = new AboutViewModel());
		DataCommand = new RelayCommand((o) => CurrentViewModel = new DataOptionsViewModel(profile, profiles));
		FpsCommand = new RelayCommand((o) => CurrentViewModel = new FpsViewModel(profile, profiles, mainViewModel));
		GameOptionsCommand = new RelayCommand((o) => CurrentViewModel = new GameOptionsViewModel(profile, profiles, mainViewModel));
		HomeCommand = new RelayCommand((o) => CurrentViewModel = new HomeOptionsViewModel(profile, profiles, mainViewModel));
		LanguageCommand = new RelayCommand((o) => CurrentViewModel = new LanguageViewModel(profile, profiles, mainViewModel));
		NotificationsCommand = new RelayCommand((o) => CurrentViewModel = new NotificationsViewModel(profile, profiles, mainViewModel));
		SaveOptionsCommand = new RelayCommand((o) => CurrentViewModel = new SaveOptionsViewModel(profile, profiles, games, mainViewModel));
		StartupCommand = new RelayCommand((o) => CurrentViewModel = new StartupViewModel(profile, profiles, mainViewModel));
		SearchCommand = new RelayCommand((o) => CurrentViewModel = new SearchViewModel(profile, profiles, mainViewModel));
		ThemeCommand = new RelayCommand((o) => CurrentViewModel = new ThemeViewModel(profile, profiles, mainViewModel));

		CurrentViewModel = new SaveOptionsViewModel(profile, profiles, games, mainViewModel);
	}
}
