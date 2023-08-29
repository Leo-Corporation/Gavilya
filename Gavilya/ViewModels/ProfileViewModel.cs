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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Gavilya.ViewModels;

public class ProfileViewModel : ViewModelBase
{
	private readonly Profile _profile;
	private readonly ProfileData _profileData;
	private readonly GameList _games;
	private readonly MainViewModel _mainViewModel;

	public ObservableCollection<StatGameViewModel> TopGames { get; set; }

	private double _recHeight1;
	public double RecHeight1 { get => _recHeight1; set { _recHeight1 = value; OnPropertyChanged(nameof(RecHeight1)); } }

	private double _recHeight2;
	public double RecHeight2 { get => _recHeight2; set { _recHeight2 = value; OnPropertyChanged(nameof(RecHeight2)); } }

	private double _recHeight3;
	public double RecHeight3 { get => _recHeight3; set { _recHeight3 = value; OnPropertyChanged(nameof(RecHeight3)); } }

	private string _totalText;
	public string TotalText { get => _totalText; set { _totalText = value; OnPropertyChanged(nameof(TotalText)); } }

	private string _profilePicture = "pack://application:,,,/Gavilya;component/Assets/DefaultPP.png";
	public string ProfilePicture { get => _profilePicture; set { _profilePicture = value; OnPropertyChanged(nameof(ProfilePicture)); } }

	private string _profileName;
	public string ProfileName { get => _profileName; set { _profileName = value; OnPropertyChanged(nameof(ProfileName)); } }

	private bool _isProfileEditorOpen;
	public bool IsProfileEditorOpen { get => _isProfileEditorOpen; set { _isProfileEditorOpen = value; OnPropertyChanged(nameof(IsProfileEditorOpen)); } }

	private List<ProfileCompViewModel> _profilesVm;
	public List<ProfileCompViewModel> ProfilesVm { get => _profilesVm; set { _profilesVm = value; OnPropertyChanged(nameof(ProfilesVm)); } }

	private string _editProfilePicture;
	public string EditProfilePicture { get => _editProfilePicture; set { _editProfilePicture = value; OnPropertyChanged(nameof(EditProfilePicture)); } }

	private string _editProfileName;
	public string EditProfileName { get => _editProfileName; set { _editProfileName = value; OnPropertyChanged(nameof(EditProfileName)); } }
	
	private Visibility _contentVis = Visibility.Visible;
	public Visibility ContentVis { get => _contentVis; set { _contentVis = value; OnPropertyChanged(nameof(ContentVis)); } }

	public ICommand AddProfileCommand { get; }
	public ICommand EditCommand { get; }
	public ICommand PopupAddCommand { get; }
	public ICommand PopupCancelCommand { get; }
	public ICommand PopupBrowseCommand { get; }
	public ICommand PopupResetCommand { get; }

	internal Profile? ProfileToEdit { get; set; }
	internal bool ProfileAddMode { get; set; }

	public ProfileViewModel(Profile profile, ProfileData profileData, GameList games, MainViewModel mainViewModel)
	{
		_profile = profile;
		_profileData = profileData;
		_games = games;
		_mainViewModel = mainViewModel;

		int total = 0;
		for (int i = 0; i < _games.Count; i++)
		{
			total += games[i].TotalTimePlayed;
		}
		TotalText = $"{total / 3600d:0.0}{Properties.Resources.HourShort}";
		var sortedGames = _games.SortByPlayTime(true);

		TopGames = new(sortedGames.Take(3).Select((g, i) => new StatGameViewModel(i, g, null)));
		ProfilePicture = string.IsNullOrEmpty(profile.ProfilePictureFilePath) ? "pack://application:,,,/Gavilya;component/Assets/DefaultPP.png" : profile.ProfilePictureFilePath;
		ProfileName = profile.Name;

		Refresh();
		AddProfileCommand = new RelayCommand(AddProfile);
		EditCommand = new RelayCommand(Edit);
		PopupAddCommand = new RelayCommand(PopupAdd);
		PopupCancelCommand = new RelayCommand(PopupCancel);
		PopupBrowseCommand = new RelayCommand(PopupBrowse);
		PopupResetCommand = new RelayCommand(PopupReset);

		// Load graph
		if (sortedGames.Count < 3)
		{
			ContentVis = Visibility.Collapsed;
			return;
		}
		double max = sortedGames[0].TotalTimePlayed;
		RecHeight1 = 150;
		RecHeight2 = sortedGames[1].TotalTimePlayed / max * 150;
		RecHeight3 = sortedGames[2].TotalTimePlayed / max * 150;

	}

	private void AddProfile(object? obj)
	{
		IsProfileEditorOpen = true;
		ProfileAddMode = true;
		ProfileToEdit = null;
		EditProfilePicture = "pack://application:,,,/Gavilya;component/Assets/DefaultPP.png";
	}

	private void Edit(object? obj)
	{
		IsProfileEditorOpen = true;
		ProfileAddMode = false;
		ProfileToEdit = _profile;
		EditProfileName = _profile.Name;
		EditProfilePicture = string.IsNullOrEmpty(_profile.ProfilePictureFilePath) ? "pack://application:,,,/Gavilya;component/Assets/DefaultPP.png" : _profile.ProfilePictureFilePath;
	}

	private void PopupAdd(object? obj)
	{
		if (ProfileAddMode)
		{
			_profileData.Profiles.Add(new(_editProfileName) { ProfilePictureFilePath = EditProfilePicture });
			_profileData.Save();
		}
		else
		{
			if (ProfileToEdit is not null && _profileData.Profiles.Contains(ProfileToEdit))
			{
				_profileData.Profiles[_profileData.Profiles.IndexOf(ProfileToEdit)].Name = EditProfileName;
				_profileData.Profiles[_profileData.Profiles.IndexOf(ProfileToEdit)].ProfilePictureFilePath = EditProfilePicture == "pack://application:,,,/Gavilya;component/Assets/DefaultPP.png" ? "" : EditProfilePicture;
				_profileData.Save();

				if (ProfileToEdit.ProfileUuid == _profile.ProfileUuid)
				{
					ProfileName = EditProfileName;
					ProfilePicture = EditProfilePicture;
					_mainViewModel.Nav.ProfilePicture = EditProfilePicture;
				}
			}
		}
		Refresh();
		IsProfileEditorOpen = false;
	}

	internal void Refresh()
	{
		ProfilesVm = _profileData.Profiles.Select(p => new ProfileCompViewModel(p, _profileData, this)).ToList();

	}

	private void PopupCancel(object? obj)
	{
		IsProfileEditorOpen = false;
	}

	private void PopupReset(object? obj)
	{
		EditProfilePicture = "pack://application:,,,/Gavilya;component/Assets/DefaultPP.png";
	}

	private void PopupBrowse(object? obj)
	{
		OpenFileDialog openFileDialog = new()
		{
			Filter = "PNG|*.png|JPG|*.jpg|Bitmap|*.bmp|All Files|*.*" // Filter
		}; // OpenFileDialog

		if (openFileDialog.ShowDialog() ?? true) // If the user selected a file
		{
			try
			{
				EditProfilePicture = openFileDialog.FileName;

			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); // Show the error
			}
		}
	}
}
