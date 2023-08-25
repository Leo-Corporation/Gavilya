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
using Gavilya.Models;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace Gavilya.ViewModels;
public class MainViewModel : ViewModelBase
{
	private NavBarViewModel _navBarViewModel;
	public NavBarViewModel Nav { get => _navBarViewModel; set { _navBarViewModel = value; OnPropertyChanged(); } }

	private object _currentView;
	private readonly Window _window;
	private readonly List<Tag> tags;
	private readonly WindowHelper _windowHelper;

	public GameList Games { get; set; }

	public object CurrentViewModel
	{
		get { return _currentView; }
		set { _currentView = value; OnPropertyChanged(nameof(CurrentViewModel)); }
	}

	private string _maxiIcon = "\uFA40";
	public string MaxiIcon
	{
		get => _maxiIcon;
		set { _maxiIcon = value; OnPropertyChanged(nameof(MaxiIcon)); }
	}

	private double _maxiIconFontSize = 12;
	public double MaxiIconFontSize
	{
		get => _maxiIconFontSize;
		set { _maxiIconFontSize = value; OnPropertyChanged(nameof(MaxiIconFontSize)); }
	}

	private double _maxHeight;
	private double _maxWidth;

	public double MaxHeight { get => _maxHeight; set { _maxHeight = value; OnPropertyChanged(nameof(MaxHeight)); } }
	public double MaxWidth { get => _maxWidth; set { _maxWidth = value; OnPropertyChanged(nameof(MaxWidth)); } }

	private Thickness _borderMargin = new(10);
	public Thickness BorderMargin { get => _borderMargin; set { _borderMargin = value; OnPropertyChanged(nameof(BorderMargin)); } }

	public ICommand MinimizeCommand { get; }
	public ICommand MaximizeRestoreCommand { get; }
	public ICommand CloseCommand { get; }

	public MainViewModel(Window window, GameList games, List<Tag> _tags)
	{
		_window = window;

		Games = games;
		tags = _tags;
		_navBarViewModel = new(this, Games, _tags);
		_windowHelper = new(window);

		MinimizeCommand = new RelayCommand(Minimize);
		MaximizeRestoreCommand = new RelayCommand(Maximize);
		CloseCommand = new RelayCommand(Close);

		(MaxHeight, MaxWidth) = _windowHelper.GetMaximumSize();

		_window.StateChanged += (o, e) =>
		{
			(MaxHeight, MaxWidth) = _windowHelper.GetMaximumSize();
			MaxiIcon = _window.WindowState == WindowState.Maximized ? "\uF670" : "\uFA40";
			MaxiIconFontSize = _window.WindowState == WindowState.Maximized ? 16 : 12;
			BorderMargin = _window.WindowState == WindowState.Maximized ? new(0) : new(10); // Set
		};

		_window.LocationChanged += (o, e) =>
		{
			(MaxHeight, MaxWidth) = _windowHelper.GetMaximumSize();
		};
	}

	private void Minimize(object parameter)
	{
		_window.WindowState = WindowState.Minimized;
	}

	private void Maximize(object parameter)
	{
		if (_window.WindowState == WindowState.Maximized)
		{
			_window.WindowState = WindowState.Normal;
			MaxiIcon = "\uFA40";
			MaxiIconFontSize = 12;
		}
		else
		{
			(MaxHeight, MaxWidth) = _windowHelper.GetMaximumSize();
			_window.WindowState = WindowState.Maximized;
			MaxiIcon = "\uF670";
			MaxiIconFontSize = 16;
		}
	}

	private void Close(object parameter)
	{
		_window.Close();
	}
}
