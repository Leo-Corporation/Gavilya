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
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Gavilya.ViewModels.Settings;
public class ThemeSelectorViewModel : ViewModelBase
{
	private readonly Profile _profile;
	private readonly ProfileData _profileData;
	private readonly MainViewModel _mainViewModel;
	private (ThemeInfo, string) _themeInfo;
	public ICommand ClickCommand { get; }

	private SolidColorBrush _background;
	public SolidColorBrush Background { get => _background; set { _background = value; OnPropertyChanged(nameof(Background)); } }

	private SolidColorBrush _background2;
	public SolidColorBrush Background2 { get => _background2; set { _background2 = value; OnPropertyChanged(nameof(Background2)); } }

	private LinearGradientBrush _playGradient;
	public LinearGradientBrush PlayGradient { get => _playGradient; set { _playGradient = value; OnPropertyChanged(nameof(PlayGradient)); } }

	private LinearGradientBrush _playGradientHover;
	public LinearGradientBrush PlayGradientHover { get => _playGradientHover; set { _playGradientHover = value; OnPropertyChanged(nameof(PlayGradientHover)); } }

	private LinearGradientBrush _goldGradient;
	public LinearGradientBrush GoldGradient { get => _goldGradient; set { _goldGradient = value; OnPropertyChanged(nameof(GoldGradient)); } }

	private Thickness _borderThickness = new(0);
	public Thickness BorderThickness { get => _borderThickness; set { _borderThickness = value; OnPropertyChanged(nameof(BorderThickness)); } }
	
	public static event EventHandler? ThemeChanged;
	public ThemeSelectorViewModel(Profile profile, ProfileData profileData, MainViewModel mainViewModel, (ThemeInfo, string) themeInfo)
	{
		_profile = profile;
		_profileData = profileData;
		_mainViewModel = mainViewModel;
		_themeInfo = themeInfo;

		BorderThickness = new(_profile.Settings.CurrentTheme.Replace(@"\theme.manifest", "") == _themeInfo.Item2 ? 2 : 0); // Set border thickness
		ClickCommand = new RelayCommand(Click);

		LoadThemeColors(); // Load theme colors
	}

	private void LoadThemeColors()
	{
		ResourceDictionary themeResources = new()
		{
			Source = _themeInfo.Item2 == "" ? new("..\\Themes\\Dark.xaml", UriKind.Relative) : new($@"{_themeInfo.Item2}\{_themeInfo.Item1.FilePath}")
		};

		Background = (SolidColorBrush)themeResources["Background"];
		Background2 = (SolidColorBrush)themeResources["Background2"];
		PlayGradient = (LinearGradientBrush)themeResources["PlayGradient"];
		PlayGradientHover = (LinearGradientBrush)themeResources["PlayGradientHover"];
		GoldGradient = (LinearGradientBrush)themeResources["GoldGradient"];
	}

	private void Click(object o)
	{
		ChangeTheme();
		ThemeChanged?.Invoke(this, EventArgs.Empty);
	}

	private void ChangeTheme()
	{
		// If the theme is the default one
		if (_themeInfo.Item2 == "")
		{
			Application.Current.Resources.MergedDictionaries.Clear();

			// Create a resource dictionary
			ResourceDictionary resourceDictionary = new()
			{
				Source = new("..\\Themes\\Dark.xaml", UriKind.Relative) // Add source
			};

			Application.Current.Resources.MergedDictionaries.Add(resourceDictionary); // Add the dictionary

			_profileData.Profiles[_profileData.Profiles.IndexOf(_profile)].Settings.CurrentTheme = _themeInfo.Item2;
			_mainViewModel.CurrentSettings.CurrentTheme = "";
			_profileData.Save();
			return;
		}

		_profileData.Profiles[_profileData.Profiles.IndexOf(_profile)].Settings.CurrentTheme = _themeInfo.Item2 + $@"\theme.manifest";
		_profileData.Save();
		_mainViewModel.CurrentSettings.CurrentTheme = _themeInfo.Item2 + $@"\theme.manifest";

		ThemeHelper.ChangeTheme(_themeInfo.Item1, _themeInfo.Item2);
	}
}
