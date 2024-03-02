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
using PeyrSharp.Core.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;

namespace Gavilya.ViewModels.Settings;

public class GameOptionsViewModel : ViewModelBase
{
	private readonly Profile _profile;
	private readonly ProfileData _profileData;
	private readonly MainViewModel _mainViewModel;

	private bool _showHiddenGames;
	public bool ShowHiddenGames { get => _showHiddenGames; set { _showHiddenGames = value; OnPropertyChanged(nameof(ShowHiddenGames)); } }

	private List<TagViewModel> _tagsVm;
	public List<TagViewModel> TagsVm { get => _tagsVm; set { _tagsVm = value; OnPropertyChanged(nameof(TagsVm)); } }
	public List<Tag> Tags { get; set; }

	private string _tagName;
	public string TagName { get => _tagName; set { _tagName = value; OnPropertyChanged(nameof(TagName)); } }

	private string _color;
	public string Color { get => _color; set { _color = value; SolidColorBrush = GetBrushFromHex(value); OnPropertyChanged(nameof(Color)); } }

	private SolidColorBrush _solidColorBrush;
	public SolidColorBrush SolidColorBrush { get => _solidColorBrush; set { _solidColorBrush = value; OnPropertyChanged(nameof(SolidColorBrush)); } }

	public ICommand CheckHiddenGames { get; }
	public ICommand AddTagCommand { get; }
	public ICommand ChangeColorCommand { get; }

	public GameOptionsViewModel(Profile profile, ProfileData profileData, MainViewModel mainViewModel)
	{
		_profile = profile;
		_profileData = profileData;
		_mainViewModel = mainViewModel;

		ShowHiddenGames = profile.Settings.ShowHiddenGames;
		Tags = profile.Tags;
		TagsVm = Tags.Select(t => new TagViewModel(t, Tags, _profile.Games, Refresh)).ToList();

		Random random = new();
		Color = new RGB(random.Next(255), random.Next(255), random.Next(255)).ToHex().Value;

		CheckHiddenGames = new RelayCommand(HandleCheck);
		AddTagCommand = new RelayCommand(Add);
		ChangeColorCommand = new RelayCommand(ChangeColor);
	}

	private void HandleCheck(object? o)
	{
		_profileData.Profiles[_profileData.Profiles.IndexOf(_profile)].Settings.ShowHiddenGames = ShowHiddenGames;
		_mainViewModel.CurrentSettings = _profileData.Profiles[_profileData.Profiles.IndexOf(_profile)].Settings;
		_profileData.Save();
		_mainViewModel.Games.Refresh();
	}

	private void Add(object? obj)
	{
		if (string.IsNullOrEmpty(TagName)) return;
		Tag tag = new(TagName, Color);
		Tags.Add(tag);
		Refresh();
	}

	internal void Refresh()
	{
		TagsVm = Tags.Select(t => new TagViewModel(t, Tags, _profile.Games, Refresh)).ToList();
		_profileData.Save();
	}

	private void ChangeColor(object? obj)
	{
		// Create color picker/dialog
		System.Windows.Forms.ColorDialog colorDialog = new()
		{
			AllowFullOpen = true,
		};

		// If the user selected a color
		if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
		{
			RGB rgb = new(colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B);
			Color = rgb.ToHex().Value;
		}
	}

	private static SolidColorBrush GetBrushFromHex(string hex)
	{
		var color = new HEX(hex).ToRgb().Color;
		return new SolidColorBrush { Color = System.Windows.Media.Color.FromRgb(color.R, color.G, color.B) };
	}
}
