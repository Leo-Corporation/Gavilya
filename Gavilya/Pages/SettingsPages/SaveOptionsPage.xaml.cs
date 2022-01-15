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
using Gavilya.Windows;
using LeoCorpLibrary;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Gavilya.Pages.SettingsPages;

/// <summary>
/// Logique d'interaction pour SaveOptionsPage.xaml
/// </summary>
public partial class SaveOptionsPage : Page
{
	public SaveOptionsPage()
	{
		InitializeComponent();
		InitUI();
	}

	private void InitUI()
	{
		// Load auto save settings
		if (Definitions.Settings.MakeAutoSave is null)
		{
			Definitions.Settings.MakeAutoSave = true; // Set value
		}

		if (Definitions.Settings.AutoSaveDay is null)
		{
			Definitions.Settings.AutoSaveDay = 1; // Set value
		}

		if (Definitions.Settings.SavePath is null)
		{
			Definitions.Settings.SavePath = $@"{Env.AppDataPath}\Gavilya\Backups"; // Set value
		}

		SettingsSaver.Save(); // Save changes

		// Combobox
		SaveTime.Items.Clear(); // Clear
		for (int i = 1; i <= 31; i++)
		{
			SaveTime.Items.Add(i); // Add item
		}

		SaveTime.SelectedIndex = Definitions.Settings.AutoSaveDay.Value - 1; // Set

		MakeAutoSavesChk.IsChecked = Definitions.Settings.MakeAutoSave.Value; // Check/Uncheck

		PathTxt.Text = Definitions.Settings.SavePath is null ? Properties.Resources.SelectADirectory : Definitions.Settings.SavePath; // Set
		HandleCheckbox();
	}

	private void HandleCheckbox()
	{
		for (int i = 0; i < AutoSaveSection.Children.Count; i++)
		{
			AutoSaveSection.Children[i].IsEnabled = MakeAutoSavesChk.IsChecked.Value;
		}
	}

	private void ImportButton_Click(object sender, RoutedEventArgs e)
	{
		OpenFileDialog openFileDialog = new(); // Create an OpenFileDialog
		openFileDialog.Filter = $"{Properties.Resources.GavFiles}|*.gav"; // Extension
		openFileDialog.Title = Properties.Resources.ImportGames; // Title

		if (openFileDialog.ShowDialog() ?? true) // If the user opened a file
		{
			if (MessageBox.Show(Properties.Resources.ImportConfirmMsg, Properties.Resources.MainWindowTitle, MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
			{
				GameSaver.Import(openFileDialog.FileName, true); // Import
				new SelectImportGamesWindow().Show(); // Show import assistant
			}
		}
	}

	private void ExportButton_Click(object sender, RoutedEventArgs e)
	{
		SaveFileDialog saveFileDialog = new(); // Create a SaveFileDialog
		saveFileDialog.FileName = $"GavilyaGames_{Definitions.Profiles[Definitions.Settings.CurrentProfileIndex].Name}.gav"; // File name
		saveFileDialog.Filter = $"{Properties.Resources.GavFiles}|*.gav"; // Extension
		saveFileDialog.Title = Properties.Resources.ExportGames; // Title

		if (saveFileDialog.ShowDialog() ?? true)
		{
			string fileLocation = saveFileDialog.FileName; // Location of the file
			GameSaver.Export(Definitions.Games, fileLocation); // Export the games
		}
	}

	private void MakeAutoSavesChk_Checked(object sender, RoutedEventArgs e)
	{
		HandleCheckbox(); // Handle
		Definitions.Settings.MakeAutoSave = MakeAutoSavesChk.IsChecked.Value; // Set
		SettingsSaver.Save(); // Save changes
	}

	private void MakeAutoSavesChk_Unchecked(object sender, RoutedEventArgs e)
	{
		HandleCheckbox(); // Handle
		Definitions.Settings.MakeAutoSave = MakeAutoSavesChk.IsChecked.Value; // Set
		SettingsSaver.Save(); // Save changes
	}

	private void SaveNowBtn_Click(object sender, RoutedEventArgs e)
	{
		if (Definitions.Games.Count > 0 && Directory.Exists(Definitions.Settings.SavePath))
		{
			string fL = $@"{Definitions.Settings.SavePath}\GavilyaGames_{Definitions.Profiles[Definitions.Settings.CurrentProfileIndex].Name}_{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.gav";
			GameSaver.Export(Definitions.Games, fL); // Export 
		}
	}

	private void Browse_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			SaveFileDialog saveFileDialog = new(); // Create a SaveFileDialog
			saveFileDialog.FileName = $@"{Definitions.Settings.SavePath}\GavilyaGames_{Definitions.Profiles[Definitions.Settings.CurrentProfileIndex].Name}_{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.gav"; // File name
			saveFileDialog.Filter = $"{Properties.Resources.GavFiles}|*.gav"; // Extension
			saveFileDialog.Title = Properties.Resources.SaveLocation; // Title

			if (saveFileDialog.ShowDialog() ?? true)
			{
				string fileLocation = System.IO.Path.GetDirectoryName(saveFileDialog.FileName); // Location of the file
				Definitions.Settings.SavePath = fileLocation; // Set
				SettingsSaver.Save();
				InitUI();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}

	private void SaveTime_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		try
		{
			Definitions.Settings.AutoSaveDay = (int)SaveTime.SelectedItem; // Set
			SettingsSaver.Save();
		}
		catch { }
	}
}
