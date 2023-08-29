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
using System.Windows;
using System.Windows.Input;

namespace Gavilya.ViewModels;

public class ProfileCompViewModel : ViewModelBase
{
	private readonly Profile _profile;
	private readonly ProfileData _profiles;
	private readonly ProfileViewModel _profileViewModel;
	private readonly bool _isCurrent;

	private string _profileName;
	public string ProfileName { get => _profileName; set { _profileName = value; OnPropertyChanged(nameof(ProfileName)); } }

	private string _profilePicture = "pack://application:,,,/Gavilya;component/Assets/DefaultPP.png";
	public string ProfilePicture { get => _profilePicture; set { _profilePicture = value; OnPropertyChanged(nameof(ProfilePicture)); } }

	private Visibility _currentLabelVis;
	public Visibility CurrentLabelVis { get => _currentLabelVis; set { _currentLabelVis = value; OnPropertyChanged(nameof(CurrentLabelVis)); } }

	private Visibility _deleteVis;
	public Visibility DeleteVis { get => _deleteVis; set { _deleteVis = value; OnPropertyChanged(nameof(DeleteVis)); } }

	public ICommand SwitchCommand { get; }
	public ICommand EditCommand { get; }
	public ICommand DeleteCommand { get; }
	public ProfileCompViewModel(Profile profile, ProfileData profiles, ProfileViewModel profileViewModel)
	{
		_profile = profile;
		_profiles = profiles;
		_profileViewModel = profileViewModel;
		_isCurrent = _profiles.SelectedProfileUuid == _profile.ProfileUuid;

		CurrentLabelVis = _isCurrent ? Visibility.Visible : Visibility.Collapsed;
		DeleteVis = _isCurrent ? Visibility.Collapsed : Visibility.Visible;
		ProfilePicture = string.IsNullOrEmpty(profile.ProfilePictureFilePath) ? "pack://application:,,,/Gavilya;component/Assets/DefaultPP.png" : profile.ProfilePictureFilePath;
		ProfileName = _profile.Name;

		SwitchCommand = new RelayCommand(Switch);
		EditCommand = new RelayCommand(Edit);
		DeleteCommand = new RelayCommand(Delete);
	}

	private void Switch(object? obj)
	{
		_profiles.SelectedProfileUuid = _profile.ProfileUuid;
		_profiles.Save();
		Process.Start(AppDomain.CurrentDomain.BaseDirectory + @"\Gavilya.exe"); // Start Gavilya
		Application.Current.Shutdown(0);
	}

	private void Edit(object? obj)
	{
		_profileViewModel.IsProfileEditorOpen = true;
		_profileViewModel.ProfileAddMode = false;
		_profileViewModel.ProfileToEdit = _profile;
		_profileViewModel.EditProfilePicture = string.IsNullOrEmpty(_profile.ProfilePictureFilePath) ? "pack://application:,,,/Gavilya;component/Assets/DefaultPP.png" : _profile.ProfilePictureFilePath;
		_profileViewModel.EditProfileName = _profile.Name;
	}

	private void Delete(object? obj)
	{
		if (!_isCurrent && MessageBox.Show(Properties.Resources.DeleteProfileMsg, Properties.Resources.Profiles, MessageBoxButton.YesNoCancel, MessageBoxImage.Question) == MessageBoxResult.No)
		{
			return;
		}
		_profiles.Profiles.Remove(_profile);
		_profileViewModel.Refresh();
	}
}
