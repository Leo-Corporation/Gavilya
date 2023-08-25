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
using Gavilya.Enums;
using Gavilya.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Gavilya.ViewModels;

public class GameEditionViewModel : ViewModelBase
{
	private readonly MainViewModel _mainViewModel;
	Game? Game { get; set; }
	GameType GameType { get; init; }
	GameList Games { get; set; }
	public List<Tag> Tags { get; }
	public List<Tag> SelectedTags { get; set; }
	public List<TagSelectorViewModel> TagsVm => Tags.Select(t => new TagSelectorViewModel(SelectedTags, t, SelectedTags.Contains(t))).ToList();

	private string _name;
	public string Name
	{
		get => _name;
		set
		{
			_name = value;
			OnPropertyChanged(nameof(Name));
		}
	}

	private string _description;
	public string Description
	{
		get => _description;
		set
		{
			_description = value;
			OnPropertyChanged(nameof(Description));
		}
	}

	private string _coverFilePath;
	public string CoverFilePath
	{
		get => _coverFilePath;
		set
		{
			_coverFilePath = value;
			OnPropertyChanged(nameof(CoverFilePath));
		}
	}

	private string _command;
	public string Command
	{
		get => _command;
		set
		{
			_command = value;
			OnPropertyChanged(nameof(Command));
		}
	}

	private string _process;
	public string Process
	{
		get => _process;
		set
		{
			_process = value;
			OnPropertyChanged(nameof(Process));
		}
	}

	private bool _checkIfRunning;
	public bool CheckIfRunning
	{
		get => _checkIfRunning;
		set
		{
			_checkIfRunning = value;
			OnPropertyChanged(nameof(CheckIfRunning));
		}
	}

	private bool _isHidden;
	public bool IsHidden
	{
		get => _isHidden;
		set
		{
			_isHidden = value;
			OnPropertyChanged(nameof(IsHidden));
		}
	}

	private bool _isTagOpen = false;
	public bool IsTagOpen
	{
		get => _isTagOpen;
		set
		{
			_isTagOpen = value;
			OnPropertyChanged(nameof(IsTagOpen));
		}
	}

	private bool _isRawgOpen = false;
	public bool IsRawgOpen { get => _isRawgOpen; set { _isRawgOpen = value; OnPropertyChanged(nameof(IsRawgOpen)); } }

	private string _steamId;
	public string SteamId { get => _steamId; set { _steamId = value; OnPropertyChanged(nameof(SteamId)); } }

	private string _appId;
	public string AppId { get => _appId; set { _appId = value; OnPropertyChanged(nameof(AppId)); } }

	private string _packageFamilyName;
	public string PackageFamilyName { get => _packageFamilyName; set { _packageFamilyName = value; OnPropertyChanged(nameof(PackageFamilyName)); } }

	private ImageSource _imgSrc;
	public ImageSource ImageSource { get => _imgSrc; set { _imgSrc = value; OnPropertyChanged(nameof(ImageSource)); } }

	private Visibility _dragAreaVis = Visibility.Collapsed;
	public Visibility DragAreaVis { get => _dragAreaVis; set { _dragAreaVis = value; OnPropertyChanged(nameof(DragAreaVis)); } }

	private Visibility _uwpFieldsVis = Visibility.Collapsed;
	public Visibility UwpFieldsVis { get => _uwpFieldsVis; set { _uwpFieldsVis = value; OnPropertyChanged(nameof(UwpFieldsVis)); } }

	private Visibility _steamFieldsVis = Visibility.Collapsed;
	public Visibility SteamFieldsVis { get => _steamFieldsVis; set { _steamFieldsVis = value; OnPropertyChanged(nameof(SteamFieldsVis)); } }

	public ICommand AddCommand { get; }
	public ICommand BrowseFileCommand { get; }
	public ICommand BrowseImageCommand { get; }
	public ICommand AssociateTagCommand { get; }
	public ICommand AssociateRawgCommand { get; }

