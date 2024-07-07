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
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Gavilya.ViewModels.Settings;

public class ThemeViewModel : ViewModelBase
{
	private readonly Profile _profile;
	private readonly ProfileData _profileData;
	private readonly MainViewModel _mainViewModel;
	private List<(ThemeInfo, string)> _installedThemes;

	private List<string> _themeNames;
	public List<string> ThemeNames { get => _themeNames; set { _themeNames = value; OnPropertyChanged(nameof(ThemeNames)); } }

	private int _index;
	public int Index { get => _index; set { _index = value; ChangeTheme(value); OnPropertyChanged(nameof(Index)); } }

	public ICommand ImportCommand { get; }
	public ICommand GetThemesCommand { get; }

	public ThemeViewModel(Profile profile, ProfileData profileData, MainViewModel mainViewModel)
	{
		_profile = profile;
		_profileData = profileData;
		_mainViewModel = mainViewModel;
		_installedThemes = ThemeHelper.GetInstalledThemes();

		var current = profile.Settings.CurrentTheme == "" ? _installedThemes[0].Item1 : ThemeHelper.GetThemeFromPath(profile.Settings.CurrentTheme);

		ThemeNames = _installedThemes.Select(t => t.Item1.Name).ToList();
		Index = _installedThemes.IndexOf(_installedThemes.Where(t => t.Item1.Equals(current)).First());

		ImportCommand = new RelayCommand(Import);
		GetThemesCommand = new RelayCommand(GetThemesOnline);
	}

	private void ChangeTheme(int i)
	{
		try
		{
			if (i == 0) // If the default theme is selected
			{
				Application.Current.Resources.MergedDictionaries.Clear();

				// Create a resource dictionary
				ResourceDictionary resourceDictionary = new()
				{
					Source = new Uri("..\\Themes\\Dark.xaml", UriKind.Relative) // Add source
				};

				Application.Current.Resources.MergedDictionaries.Add(resourceDictionary); // Add the dictionary

				_profileData.Profiles[_profileData.Profiles.IndexOf(_profile)].Settings.CurrentTheme = _installedThemes[i].Item2;
				_mainViewModel.CurrentSettings.CurrentTheme = "";
				_profileData.Save();

				return;
			}

			ThemeHelper.ChangeTheme(_installedThemes[i].Item1, _installedThemes[i].Item2);
			_profileData.Profiles[_profileData.Profiles.IndexOf(_profile)].Settings.CurrentTheme = _installedThemes[i].Item2 + $@"\theme.manifest";
			_profileData.Save();
			_mainViewModel.CurrentSettings.CurrentTheme = _installedThemes[i].Item2 + $@"\theme.manifest";
		}
		catch (IndexOutOfRangeException ex)
		{
			Console.WriteLine("Failed to change theme, code: " + ex.StackTrace);
		}
	}

	private void Import(object? obj)
	{
		OpenFileDialog openFileDialog = new()
		{
			DefaultExt = ".gavtheme",
			Filter = $"{Properties.Resources.Themes}|*.gavtheme",
			Title = Properties.Resources.ThemesImport
		};

		if (openFileDialog.ShowDialog() ?? false)
		{
			ThemeHelper.InstallTheme(openFileDialog.FileName);
			_installedThemes = ThemeHelper.GetInstalledThemes();
			ThemeNames = _installedThemes.Select(t => t.Item1.Name).ToList();
		}
	}

	private void GetThemesOnline(object? obj)
	{
		Process.Start("explorer.exe", "https://gavilya.leocorporation.dev/themes");
	}
}
