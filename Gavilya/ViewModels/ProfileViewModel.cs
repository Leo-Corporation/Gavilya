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
using PeyrSharp.Core.Converters;
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
	public ObservableCollection<RecentGamesItemViewModel> RecentGames { get; set; }

	private string _totalText;
	public string TotalText { get => _totalText; set { _totalText = value; OnPropertyChanged(nameof(TotalText)); } }

	private string _totalGamesText;
	public string TotalGamesText { get => _totalGamesText; set { _totalGamesText = value; OnPropertyChanged(nameof(TotalGamesText)); } }

	private string _totalGamesTextDesc;
	public string TotalGamesTextDesc { get => _totalGamesTextDesc; set { _totalGamesTextDesc = value; OnPropertyChanged(nameof(TotalGamesTextDesc)); } }

	private string _lastSessionText;
	public string LastSessionText { get => _lastSessionText; set { _lastSessionText = value; OnPropertyChanged(nameof(LastSessionText)); } }

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

	private Visibility _placeholderVis = Visibility.Collapsed;
	public Visibility PlaceholderVis { get => _placeholderVis; set { _placeholderVis = value; OnPropertyChanged(nameof(PlaceholderVis)); } }

	private int _steamGamesCount = 0;
	public int SteamGamesCount { get => _steamGamesCount; set { _steamGamesCount = value; OnPropertyChanged(nameof(SteamGamesCount)); } }

	private int _microsoftGamesCount = 0;
	public int MicrosoftGamesCount { get => _microsoftGamesCount; set { _microsoftGamesCount = value; OnPropertyChanged(nameof(MicrosoftGamesCount)); } }

	private int _classicGamesCount = 0;
	public int ClassicGamesCount { get => _classicGamesCount; set { _classicGamesCount = value; OnPropertyChanged(nameof(ClassicGamesCount)); } }

	private int _gamesCount;
	public int GamesCount { get => _gamesCount; set { _gamesCount = value; OnPropertyChanged(nameof(GamesCount)); } }

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

			if (games[i].GameType == Enums.GameType.Steam) SteamGamesCount++;
			else if (games[i].GameType == Enums.GameType.UWP) MicrosoftGamesCount++;
			else ClassicGamesCount++;
		}

		GamesCount = _games.Count;

		TotalText = $"{total / 3600d:0.0}{Properties.Resources.HourShort}";
		var sortedGames = _games.SortByPlayTime(true, _profile.Settings.ShowHiddenGames);

		TotalGamesText = _games.Count.ToString();
		TotalGamesTextDesc = string.Format(Properties.Resources.GamesLibraryDesc, _games.GetNumberOfGamesLastWeek());

		// Get the last session time
		LastSessionText = GetLastSessionTime();

		double max = sortedGames.Count > 0 ? sortedGames[0].TotalTimePlayed : 0;

		TopGames = [.. sortedGames.Take(3).Select((g, i) => new StatGameViewModel(i, g, null, g.TotalTimePlayed / max * 100))];
		ProfilePicture = string.IsNullOrEmpty(profile.ProfilePictureFilePath) ? "pack://application:,,,/Gavilya;component/Assets/DefaultPP.png" : profile.ProfilePictureFilePath;
		ProfileName = profile.Name;

		RecentGames = [.. _games
			.OrderByDescending(g => g.LastTimePlayed)
			.Where(g => g.LastTimePlayed > 0 && (profile.Settings.ShowHiddenGames || !g.IsHidden))
			.Take(3)
			.Select(g => new RecentGamesItemViewModel(g))];

		Refresh();
		AddProfileCommand = new RelayCommand(AddProfile);
		EditCommand = new RelayCommand(Edit);
		PopupAddCommand = new RelayCommand(PopupAdd);
		PopupCancelCommand = new RelayCommand(PopupCancel);
		PopupBrowseCommand = new RelayCommand(PopupBrowse);
		PopupResetCommand = new RelayCommand(PopupReset);

		if (sortedGames.Count < 3)
		{
			ContentVis = Visibility.Collapsed;
			PlaceholderVis = Visibility.Visible;
			return;
		}
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

	/// <summary>
	/// Returns today if the date is today, yesterday if the date is yesterday,
	/// this week if the date is this week, this month if the date is this month,
	/// and x months ago otherwise.
	/// </summary>
	/// <returns></returns>
	private string GetLastSessionTime()
	{
		var lastSession = _games.OrderByDescending(g => g.LastTimePlayed)
			.FirstOrDefault(g => g.LastTimePlayed > 0);
		if (lastSession == null)
			return Properties.Resources.Never;

		DateTime lastSessionDate = Time.UnixTimeToDateTime(lastSession.LastTimePlayed);
		DateTime today = DateTime.Now.Date;
		return lastSessionDate.ToString("dd/MM/yyyy HH:mm") switch
		{
			_ when lastSessionDate.Date == today => Properties.Resources.Today,
			_ when lastSessionDate.Date == today.AddDays(-1) => Properties.Resources.Yesterday,
			_ when lastSessionDate >= today.AddDays(-(int)today.DayOfWeek) => Properties.Resources.ThisWeek,
			_ when lastSessionDate.Month == today.Month && lastSessionDate.Year == today.Year => Properties.Resources.ThisMonth,
			_ => $"{lastSessionDate:MMMM yyyy} ({(today - lastSessionDate).TotalDays} {Properties.Resources.Months})"
		};
	}

	internal void Refresh()
	{
		ProfilesVm = [.. _profileData.Profiles.Select(p => new ProfileCompViewModel(p, _profileData, this))];
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
		// OpenFileDialog
		OpenFileDialog openFileDialog = new()
		{
			Filter = "PNG|*.png|JPG|*.jpg|Bitmap|*.bmp|All Files|*.*" // Filter
		};

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
