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
using System.Windows;

namespace Gavilya.Windows;

/// <summary>
/// Logique d'interaction pour About.xaml
/// </summary>
public partial class About : Window
{
	public About()
	{
		InitializeComponent();
		VersionTxt.Text = !Definitions.IsBeta ? Definitions.Version : Definitions.BetaVersion; // Display the current version
	}

	private void CloseBtn_Click(object sender, RoutedEventArgs e)
	{
		Close(); // Close the window
	}

	private async void UpdateBtn_Click(object sender, RoutedEventArgs e)
	{
		string lastVersion = await Update.GetLastVersionAsync(Definitions.LastVersionLink); // Last version of Gavilya
		if (Update.IsAvailable(Definitions.Version, lastVersion)) // If updates are available
		{
			new UpdateAvailable().Show(); // Show the updates available window
		}
		else
		{
			new NoUpdateAvailable().Show(); // Show the no updates available window
		}
	}

	private void LicenseBtn_Click(object sender, RoutedEventArgs e)
	{
		MessageBox.Show($"{Properties.Resources.Licenses}\n\n" +
			"Fluent System Icons - MIT License - © 2020 Microsoft Corporation\n" +
			"System.Drawing.Common - MIT License - © .NET Foundation and Contributors\n" +
			"LeoCorpLibrary - MIT License - © 2020-2022 Léo Corporation\n" +
			"RestSharp - Apache License 2.0 - © RestSharp\n" +
			"Gavilya - MIT License - © 2020-2022 Léo Corporation", $"{Properties.Resources.MainWindowTitle} - {Properties.Resources.Licenses}", MessageBoxButton.OK, MessageBoxImage.Information);
	}
}
