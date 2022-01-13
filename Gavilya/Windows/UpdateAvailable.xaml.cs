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
using Gavilya.Classes;
using LeoCorpLibrary;
using System;
using System.IO;
using System.Windows;

namespace Gavilya.Windows;

/// <summary>
/// Logique d'interaction pour UpdateAvailable.xaml
/// </summary>
public partial class UpdateAvailable : Window
{
	public UpdateAvailable()
	{
		InitializeComponent();
		DisplayLastVersion();
	}

	private async void DisplayLastVersion()
	{
		VersionTxt.Text = $"{Properties.Resources.UpdateVersion} {await Update.GetLastVersionAsync(Definitions.LastVersionLink)}"; // Show the last version
	}

	private void CloseBtn_Click(object sender, RoutedEventArgs e)
	{
		Close(); // Close the window
	}

	private void InstallBtn_Click(object sender, RoutedEventArgs e)
	{
		if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\Xalyus Updater.exe")) // If Xalyus Updater exist
		{
			Env.ExecuteAsAdmin(AppDomain.CurrentDomain.BaseDirectory + @"\Xalyus Updater.exe"); // Launch the updater
			Environment.Exit(0); // Close Gavilya
		}
		else
		{
			MessageBox.Show(Properties.Resources.XUDoesNotExist, "ERROR_FILE_NOT_FOUND (0x2)", MessageBoxButton.OK, MessageBoxImage.Error); // Show error
		}
	}

	private void CancelBtn_Click(object sender, RoutedEventArgs e)
	{
		Close(); // Close the window
	}
}
