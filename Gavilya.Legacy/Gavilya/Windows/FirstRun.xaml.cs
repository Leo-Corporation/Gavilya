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
using Gavilya.Enums;
using Gavilya.Pages.FirstRunPages;
using System;
using System.Windows;

namespace Gavilya.Windows;

/// <summary>
/// Logique d'interaction pour FirstRun.xaml
/// </summary>
public partial class FirstRun : Window
{
	private Welcome welcome; // The "Welcome" page
	private AddGamesPage addGamesPage; // The "AddGamesPage"
	private FinishPage finishPage; // The "FinishPage"
	private ImportGamesPage importGamesPage; // The "ImportGamesPage"
	private SearchRAWGPage searchRAWGPage; // The "SearchRAWGPage"
	private SelectImportedGamesPage selectImportedGamesPage; // The "SelectImportedGamesPage"

	public FirstRun()
	{
		InitializeComponent();
		LoadUI(); // Load the UI
	}

	private void LoadUI()
	{
		welcome = new(this); // Define "Welcome"
		addGamesPage = new(this); // Define
		finishPage = new(this); // Define
		importGamesPage = new(this); // Define
		searchRAWGPage = new(this); // Define
		selectImportedGamesPage = new(this); // Define
		ChangePage(FirstRunPages.Welcome); // Change page
	}

	/// <summary>
	/// Change page.
	/// </summary>
	/// <param name="firstRunPage">The page to change.</param>
	internal void ChangePage(FirstRunPages firstRunPage)
	{
		switch (firstRunPage)
		{
			case FirstRunPages.AddGames:
				PageViewer.Content = addGamesPage; // Change page
				break;
			case FirstRunPages.Finish:
				PageViewer.Content = finishPage; // Change page
				break;
			case FirstRunPages.ImportGames:
				PageViewer.Content = importGamesPage; // Change page
				break;
			case FirstRunPages.SearchRawgGames:
				PageViewer.Content = searchRAWGPage; // Change page
				break;
			case FirstRunPages.Welcome:
				PageViewer.Content = welcome; // Change page
				break;
			case FirstRunPages.SelectImportedGames:
				PageViewer.Content = selectImportedGamesPage; // Change page
				break;
		}
	}

	private void Button_Click(object sender, RoutedEventArgs e)
	{
		WindowState = WindowState.Minimized; // Minimize
	}

	private void CloseBtn_Click(object sender, RoutedEventArgs e)
	{
		Environment.Exit(0); // Close the app
	}
}
