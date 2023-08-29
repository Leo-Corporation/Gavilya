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
using Gavilya.Services;
using Microsoft.Win32;
using PeyrSharp.Env;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Gavilya.ViewModels.FirstRun;
public class ImportViewModel : ViewModelBase
{
	private readonly FirstRunViewModel _firstRunViewModel;
	private readonly Profile _profile;
	private readonly ProfileData _profileData;

	public ICommand MigrateCommand { get; }
	public ICommand ImportCommand { get; }
	public ICommand NextCommand { get; }

	public ImportViewModel(FirstRunViewModel firstRunViewModel, Profile profile, ProfileData profileData)
	{
		_firstRunViewModel = firstRunViewModel;
		_profile = profile;
		_profileData = profileData;

		MigrateCommand = new RelayCommand(Migrate);
		ImportCommand = new RelayCommand(Import);
		NextCommand = new RelayCommand((o) => _firstRunViewModel.CurrentViewModel = new JumpInViewModel(_profile, _profileData));
	}

	private void Migrate(object? o)
	{
		OpenFileDialog openFileDialog = new()
		{
			Filter = $"{Properties.Resources.GavFiles} (v3)|*.gav", // Extension
			InitialDirectory = $@"{FileSys.AppDataPath}\Gavilya",
			FileName = "Games.gav",
			Title = Properties.Resources.ImportGames // Title
		}; // Create an OpenFileDialog
		if (openFileDialog.ShowDialog() ?? true) // If the user opened a file
		{
			if (MessageBox.Show(Properties.Resources.ImportConfirmMsg, Properties.Resources.MainWindowTitle, MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
			{
				GameMigrationService gameMigrationService = new(openFileDialog.FileName);
				_profileData.Profiles[_profileData.Profiles.IndexOf(_profile)].Games = gameMigrationService.Migrate();
				_profileData.Save();
			}
		}
	}

	private void Import(object? o)
	{
		OpenFileDialog openFileDialog = new()
		{
			Filter = $"{Properties.Resources.GavFiles}|*.g4vgames", // Extension
			Title = Properties.Resources.ImportGames // Title
		}; // Create an OpenFileDialog
		if (openFileDialog.ShowDialog() ?? true) // If the user opened a file
		{
			if (MessageBox.Show(Properties.Resources.ImportConfirmMsg, Properties.Resources.MainWindowTitle, MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
			{
				_profileData.Profiles[_profileData.Profiles.IndexOf(_profile)].Games.Import(openFileDialog.FileName);
				_profileData.Save();
			}
		}
	}
}
