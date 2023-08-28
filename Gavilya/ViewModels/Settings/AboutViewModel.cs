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
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace Gavilya.ViewModels.Settings;
public class AboutViewModel : ViewModelBase
{
	private readonly ProfileData _profileData;

	public string Version => Context.Version;

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
				if (MessageBox.Show($"{Properties.Resources.UpdateAvMessage}\n{Properties.Resources.UpdateVersion} {lastVer}\n\n{Properties.Resources.ContinueInstall}", $"{Properties.Resources.Version} {lastVer}", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.No)
				{
					return;
				}
				_profileData.Save();
				Sys.ExecuteAsAdmin(Directory.GetCurrentDirectory() + @"\Xalyus Updater.exe"); // Start the updater
				Application.Current.Shutdown(); // Close
			}
		}
	}

	private void Licenses(object? obj)
	{
		MessageBox.Show($"{Properties.Resources.Licenses}\n\n" +
			"Fluent System Icons - MIT License - © 2020 Microsoft Corporation\n" +
			"System.Drawing.Common - MIT License - © .NET Foundation and Contributors\n" +
			"PeyrSharp - MIT License - © 2022-2023 Léo Corporation\n" +
			"RestSharp - Apache License 2.0 - © RestSharp\n" +
			"Gavilya - MIT License - © 2020-2023 Léo Corporation", $"{Properties.Resources.MainWindowTitle} - {Properties.Resources.Licenses}", MessageBoxButton.OK, MessageBoxImage.Information);
	}

	private void OpenRepo(object? obj)
	{
		Process.Start("explorer.exe", "https://github.com/Leo-Corporation/Gavilya"); // Open the GitHub repository in the default browser
	}
}
