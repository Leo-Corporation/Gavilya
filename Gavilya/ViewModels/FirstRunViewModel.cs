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
using Gavilya.ViewModels.FirstRun;
using System.Windows;
using System.Windows.Input;

namespace Gavilya.ViewModels;

public class FirstRunViewModel : ViewModelBase
{
	private readonly Window _window;
	private readonly Profile _profile;
	private readonly ProfileData _profileData;
	private readonly WindowHelper _windowHelper;

	private ViewModelBase _currentViewModel;
	public ViewModelBase CurrentViewModel { get => _currentViewModel; set { _currentViewModel = value; OnPropertyChanged(nameof(CurrentViewModel)); } }

	public ICommand MinimizeCommand { get; }
	public ICommand CloseCommand { get; }

	public FirstRunViewModel(Window window, Profile profile, ProfileData profileData)
	{
		_window = window;
		_profile = profile;
		_profileData = profileData;

		_windowHelper = new(window);

		CurrentViewModel = new WelcomeViewModel(this, _profile, _profileData);

		MinimizeCommand = new RelayCommand(Minimize);
		CloseCommand = new RelayCommand(Close);
	}

	private void Minimize(object parameter)
	{
		_window.WindowState = WindowState.Minimized;
	}

	private void Close(object parameter)
	{
		_window.Close();
	}
}
