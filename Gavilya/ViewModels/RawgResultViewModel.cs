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
using Gavilya.Models.Rawg;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Gavilya.ViewModels;

public class RawgResultViewModel : ViewModelBase
{
	private readonly RawgGame _game;
	private readonly GameEditionViewModel _gameEditionViewModel;
	private string _name;
	public string Name { get => _name; set { _name = value; OnPropertyChanged(nameof(Name)); } }

	private string _description;
	public string Description { get => _description; set { _description = value; OnPropertyChanged(nameof(Description)); } }

	private ImageSource _firstScreen;
	public ImageSource FirstScreen { get => _firstScreen; set { _firstScreen = value; OnPropertyChanged(nameof(FirstScreen)); } }

	private ImageSource _secondScreen;
	public ImageSource SecondScreen { get => _secondScreen; set { _secondScreen = value; OnPropertyChanged(nameof(SecondScreen)); } }

	private Visibility _collapseGridVis = Visibility.Collapsed;
	public Visibility CollapseGridVid { get => _collapseGridVis; set { _collapseGridVis = value; OnPropertyChanged(nameof(CollapseGridVid)); } }

	private string _collapseIcon = "\uF2A4";
	public string CollapseIcon { get => _collapseIcon; set { _collapseIcon = value; OnPropertyChanged(nameof(CollapseIcon)); } }
	public (string, int) SelectedScreen { get; set; }
	public ICommand SelectCommand { get; }
	public ICommand CollapseCommand { get; }
	public ICommand SelectFirstScreenCommand { get; }
	public ICommand SelectSecondScreenCommand { get; }

	private Thickness _thickness1;
	public Thickness Thickness1 { get => _thickness1; set { _thickness1 = value; OnPropertyChanged(nameof(Thickness1)); } }

	private Thickness _thickness2;
	public Thickness Thickness2 { get => _thickness2; set { _thickness2 = value; OnPropertyChanged(nameof(Thickness2)); } }
	private bool _detailsLoaded = false;
	public RawgResultViewModel(RawgGame rawgGame, GameEditionViewModel gameEditionViewModel)
	{
		_game = rawgGame;
		_gameEditionViewModel = gameEditionViewModel;

		// Load properties
		if (!string.IsNullOrEmpty(_game.Name)) Name = _game.Name.Length > 50 ? $"{_game.Name[0..50]}..." : _game.Name;
		if (!string.IsNullOrEmpty(_game.Description)) Description = _game.DescriptionRaw.Length > 150 ? $"{_game.DescriptionRaw[0..150]}..." : _game.DescriptionRaw;
		SelectedScreen = (_game.BackgroundImage, 0);

		// Commands
		SelectCommand = new RelayCommand(Select);
		CollapseCommand = new RelayCommand(Collapse);
		SelectFirstScreenCommand = new RelayCommand(SelectFirstScreen);
		SelectSecondScreenCommand = new RelayCommand(SelectSecondScreen);
	}

	private async void LoadDetails()
	{
		var game = await new RawgClient(_game.Id).GetGameAsync();
		if (!string.IsNullOrEmpty(_game.BackgroundImage)) FirstScreen = new BitmapImage(new(_game.BackgroundImage));
		if (!string.IsNullOrEmpty(game?.BackgroundImageAdditional)) SecondScreen = new BitmapImage(new(game.BackgroundImageAdditional));
		_game.BackgroundImageAdditional = game?.BackgroundImageAdditional ?? "";
		_game.DescriptionRaw = game?.DescriptionRaw ?? "";
		_detailsLoaded = true;
	}

	private async void Select(object? obj)
	{
		try
		{
			if (!_detailsLoaded) LoadDetails();

			_gameEditionViewModel.CoverFilePath = await new CoverImageHelper(SelectedScreen.Item1, SelectedScreen.Item2, _game.Id).Download();

			BitmapImage bmp = new();
			bmp.BeginInit();
			bmp.UriSource = new Uri(_gameEditionViewModel.CoverFilePath);
			bmp.EndInit();
			bmp.Freeze();

			_gameEditionViewModel.ImageSource = bmp; // Create the image
			_gameEditionViewModel.Description = _game.DescriptionRaw;
			_gameEditionViewModel.RawgId = _game.Id;
			_gameEditionViewModel.IsRawgOpen = false;
		}
		catch
		{

		}
	}

	private void Collapse(object? obj)
	{
		CollapseGridVid = CollapseGridVid == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
		CollapseIcon = CollapseGridVid == Visibility.Visible ? "\uF2B7" : "\uF2A4";
		if (!_detailsLoaded) LoadDetails();
	}

	private void SelectFirstScreen(object? obj)
	{
		SelectedScreen = (_game.BackgroundImage, 0);
		Thickness1 = new(2);
		Thickness2 = new(0);
	}

	private void SelectSecondScreen(object? obj)
	{
		SelectedScreen = (_game.BackgroundImageAdditional, 1);
		Thickness1 = new(0);
		Thickness2 = new(2);
	}
}
