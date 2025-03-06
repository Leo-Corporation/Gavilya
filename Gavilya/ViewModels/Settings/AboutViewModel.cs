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
using PeyrSharp.Core;
using PeyrSharp.Env;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Gavilya.ViewModels.Settings;

public class AboutViewModel : ViewModelBase
{
	private readonly ProfileData _profileData;

	public static string Version => Context.Version;

	private string _statusMessage = Properties.Resources.UpdateUn;
	public string StatusMessage { get => _statusMessage; set { _statusMessage = value; OnPropertyChanged(nameof(StatusMessage)); } }

	private string _statusIcon = "\uF299";
	public string StatusIcon { get => _statusIcon; set { _statusIcon = value; OnPropertyChanged(nameof(StatusIcon)); } }

	private string _checkUpdateBtnContent = "\uF191";
	public string CheckUpdateBtnContent { get => _checkUpdateBtnContent; set { _checkUpdateBtnContent = value; OnPropertyChanged(nameof(CheckUpdateBtnContent)); } }

	private FontFamily _iconFont = new(new Uri("pack://application:,,,/"), "./Fonts/#FluentSystemIcons-Regular");
	public FontFamily IconFont { get => _iconFont; set { _iconFont = value; OnPropertyChanged(nameof(IconFont)); } }

	private int _iconSize = 14;
	public int IconSize { get => _iconSize; set { _iconSize = value; OnPropertyChanged(nameof(IconSize)); } }

	private FontWeight _fontWeight = FontWeights.Normal;
	public FontWeight FontWeight { get => _fontWeight; set { _fontWeight = value; OnPropertyChanged(nameof(FontWeight)); } }

	private SolidColorBrush _foregroundColor = ThemeHelper.GetSolidColor("ForegroundGreen");
	public SolidColorBrush ForegroundColor { get => _foregroundColor; set { _foregroundColor = value; OnPropertyChanged(nameof(ForegroundColor)); } }

	private SolidColorBrush _backgroundColor = ThemeHelper.GetSolidColor("LightGreen");
	public SolidColorBrush BackgroundColor { get => _backgroundColor; set { _backgroundColor = value; OnPropertyChanged(nameof(BackgroundColor)); } }


	public ICommand UpdateCommand { get; }
	public ICommand LicensesCommand { get; }
	public ICommand OpenRepoCommand { get; }
	public AboutViewModel(ProfileData profileData)
	{
		UpdateCommand = new RelayCommand(CheckUpdate);
		LicensesCommand = new RelayCommand(Licenses);
		OpenRepoCommand = new RelayCommand(OpenRepo);
		_profileData = profileData;
	}

	private async void CheckUpdate(object? obj)
	{
		System.Windows.Forms.NotifyIcon notifyIcon = new();

		if (await Internet.IsAvailableAsync())
		{
			var lastVer = await Update.GetLastVersionAsync(Context.LastVersionLink);

			if (Update.IsAvailable(lastVer, Context.Version))
			{
				StatusMessage = Properties.Resources.UpdateAv;
				StatusIcon = "\uF36E";
				ForegroundColor = ThemeHelper.GetSolidColor("ForegroundOrange");
				BackgroundColor = ThemeHelper.GetSolidColor("LightOrange");
				CheckUpdateBtnContent = Properties.Resources.Install;
				IconFont = new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/#Hauora");
				IconSize = 12;
				FontWeight = FontWeights.Bold;

				if (MessageBox.Show($"{Properties.Resources.UpdateAvMessage}\n{Properties.Resources.UpdateVersion} {lastVer}\n\n{Properties.Resources.ContinueInstall}", $"{Properties.Resources.Version} {lastVer}", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.No)
				{
					return;
				}

				_profileData.Save();
				Sys.ExecuteAsAdmin(Directory.GetCurrentDirectory() + @"\Xalyus Updater.exe"); // Start the updater
				Application.Current.Shutdown(); // Close
			}

			StatusMessage = Properties.Resources.UpdateUn;
			StatusIcon = "\uF299";
			ForegroundColor = ThemeHelper.GetSolidColor("ForegroundGreen");
			BackgroundColor = ThemeHelper.GetSolidColor("LightGreen");
			CheckUpdateBtnContent = "\uF191";
			IconFont = new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/#FluentSystemIcons-Regular");
			IconSize = 14;
			FontWeight = FontWeights.Normal;
		}
	}

	private void Licenses(object? obj)
	{
		MessageBox.Show($"{Properties.Resources.Licenses}\n\n" +
			"Fluent System Icons - MIT License - © 2020 Microsoft Corporation\n" +
			"System.Drawing.Common - MIT License - © .NET Foundation and Contributors\n" +
			"PeyrSharp - MIT License - © 2022-2024 Devyus\n" +
			"RestSharp - Apache License 2.0 - © RestSharp\n" +
			"Gavilya - MIT License - © 2020-2024 Léo Corporation", $"{Properties.Resources.MainWindowTitle} - {Properties.Resources.Licenses}", MessageBoxButton.OK, MessageBoxImage.Information);
	}

	private void OpenRepo(object? obj)
	{
		Process.Start("explorer.exe", "https://github.com/Leo-Corporation/Gavilya"); // Open the GitHub repository in the default browser
	}
}
