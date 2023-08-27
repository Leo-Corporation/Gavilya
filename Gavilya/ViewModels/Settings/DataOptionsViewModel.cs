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
public class DataOptionsViewModel : ViewModelBase
{
	private readonly Profile _profile;
	private readonly ProfileData _profiles;

	public ICommand ResetCommand { get; }

	public DataOptionsViewModel(Profile profile, ProfileData profiles)
    {
		_profile = profile;
		_profiles = profiles;

		ResetCommand = new RelayCommand(Reset);
	}

    private void Reset(object? obj)
    {
		if (MessageBox.Show(Properties.Resources.ResetSettingsMsg, Properties.Resources.MainWindowTitle, MessageBoxButton.YesNoCancel, MessageBoxImage.Question) == MessageBoxResult.Yes)
		{
			_profile.Settings = new() { IsFirstRun = false };
			_profiles.Profiles[_profiles.Profiles.IndexOf(_profile)] = _profile;
			_profiles.Save();

			MessageBox.Show(Properties.Resources.GavilyaNeedsRestartChanges, Properties.Resources.ResetSettings, MessageBoxButton.OK, MessageBoxImage.Information);
			Process.Start(Directory.GetCurrentDirectory() + @"\Gavilya.exe");
			Environment.Exit(0); // Quit
		}
	}
}