	public GameEditionViewModel(Game game, GameList games, List<Tag> tags, MainViewModel mainViewModel)
	{
		_mainViewModel = mainViewModel;

		Game = game;
		GameType = game.GameType;
		Games = games;
		Tags = tags;
		AddCommand = new RelayCommand(AddGame);
		BrowseFileCommand = new RelayCommand(BrowseGame);
		BrowseImageCommand = new RelayCommand(BrowseImage);
		AssociateTagCommand = new RelayCommand(OpenTagPopup);

		// Load properties
		Name = game.Name;
		Description = game.Description;
		CheckIfRunning = game.CheckIfRunning;
		CoverFilePath = game.CoverFilePath;
		Process = game.ProcessName ?? "";
		GameType = game.GameType;
		IsHidden = game.IsHidden;
		SelectedTags = game.Tags ?? new();

		if (!string.IsNullOrEmpty(CoverFilePath))
		{
			BitmapImage image = new(new Uri(CoverFilePath)); // Create the image
			ImageSource = image; // Set the GameImg's source to the image 
		}

		switch (GameType)
		{
			case GameType.UWP:
				string[] appProps = game.Command.Replace(@"shell:appsFolder\", "").Split(new string[] { "!" }, StringSplitOptions.None);
				PackageFamilyName = appProps[0];
				AppId = appProps[1];
				break;

			case GameType.Steam:
				SteamId = game.Command.Replace("steam://rungameid/", "");
				break;
			default:
				Command = game.Command;
				break;
		}

		// Load UI
		DragAreaVis = game.GameType == GameType.Win32 ? Visibility.Visible : Visibility.Collapsed;
		UwpFieldsVis = game.GameType == GameType.UWP ? Visibility.Visible : Visibility.Collapsed;
		SteamFieldsVis = game.GameType == GameType.Steam ? Visibility.Visible : Visibility.Collapsed;
	}

	public GameEditionViewModel(GameType gameType, GameList games, List<Tag> tags, MainViewModel mainViewModel)
	{
		_mainViewModel = mainViewModel;

		GameType = gameType;
		Game = null;
		Games = games;
		Tags = tags;
		AddCommand = new RelayCommand(AddGame);
		BrowseFileCommand = new RelayCommand(BrowseGame);
		BrowseImageCommand = new RelayCommand(BrowseImage);
		AssociateTagCommand = new RelayCommand(OpenTagPopup);
		AssociateRawgCommand = new RelayCommand(OpenRawgPopup);
		SelectedTags = new();

		// Load UI
		DragAreaVis = gameType == GameType.Win32 ? Visibility.Visible : Visibility.Collapsed;
		UwpFieldsVis = gameType == GameType.UWP ? Visibility.Visible : Visibility.Collapsed;
		SteamFieldsVis = gameType == GameType.Steam ? Visibility.Visible : Visibility.Collapsed;
	}

	private void OpenTagPopup(object? obj)
	{
		IsTagOpen = true;
	}

	private void OpenRawgPopup(object? obj)
	{
		IsRawgOpen = true;
	}

	private void BrowseGame(object? obj)
	{
		OpenFileDialog openFileDialog = new()
		{
			Filter = "EXE|*.exe" // Filter
		}; // OpenFileDialog

		if (openFileDialog.ShowDialog() ?? false) // If the user selected a file
		{
			FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(openFileDialog.FileName); // Get the version

			Name = string.IsNullOrEmpty(fileVersionInfo.ProductName) ? Path.GetFileNameWithoutExtension(openFileDialog.FileName) : fileVersionInfo.ProductName; // Name of the file
			Command = openFileDialog.FileName;
		}
	}

	private void BrowseImage(object? obj)
	{
		OpenFileDialog openFileDialog = new()
		{
			Filter = "PNG|*.png|JPG|*.jpg|Bitmap|*.bmp|All Files|*.*" // Filter
		}; // OpenFileDialog

		if (openFileDialog.ShowDialog() ?? false) // If the user selected a file
		{
			try
			{
				BitmapImage image = new(new Uri(openFileDialog.FileName)); // Create the image
				ImageSource = image; // Set the GameImg's source to the image
				CoverFilePath = openFileDialog.FileName; // Set the path to the image
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); // Show the error
			}
		}
	}

	private void AddGame(object? obj)
	{
		if (Game is null)
		{
			string comm = GameType switch
			{
				GameType.Steam => $"steam://rungameid/{SteamId}",
				GameType.UWP => $@"shell:appsFolder\{PackageFamilyName}!{AppId}",
				_ => Command
			};
			Game = new()
			{
				Name = Name,
				Description = Description,
				Command = comm,
				CheckIfRunning = CheckIfRunning,
				CoverFilePath = CoverFilePath,
				ProcessName = Process,
				GameType = GameType,
				IsFavorite = false,
				IsHidden = IsHidden,
				LastTimePlayed = 0,
				TotalTimePlayed = 0,
				Tags = SelectedTags
			};
			Games.Add(Game);
			_mainViewModel.CurrentViewModel = new LibPageViewModel(Games, Tags, _mainViewModel);
			return;
		}
		string comm2 = GameType switch
		{
			GameType.Steam => $"steam://rungameid/{SteamId}",
			GameType.UWP => $@"shell:appsFolder\{PackageFamilyName}!{AppId}",
			_ => Command
		};
		Game.Name = Name;
		Game.Description = Description;
		Game.Command = comm2;
		Game.CheckIfRunning = CheckIfRunning;
		Game.CoverFilePath = CoverFilePath;
		Game.ProcessName = Process;
		Game.GameType = GameType;
		Game.IsHidden = IsHidden;
		Game.Tags = SelectedTags;
		Games[Games.IndexOf(Game)] = Game;
		_mainViewModel.CurrentViewModel = new LibPageViewModel(Games, Tags, _mainViewModel);
	}
}
