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
using System.Windows;
using System.Windows.Input;

namespace Gavilya.ViewModels.Settings;

public class FpsViewModel : ViewModelBase
{
	private readonly Profile _profile;
	private readonly ProfileData _profileData;
	private readonly MainViewModel _mainViewModel;

	private string _opacity;
	public string Opacity { get => _opacity; set { _opacity = value; OnPropertyChanged(nameof(Opacity)); } }

	public static string CombinationString => string.Format(Properties.Resources.OpenFpsCounter, "Control+Shift+F");
	public ICommand SaveCommand { get; }

	public FpsViewModel(Profile profile, ProfileData profileData, MainViewModel mainViewModel)
	{
		_profile = profile;
		_profileData = profileData;
		_mainViewModel = mainViewModel;

		Opacity = (profile.Settings.FpsCounterOpacity * 100).ToString();
		SaveCommand = new RelayCommand(Save);
	}

	private void Save(object? obj)
	{
		if (int.TryParse(Opacity, out int opa) && opa >= 0 && opa <= 100)
		{
			_profileData.Profiles[_profileData.Profiles.IndexOf(_profile)].Settings.FpsCounterOpacity = opa / 100d;
			_mainViewModel.CurrentSettings = _profileData.Profiles[_profileData.Profiles.IndexOf(_profile)].Settings;
			_profileData.Save();
			return;
		}

		MessageBox.Show(Properties.Resources.IncorrectValue, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
	}
}
