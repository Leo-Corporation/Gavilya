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
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace Gavilya.ViewModels.Settings;

public class LanguageViewModel : ViewModelBase
{
	private readonly Profile _profile;
	private readonly ProfileData _profileData;
	private readonly MainViewModel _mainViewModel;

	private int _selectedIndex;
	public int SelectedIndex { get => _selectedIndex; set { _selectedIndex = value; OnPropertyChanged(nameof(SelectedIndex)); } }

	public ICommand SaveCommand { get; }

	public LanguageViewModel(Profile profile, ProfileData profileData, MainViewModel mainViewModel)
	{
		_profile = profile;
		_profileData = profileData;
		_mainViewModel = mainViewModel;

		SelectedIndex = (int)profile.Settings.Language;

		SaveCommand = new RelayCommand(SaveLanguage);
	}

	private void SaveLanguage(object? obj)
	{
		_profileData.Profiles[_profileData.Profiles.IndexOf(_profile)].Settings.Language = (Language)SelectedIndex;
		_mainViewModel.CurrentSettings = _profileData.Profiles[_profileData.Profiles.IndexOf(_profile)].Settings;
		_profileData.Save();
		if (MessageBox.Show(Properties.Resources.GavilyaNeedsRestartChanges, Properties.Resources.MainWindowTitle, MessageBoxButton.YesNoCancel, MessageBoxImage.Question) == MessageBoxResult.Yes)
		{
			Process.Start(Directory.GetCurrentDirectory() + @"\Gavilya.exe");
			Environment.Exit(0);
		}
	}
}
