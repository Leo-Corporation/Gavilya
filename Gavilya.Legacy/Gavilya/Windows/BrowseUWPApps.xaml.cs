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
using Gavilya.UserControls;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Gavilya.Windows;
/// <summary>
/// Interaction logic for BrowseUWPApps.xaml
/// </summary>
public partial class BrowseUWPApps : Window
{
	internal TextBox PackageFamilyaNameTextBox { get; init; }
	internal TextBox AppIDTextBox { get; init; }
	internal TextBox GameNameTextBox { get; init; }

	public BrowseUWPApps(TextBox packageFamilyaNameTextBox, TextBox appIDTextBox, TextBox gameNameTextBox)
	{
		InitializeComponent();
		PackageFamilyaNameTextBox = packageFamilyaNameTextBox; // Set property
		AppIDTextBox = appIDTextBox; // Set property
		GameNameTextBox = gameNameTextBox; // Set property

		InitUI();
	}

	private async void InitUI()
	{
		List<SDK.UwpApp> apps = await Global.GetUwpAppsAsync();

		for (int i = 0; i < apps.Count; i++)
		{
			GamesPanel.Children.Add(new UwpAppItem(apps[i]));
		}
	}

	private void CloseBtn_Click(object sender, RoutedEventArgs e)
	{
		Close();
	}

	private void SelectBtn_Click(object sender, RoutedEventArgs e)
	{
		for (int i = 0; i < GamesPanel.Children.Count; i++)
		{
			if (GamesPanel.Children[i] is UwpAppItem uwpAppItem)
			{
				uwpAppItem = (UwpAppItem)GamesPanel.Children[i];
				if (uwpAppItem.GameCheck.IsChecked.Value)
				{
					PackageFamilyaNameTextBox.Text = uwpAppItem.UwpApp.AppID.Split("!")[0]; // Get and set PackageFamilyName text
					AppIDTextBox.Text = uwpAppItem.UwpApp.AppID.Split("!")[1]; // Get and set AppID text
					GameNameTextBox.Text = uwpAppItem.UwpApp.Name; // Set text

					Close(); // Close the Window, go back to the parent window.
					break;
				}
			}
		}
	}
}
