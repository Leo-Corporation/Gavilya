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
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Gavilya.Pages.SettingsPages;
/// <summary>
/// Interaction logic for SearchOptionsPage.xaml
/// </summary>
public partial class SearchOptionsPage : Page
{
	public SearchOptionsPage()
	{
		InitializeComponent();
		InitUI(); // Initialize the UI.
	}

	private void InitUI()
	{
		HideSearchBarChk.IsChecked = Definitions.Settings.HideSearchBar; // Set the Checked state
	}

	private void HideSearchBarChk_Checked(object sender, RoutedEventArgs e)
	{
		Definitions.Settings.HideSearchBar = HideSearchBarChk.IsChecked; // Set settings value
		SettingsSaver.Save(); // Save changes

		// Hide search bar in MainWindow
		Definitions.MainWindow.SearchBox.Visibility = HideSearchBarChk.IsChecked.Value ? Visibility.Collapsed : Visibility.Visible; // Hide
		Definitions.MainWindow.SearchBtn.Visibility = !HideSearchBarChk.IsChecked.Value ? Visibility.Collapsed : Visibility.Visible; // Hide

		// Reset toggle search button state
		Definitions.MainWindow.SearchBtn.Background = new SolidColorBrush(Colors.Transparent); // Set default background color
		Definitions.MainWindow.IsSearchVisible = false; // Reset to default value
	}

	private void SaveButton_Click(object sender, RoutedEventArgs e)
	{

	}

	private void SearchResultsTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
	{
		Regex regex = new("[^0-9]+");
		e.Handled = regex.IsMatch(e.Text);
	}
}
