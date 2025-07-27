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
using Gavilya.Helpers;
using Gavilya.Models;
using Gavilya.Models.Rawg;
using Gavilya.Services;
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
	GameType GameType { get; set; }
	GameList Games { get; set; }
	public List<Tag> Tags { get; }
	public List<Tag> SelectedTags { get; set; }
	public List<TagSelectorViewModel> TagsVm => [.. Tags.Select(t => new TagSelectorViewModel(SelectedTags, t, SelectedTags.Contains(t)))];

	private List<RawgResultViewModel> _searchResults;
	public List<RawgResultViewModel> SearchResults { get => _searchResults; set { _searchResults = value; OnPropertyChanged(nameof(SearchResults)); } }

	private int _rawgId = -1;
	public int RawgId { get => _rawgId; set { _rawgId = value; AssociatedVis = value != -1 ? Visibility.Visible : Visibility.Collapsed; OnPropertyChanged(nameof(RawgId)); } }

	private Visibility _associatedVis = Visibility.Collapsed;
	public Visibility AssociatedVis { get => _associatedVis; set { _associatedVis = value; OnPropertyChanged(nameof(AssociatedVis)); } }

	private string _name;
	public string Name
	{
		get => _name;
		set
		{
			_name = value;
			OnPropertyChanged(nameof(Name));
			CanExecute = !string.IsNullOrEmpty(value);
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

	private bool _canExecute;
	public bool CanExecute
	{
		get { return _canExecute; }
		set
		{
			_canExecute = value;
			OnPropertyChanged(nameof(CanExecute));
		}
	}

	private bool _isUwpOpen = false;
	public bool IsUwpOpen { get => _isUwpOpen; set { _isUwpOpen = value; OnPropertyChanged(nameof(IsUwpOpen)); } }

	private bool _isExeSelectorOpen = false;
	public bool IsExeSelectorOpen { get => _isExeSelectorOpen; set { _isExeSelectorOpen = value; OnPropertyChanged(nameof(IsExeSelectorOpen)); } }


	private bool _isRawgOpen = false;
	public bool IsRawgOpen { get => _isRawgOpen; set { _isRawgOpen = value; OnPropertyChanged(nameof(IsRawgOpen)); } }

	private string _convertSteamId;
	public string ConvertSteamId { get => _convertSteamId; set { _convertSteamId = value; OnPropertyChanged(nameof(ConvertSteamId)); } }

	private string _convertSteamProcess;
	public string ConvertSteamProcess { get => _convertSteamProcess; set { _convertSteamProcess = value; OnPropertyChanged(nameof(ConvertSteamProcess)); } }

	private string _rawgSearchQuery;
	public string RawgSearchQuery { get => _rawgSearchQuery; set { _rawgSearchQuery = value; OnPropertyChanged(nameof(RawgSearchQuery)); } }

	private string _steamId;
	public string SteamId { get => _steamId; set { _steamId = value; OnPropertyChanged(nameof(SteamId)); } }

	private string _appId;
	public string AppId { get => _appId; set { _appId = value; OnPropertyChanged(nameof(AppId)); } }

	private string _packageFamilyName;
	public string PackageFamilyName { get => _packageFamilyName; set { _packageFamilyName = value; OnPropertyChanged(nameof(PackageFamilyName)); } }

	private string _applyBtnString;
	public string ApplyBtnString { get => _applyBtnString; set { _applyBtnString = value; OnPropertyChanged(nameof(ApplyBtnString)); } }

	private ImageSource _imgSrc;
	public ImageSource ImageSource { get => _imgSrc; set { _imgSrc = value; OnPropertyChanged(nameof(ImageSource)); } }

	private List<UwpSelectorViewModel> _uwpApps;
	public List<UwpSelectorViewModel> UwpApps { get => _uwpApps; set { _uwpApps = value; OnPropertyChanged(nameof(UwpApps)); } }

	private List<ExecutableViewModel> _exeApps;
	public List<ExecutableViewModel> ExeApps { get => _exeApps; set { _exeApps = value; OnPropertyChanged(nameof(ExeApps)); } }

	private Visibility _dragAreaVis = Visibility.Collapsed;
	public Visibility DragAreaVis { get => _dragAreaVis; set { _dragAreaVis = value; OnPropertyChanged(nameof(DragAreaVis)); } }

	private Visibility _uwpFieldsVis = Visibility.Collapsed;
	public Visibility UwpFieldsVis { get => _uwpFieldsVis; set { _uwpFieldsVis = value; OnPropertyChanged(nameof(UwpFieldsVis)); } }

	/// <summary>
	/// The section with the Steam App ID.
	/// </summary>
	private Visibility _steamFieldsVis = Visibility.Collapsed;
	public Visibility SteamFieldsVis { get => _steamFieldsVis; set { _steamFieldsVis = value; OnPropertyChanged(nameof(SteamFieldsVis)); } }


	/// <summary>
	/// The section with the Steam Convert Assistant field.
	/// </summary>
	private Visibility _convertSteam = Visibility.Collapsed;
	public Visibility ConvertSteamVis { get => _convertSteam; set { _convertSteam = value; OnPropertyChanged(nameof(ConvertSteamVis)); } }

	/// <summary>
	/// The section with the Convert Steam  button.
	/// </summary>
	private Visibility _steamSectionVis = Visibility.Collapsed;
	public Visibility SteamSectionVis { get => _steamSectionVis; set { _steamSectionVis = value; OnPropertyChanged(nameof(SteamSectionVis)); } }

	private Visibility _noExeVis = Visibility.Collapsed;
	public Visibility NoExeVis { get => _noExeVis; set { _noExeVis = value; OnPropertyChanged(nameof(NoExeVis)); } }

	private int _monitorIndex = 0;
	public int MonitorIndex { get => _monitorIndex; set { _monitorIndex = value; OnPropertyChanged(nameof(MonitorIndex)); } }

	private List<Monitor> _monitors => DesktopMonitorHelper.GetMonitors();
	public List<Monitor> Monitors { get => _monitors; }

	public ICommand AddCommand { get; }
	public ICommand BrowseFileCommand { get; }
	public ICommand BrowseImageCommand { get; }
	public ICommand BrowseUwpCommand { get; }
	public ICommand AssociateTagCommand { get; }
	public ICommand AssociateRawgCommand { get; }
	public ICommand RawgSearchCommand { get; }
	public ICommand ShowConvertSectionCommand { get; }
	public ICommand ProcessHelpCommand { get; }
	public ICommand DropCommand { get; }
	public ICommand ScanCommand { get; }
	public ICommand CloseExeSelectorCommand { get; }

	/// <summary>
	/// This constructor is used when editing an exisiting game.
	/// </summary>
	/// <param name="game"></param>
	/// <param name="games"></param>
	/// <param name="tags"></param>
	/// <param name="mainViewModel"></param>
	public GameEditionViewModel(Game game, GameList games, List<Tag> tags, MainViewModel mainViewModel)
	{
		_mainViewModel = mainViewModel;

		Game = game;
		GameType = game.GameType;
		Games = games;
		Tags = tags;

		AddCommand = new RelayCommand(AddGame, (o) => CanExecute);
		BrowseFileCommand = new RelayCommand(BrowseGame);
		BrowseImageCommand = new RelayCommand(BrowseImage);
		BrowseUwpCommand = new RelayCommand(BrowseUwp);
		AssociateTagCommand = new RelayCommand(OpenTagPopup);
		AssociateRawgCommand = new RelayCommand(OpenRawgPopup);
		RawgSearchCommand = new RelayCommand(SearchRawg);
		ShowConvertSectionCommand = new RelayCommand(ShowConvert);
		ProcessHelpCommand = new RelayCommand(ShowProcessHelp);
		DropCommand = new RelayCommand(ExecuteDrop);
		ScanCommand = new RelayCommand(Scan);
		CloseExeSelectorCommand = new RelayCommand((o) => IsExeSelectorOpen = false);

		// Load properties
		Name = game.Name;
		Description = game.Description;
		CheckIfRunning = game.CheckIfRunning;
		CoverFilePath = game.CoverFilePath;
		Process = game.ProcessName ?? "";
		GameType = game.GameType;
		IsHidden = game.IsHidden;
		RawgId = game.RawgId;
		SelectedTags = game.Tags ?? [];

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
				SteamSectionVis = Visibility.Visible;
				break;
		}

		// Load UI
		DragAreaVis = game.GameType == GameType.Win32 ? Visibility.Visible : Visibility.Collapsed;
		UwpFieldsVis = game.GameType == GameType.UWP ? Visibility.Visible : Visibility.Collapsed;
		SteamFieldsVis = game.GameType == GameType.Steam ? Visibility.Visible : Visibility.Collapsed;

		ApplyBtnString = Properties.Resources.EditGame;

		// Monitor ComboBox
		MonitorIndex = Game.DefaultMonitor is not null && Monitors.IndexOf(Game.DefaultMonitor) != -1 ? Monitors.IndexOf(Game.DefaultMonitor) : 0;
	}

	/// <summary>
	/// This constructor is used when adding a game for the first time.
	/// </summary>
	/// <param name="gameType"></param>
	/// <param name="games"></param>
	/// <param name="tags"></param>
	/// <param name="mainViewModel"></param>
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
		BrowseUwpCommand = new RelayCommand(BrowseUwp);
		AssociateTagCommand = new RelayCommand(OpenTagPopup);
		AssociateRawgCommand = new RelayCommand(OpenRawgPopup);
		RawgSearchCommand = new RelayCommand(SearchRawg);
		ProcessHelpCommand = new RelayCommand(ShowProcessHelp);
		DropCommand = new RelayCommand(ExecuteDrop);
		ScanCommand = new RelayCommand(Scan);
		CloseExeSelectorCommand = new RelayCommand((o) => IsExeSelectorOpen = false);

		SelectedTags = [];

		// Load UI
		DragAreaVis = gameType == GameType.Win32 ? Visibility.Visible : Visibility.Collapsed;
		UwpFieldsVis = gameType == GameType.UWP ? Visibility.Visible : Visibility.Collapsed;
		SteamFieldsVis = gameType == GameType.Steam ? Visibility.Visible : Visibility.Collapsed;

		ApplyBtnString = Properties.Resources.AddGame;
	}

	private void ShowConvert(object? obj)
	{
		if (ConvertSteamVis == Visibility.Visible && !string.IsNullOrEmpty(ConvertSteamId))
		{
			if (MessageBox.Show(Properties.Resources.ConvertToSteamMsg, Properties.Resources.ConvertToSteam, MessageBoxButton.YesNoCancel, MessageBoxImage.Question) == MessageBoxResult.Yes)
			{
				SteamId = ConvertSteamId;
				Process = string.IsNullOrEmpty(ConvertSteamProcess) ? Process : ConvertSteamProcess;
				GameType = GameType.Steam;

				Game.Command = $"steam://rungameid/{ConvertSteamId}";
				Game.ProcessName = string.IsNullOrEmpty(ConvertSteamProcess) ? Game.ProcessName : ConvertSteamProcess;
				Game.GameType = GameType.Steam;
				SteamSectionVis = Visibility.Collapsed;
				SteamFieldsVis = Visibility.Visible;
				DragAreaVis = Visibility.Collapsed;
			}
		}

		ConvertSteamVis = Visibility.Visible;
	}

	private void ExecuteDrop(object parameter)
	{
		if (parameter is not IDataObject ido) return;
		string[] files = (string[])ido.GetData("FileName");

		if (files.Length > 0)
		{
			string filePath = files[0]; // Get the first dropped file
			if (Path.GetExtension(filePath).Equals(".exe", StringComparison.OrdinalIgnoreCase))
			{
				Command = filePath;
			}
		}
	}

	private async void BrowseUwp(object? obj)
	{
		IsUwpOpen = true;
		UwpApps = [.. (await UwpHelper.GetUwpAppsAsync()).Select(a => new UwpSelectorViewModel(a, this))];
	}

	private void OpenTagPopup(object? obj)
	{
		IsTagOpen = true;
	}

	private void OpenRawgPopup(object? obj)
	{
		IsRawgOpen = true;
	}

	private async void SearchRawg(object? obj)
	{
		var results = await new RawgClient(RawgSearchQuery).GetResultsAsync();
		SearchResults = results?.Results.Select(g => new RawgResultViewModel(g, this)).ToList() ?? [];
	}

	private void BrowseGame(object? obj)
	{
		// OpenFileDialog
		OpenFileDialog openFileDialog = new()
		{
			Filter = "EXE|*.exe" // Filter
		};

		// If the user selected a file
		if (openFileDialog.ShowDialog() ?? false)
		{
			FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(openFileDialog.FileName); // Get the version

			Name = string.IsNullOrEmpty(fileVersionInfo.ProductName) ? Path.GetFileNameWithoutExtension(openFileDialog.FileName) : fileVersionInfo.ProductName; // Name of the file
			Command = openFileDialog.FileName;
		}
	}

	private void BrowseImage(object? obj)
	{
		// OpenFileDialog
		OpenFileDialog openFileDialog = new()
		{
			Filter = "PNG|*.png|JPG|*.jpg|Bitmap|*.bmp|All Files|*.*" // Filter
		};

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
				Tags = SelectedTags,
				RawgId = RawgId,
				DefaultMonitor = Monitors[MonitorIndex]
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
		Game.RawgId = RawgId;
		Game.DefaultMonitor = Monitors[MonitorIndex];
		Games[Games.IndexOf(Game)] = Game;
		_mainViewModel.CurrentViewModel = new LibPageViewModel(Games, Tags, _mainViewModel);
	}

	private void ShowProcessHelp(object? obj)
	{
		MessageBox.Show(Properties.Resources.ProcessNameHelp, Properties.Resources.Help, MessageBoxButton.OK, MessageBoxImage.Information);
	}

	private void Scan(object? obj)
	{
		using System.Windows.Forms.FolderBrowserDialog dialog = new();
		System.Windows.Forms.DialogResult result = dialog.ShowDialog();

		if (result == System.Windows.Forms.DialogResult.OK)
		{
			string selectedPath = dialog.SelectedPath;
			GameScannerService gameScannerService = new();
			ExeApps = GameScannerService.ScanForExecutables(selectedPath, this);
			NoExeVis = ExeApps is not null && ExeApps.Count > 0 ? Visibility.Collapsed : Visibility.Visible;
			IsExeSelectorOpen = true;
		}
	}
}
