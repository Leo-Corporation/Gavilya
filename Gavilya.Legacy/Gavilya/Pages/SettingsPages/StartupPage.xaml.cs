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
using Gavilya.Enums;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Gavilya.Pages.SettingsPages;

/// <summary>
/// Logique d'interaction pour StartupPage.xaml
/// </summary>
public partial class StartupPage : Page
{
	public StartupPage()
	{
		InitializeComponent();
		InitUI();
	}

	/// <summary>
	/// Add items to the combobox.
	/// </summary>
	private void InitUI()
	{
		CardsPageRadioBtn.IsChecked = Global.Settings.PageId == 0; // Check if the page ID is equal to 0
		ListPageRadioBtn.IsChecked = Global.Settings.PageId == 2; // Check if the page ID is equal to 2

		HomePageRadioBtn.IsChecked = Global.Settings.DefaultGavilyaHomePage == GavilyaWindowPages.Home; // Check
		LibraryPageRadioBtn.IsChecked = Global.Settings.DefaultGavilyaHomePage == GavilyaWindowPages.Library; // Check
		ProfilePageRadioBtn.IsChecked = Global.Settings.DefaultGavilyaHomePage == GavilyaWindowPages.Profile; // Check
		RecentPageRadioBtn.IsChecked = Global.Settings.DefaultGavilyaHomePage == GavilyaWindowPages.Recent; // Check if the page ID is equal to 1
	}

	Border CheckedBorder { get; set; }
	Border PageCheckedBorder { get; set; }
	private void Border_MouseEnter(object sender, MouseEventArgs e)
	{
		Border border = (Border)sender;
		border.BorderBrush = Global.GetSolidColor("Accent"); // Set color
	}

	private void Border_MouseLeave(object sender, MouseEventArgs e)
	{
		Border border = (Border)sender;
		if (border != CheckedBorder)
		{
			border.BorderBrush = new SolidColorBrush() { Color = Colors.Transparent }; // Set color 
		}
	}

	private void CardsPageBorder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		CardsPageRadioBtn.IsChecked = true; // Check
		CheckedBorder = CardsPageBorder; // Set checked border
		RefreshBorders(); // Refresh

		Global.Settings.PageId = 0; // Set the startup page
		SettingsSaver.Save(); // Save changes
	}

	private void RecentPageBorder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		RecentPageRadioBtn.IsChecked = true; // Check
		PageCheckedBorder = RecentPageBorder; // Set checked border
		RefreshPageBorders(); // Refresh

		Global.Settings.DefaultGavilyaHomePage = GavilyaWindowPages.Recent; // Set the startup page
		SettingsSaver.Save(); // Save changes
	}

	private void CardsPageRadioBtn_Checked(object sender, RoutedEventArgs e)
	{
		CheckedBorder = CardsPageBorder; // Set checked border
		RefreshBorders(); // Refresh
	}

	private void RecentPageRadioBtn_Checked(object sender, RoutedEventArgs e)
	{
		PageCheckedBorder = RecentPageBorder; // Set checked border
		RefreshPageBorders(); // Refresh
	}

	private void ListPageRadioBtn_Checked(object sender, RoutedEventArgs e)
	{
		CheckedBorder = ListPageBorder; // Set checked border
		RefreshBorders(); // Refresh
	}

	private void ListPageBorder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		ListPageRadioBtn.IsChecked = true; // Check
		CheckedBorder = ListPageBorder; // Set checked border
		RefreshBorders(); // Refresh

		Global.Settings.PageId = 2; // Set the startup page
		SettingsSaver.Save(); // Save changes
	}

	private void RefreshBorders()
	{
		CardsPageBorder.BorderBrush = new SolidColorBrush() { Color = Colors.Transparent }; // Set color 
		ListPageBorder.BorderBrush = new SolidColorBrush() { Color = Colors.Transparent }; // Set color 

		CheckedBorder.BorderBrush = Global.GetSolidColor("Accent"); // Set color
	}

	private void RefreshPageBorders()
	{
		HomePageBorder.BorderBrush = new SolidColorBrush() { Color = Colors.Transparent }; // Set color 
		LibraryPageBorder.BorderBrush = new SolidColorBrush() { Color = Colors.Transparent }; // Set color 
		ProfilePageBorder.BorderBrush = new SolidColorBrush() { Color = Colors.Transparent }; // Set color 
		RecentPageBorder.BorderBrush = new SolidColorBrush() { Color = Colors.Transparent }; // Set color 

		PageCheckedBorder.BorderBrush = Global.GetSolidColor("Accent"); // Set color
	}

	private void HomePageBorder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		HomePageRadioBtn.IsChecked = true; // Check
		PageCheckedBorder = HomePageBorder; // Set checked border
		RefreshPageBorders(); // Refresh

		Global.Settings.DefaultGavilyaHomePage = GavilyaWindowPages.Home;
		SettingsSaver.Save(); // Save changes
	}

	private void LibraryPageBorder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		LibraryPageRadioBtn.IsChecked = true; // Check
		PageCheckedBorder = LibraryPageBorder; // Set checked border
		RefreshPageBorders(); // Refresh

		Global.Settings.DefaultGavilyaHomePage = GavilyaWindowPages.Library;
		SettingsSaver.Save(); // Save changes
	}

	private void ProfilePageBorder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		ProfilePageRadioBtn.IsChecked = true; // Check
		PageCheckedBorder = ProfilePageBorder; // Set checked border
		RefreshPageBorders(); // Refresh

		Global.Settings.DefaultGavilyaHomePage = GavilyaWindowPages.Profile;
		SettingsSaver.Save(); // Save changes
	}

	private void HomePageRadioBtn_Checked(object sender, RoutedEventArgs e)
	{
		PageCheckedBorder = HomePageBorder; // Set checked border
		RefreshPageBorders(); // Refresh
	}

	private void LibraryPageRadioBtn_Checked(object sender, RoutedEventArgs e)
	{
		PageCheckedBorder = LibraryPageBorder; // Set checked border
		RefreshPageBorders(); // Refresh
	}

	private void ProfilePageRadioBtn_Checked(object sender, RoutedEventArgs e)
	{
		PageCheckedBorder = ProfilePageBorder; // Set checked border
		RefreshPageBorders(); // Refresh
	}

	private void Border_MouseEnter_1(object sender, MouseEventArgs e)
	{
		Border border = (Border)sender;
		border.BorderBrush = Global.GetSolidColor("Accent"); // Set color
	}

	private void Border_MouseLeave_1(object sender, MouseEventArgs e)
	{
		Border border = (Border)sender;
		if (border != PageCheckedBorder)
		{
			border.BorderBrush = new SolidColorBrush() { Color = Colors.Transparent }; // Set color 
		}
	}
}
