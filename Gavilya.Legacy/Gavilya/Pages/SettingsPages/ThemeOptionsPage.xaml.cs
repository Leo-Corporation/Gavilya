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
using Microsoft.Win32;
using PeyrSharp.Env;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace Gavilya.Pages.SettingsPages;
/// <summary>
/// Interaction logic for ThemeOptionsPage.xaml
/// </summary>
public partial class ThemeOptionsPage : Page
{
	private List<(ThemeInfo, string)> InstalledThemes { get; set; }
	public ThemeOptionsPage()
	{
		InitializeComponent();
		InitUI();
	}

	private void InitUI()
	{
		// Clear UI
		ThemesComboBox.SelectionChanged -= ThemesComboBox_SelectionChanged;
		ThemesComboBox.Items.Clear();

		// 1. Get a list of installed themes
		InstalledThemes = ThemeManager.GetInstalledThemes();

		// 2. Display the available themes
		int s = 0;
		var selectedTheme = (Global.Settings.ThemePath != "_default") ? ThemeManager.GetThemeInfoFromPath(Global.Settings.ThemePath) : null;

		for (int i = 0; i < InstalledThemes.Count; i++)
		{
			ThemesComboBox.Items.Add(InstalledThemes[i].Item1.Name);
			if (InstalledThemes[i].Item1.Equals(selectedTheme)) s = i;
		}
		ThemesComboBox.SelectedIndex = s;
		// 3. Change theme when selection changed
		ThemesComboBox.SelectionChanged += ThemesComboBox_SelectionChanged;
	}

	private void ThemesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		try
		{
			if (ThemesComboBox.SelectedIndex == 0) // If the default theme is selected
			{
				Application.Current.Resources.MergedDictionaries.Clear();
				ResourceDictionary resourceDictionary = new(); // Create a resource dictionary
				resourceDictionary.Source = new Uri("..\\Themes\\Dark.xaml", UriKind.Relative); // Add source
				Application.Current.Resources.MergedDictionaries.Add(resourceDictionary); // Add the dictionary

				Global.Settings.ThemePath = "_default";
				SettingsSaver.Save();
				return;
			}
			ThemeManager.ChangeTheme(InstalledThemes[ThemesComboBox.SelectedIndex].Item1, InstalledThemes[ThemesComboBox.SelectedIndex].Item2);
			Global.Settings.ThemePath = InstalledThemes[ThemesComboBox.SelectedIndex].Item2 + $@"\theme.manifest";
			SettingsSaver.Save();
		}
		catch (IndexOutOfRangeException)
		{
			
		}
	}

	private void ImportButton_Click(object sender, RoutedEventArgs e)
	{
		OpenFileDialog openFileDialog = new()
		{
			DefaultExt = ".gavtheme",
			Filter = $"{Properties.Resources.Themes}|*.gavtheme",
			Title = Properties.Resources.ThemesImport
		};

		if (openFileDialog.ShowDialog() ?? false)
		{
			ThemeManager.InstallTheme(openFileDialog.FileName);
			InitUI();
		}
	}
}
