﻿/*
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
using Gavilya.Enums;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Gavilya.Pages.SettingsPages;
/// <summary>
/// Interaction logic for HomeOptionsPage.xaml
/// </summary>
public partial class HomeOptionsPage : Page
{
	public HomeOptionsPage()
	{
		InitializeComponent();
		InitUI();
	}

	private void InitUI()
	{
		NumberRecentGamesTextBox.Text = Global.Settings.MaxNumberRecentGamesShown.Value.ToString();
		DisplayedUnusedGamesChk.IsChecked = Global.Settings.ShowMoreUnplayedGamesRecommanded; // Change the check state
		PositionCombobox.SelectedIndex = (int)Global.Settings.SidebarPosition;
	}

	private void SaveButton_Click(object sender, RoutedEventArgs e)
	{
		if (Global.Settings.MaxNumberRecentGamesShown.Value > 0)
		{
			Global.Settings.MaxNumberRecentGamesShown = int.Parse(NumberRecentGamesTextBox.Text);
		}
		else
		{
			MessageBox.Show(Properties.Resources.InvalidGameNumber, Properties.Resources.MainWindowTitle, MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}

	private void NumberRecentGamesTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
	{
		Regex regex = new("[^0-9]+");
		e.Handled = regex.IsMatch(e.Text);
	}

	private void DisplayedUnusedGamesChk_Checked(object sender, RoutedEventArgs e)
	{
		Global.Settings.ShowMoreUnplayedGamesRecommanded = DisplayedUnusedGamesChk.IsChecked; // Set value
		SettingsSaver.Save(); // Save the settings
	}

	private void PositionCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		Global.Settings.SidebarPosition = (Position)PositionCombobox.SelectedIndex;
		SettingsSaver.Save();
		if (Global.MainWindow is null) return;
		Grid.SetColumn(Global.MainWindow.Sidebar, Global.Settings.SidebarPosition switch
		{
			Position.Right => 3,
			_ => 0
		});
	}
}
