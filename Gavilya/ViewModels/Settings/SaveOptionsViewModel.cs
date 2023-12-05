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
using Microsoft.Win32;
using PeyrSharp.Env;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Gavilya.ViewModels.Settings;

public class SaveOptionsViewModel : ViewModelBase
{
	private readonly Profile _profile;
	private readonly ProfileData _profileData;
	private readonly GameList _games;
	private readonly MainViewModel _mainViewModel;
	private int _monthDay;
	public int MonthDay
	{
		get => _monthDay; set
		{
			_monthDay = value;
			_profileData.Profiles[_profileData.Profiles.IndexOf(_profile)].Settings.AutoSaveDay = MonthDay + 1;
			SaveInfo();
			OnPropertyChanged(nameof(MonthDay));
		}
	}

	private string _saveLocation;
	public string SaveLocation { get => _saveLocation; set { _saveLocation = value; OnPropertyChanged(nameof(SaveLocation)); } }

	private bool _makeSave;
	public bool MakeSave { get => _makeSave; set { _makeSave = value; OnPropertyChanged(nameof(MakeSave)); } }

	public IEnumerable<int> Days => Enumerable.Range(1, 31);

	public ICommand ImportCommand { get; }
	public ICommand ExportCommand { get; }
	public ICommand BrowseCommand { get; }
	public ICommand SaveNowCommand { get; }
	public ICommand MakeSaveCommand { get; }

	public SaveOptionsViewModel(Profile profile, ProfileData profileData, GameList games, MainViewModel mainViewModel)
	{
		_profile = profile;
		_profileData = profileData;
		_games = games;
		_mainViewModel = mainViewModel;
		SaveLocation = profile.Settings.SavePath;
		MakeSave = profile.Settings.MakeAutoSave;
		MonthDay = profile.Settings.AutoSaveDay - 1;

		ExportCommand = new RelayCommand(Export);
		ImportCommand = new RelayCommand(Import);
		BrowseCommand = new RelayCommand(Browse);
		SaveNowCommand = new RelayCommand(SaveNow);
		MakeSaveCommand = new RelayCommand(SetMakeSave);
	}

	private void Export(object? obj)
	{
        // Create a SaveFileDialog
        SaveFileDialog saveFileDialog = new()
		{
			Filter = $"{Properties.Resources.GavFiles}|*.g4vgames", // Extension
			Title = Properties.Resources.ExportGames // Title
		};

		if (saveFileDialog.ShowDialog() ?? true)
		{
			_games.Export(saveFileDialog.FileName);
		}
	}

	private void Import(object? obj)
	{
        // Create an OpenFileDialog
        OpenFileDialog openFileDialog = new()
		{
			Filter = $"{Properties.Resources.GavFiles}|*.g4vgames", // Extension
			Title = Properties.Resources.ImportGames // Title
		};

		if (openFileDialog.ShowDialog() ?? true) // If the user opened a file
		{
			if (MessageBox.Show(Properties.Resources.ImportConfirmMsg, Properties.Resources.MainWindowTitle, MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
			{
				_games.Import(openFileDialog.FileName);
			}
		}
	}

	private void Browse(object? obj)
	{
		try
		{
            // Create a SaveFileDialog
            SaveFileDialog saveFileDialog = new()
			{
				FileName = Properties.Resources.Games,
				Filter = $"{Properties.Resources.GavFiles}|*.g4v", // Extension
				Title = Properties.Resources.SaveLocation // Title
			};

			if (saveFileDialog.ShowDialog() ?? true)
			{
				SaveLocation = Path.GetDirectoryName(saveFileDialog.FileName) ?? $@"{FileSys.AppDataPath}\Léo Corporation\Gavilya\Backups"; // Location of the file
				_profileData.Profiles[_profileData.Profiles.IndexOf(_profile)].Settings.SavePath = SaveLocation;
				SaveInfo();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}

	private void SaveNow(object? obj)
	{
		_profileData.Backup(_profile.Settings.SavePath);
	}

	private void SetMakeSave(object? obj)
	{
		_profileData.Profiles[_profileData.Profiles.IndexOf(_profile)].Settings.MakeAutoSave = MakeSave;
		SaveInfo();
	}

	private void SaveInfo()
	{
		_mainViewModel.CurrentSettings = _profileData.Profiles[_profileData.Profiles.IndexOf(_profile)].Settings;
		_profileData.Save();
	}
}
