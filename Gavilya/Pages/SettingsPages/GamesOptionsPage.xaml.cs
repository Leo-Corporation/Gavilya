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
using Gavilya.UserControls;
using PeyrSharp.Core.Converters;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Gavilya.Pages.SettingsPages;
/// <summary>
/// Interaction logic for GamesOptionsPage.xaml
/// </summary>
public partial class GamesOptionsPage : Page
{
	string hexColor = "";
	public GamesOptionsPage()
	{
		InitializeComponent();
		InitUI();
	}

	internal void InitUI()
	{
		// Load tags
		TagsDisplayer.Children.Clear();
		NameTextBox.Text = "";
		for (int i = 0; i < Definitions.Settings.GameTags.Count; i++)
		{
			TagsDisplayer.Children.Add(new EditTagItem(Definitions.Settings.GameTags[i], i, this));
		}

		// Generate a random color
		int r = new Random().Next(0, 255);
		int g = new Random().Next(0, 255);
		int b = new Random().Next(0, 255);
		ForegroundBorder.Background = new SolidColorBrush { Color = Color.FromRgb((byte)r, (byte)g, (byte)b) }; // Set color

		hexColor = new RGB(r, g, b).ToHex().Value;
	}

	private void AddBtn_Click(object sender, RoutedEventArgs e)
	{
		if (string.IsNullOrEmpty(NameTextBox.Text))
		{
			MessageBox.Show(Properties.Resources.NameNeededMsg, Properties.Resources.ManageGameTags, MessageBoxButton.OK, MessageBoxImage.Information);
			return;
		}
		Definitions.Settings.GameTags.Add(new(NameTextBox.Text, hexColor));
		SettingsSaver.Save();

		InitUI();
	}

	private void ForegroundBorder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		System.Windows.Forms.ColorDialog colorDialog = new()
		{
			AllowFullOpen = true,
		}; // Create color picker/dialog

		if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) // If the user selected a color
		{
			var color = new SolidColorBrush { Color = Color.FromRgb(colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B) }; // Set color

			RGB rgb = new(colorDialog.Color);
			hexColor = rgb.ToHex().Value;

			ForegroundBorder.Background = color;
		}
	}

	private void ForegroundBorder_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
	{
		// Generate a random color
		int r = new Random().Next(0, 255);
		int g = new Random().Next(0, 255);
		int b = new Random().Next(0, 255);
		ForegroundBorder.Background = new SolidColorBrush { Color = Color.FromRgb((byte)r, (byte)g, (byte)b) }; // Set color
		hexColor = new RGB(r, g, b).ToHex().Value;
	}

	private void DisplayHiddenChk_Unchecked(object sender, RoutedEventArgs e)
	{
		Definitions.DisplayHiddenGames = DisplayHiddenChk.IsChecked ?? false;
		Global.ReloadAllPages();
		Definitions.ProfilePage.InitUI();
		Definitions.HomePage.InitUI();
	}
}
