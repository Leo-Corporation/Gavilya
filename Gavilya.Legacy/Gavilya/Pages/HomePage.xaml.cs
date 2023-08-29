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
using Gavilya.Windows;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Gavilya.Pages;

/// <summary>
/// Interaction logic for HomePage.xaml
/// </summary>
public partial class HomePage : Page
{
	public HomePage()
	{
		InitializeComponent();
		InitUI(); // Load the UI
	}

	internal void InitUI()
	{
		// Welcome message
		HelloTxt.Text = $"{Properties.Resources.Hello} {Global.Profiles[Global.Settings.CurrentProfileIndex].Name}{Properties.Resources.ExclamationMark}"; // Set text

		// Load "Statistics" page
		Statistics.Content = Global.Statistics; // Set content to Statistics page

		// Place holder
		RecentPlaceholder.Children.Add(new WelcomeRecentGames() { VerticalAlignment = VerticalAlignment.Center });
		RecentPlaceholder.Visibility = Visibility.Collapsed; // Hide
		RecentBar.Visibility = Visibility.Visible;

		if (Global.Games.Count < 3)
		{
			RandomGameBtn.Visibility = Visibility.Collapsed;
		}
	}

	private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
	{
		ScrollViewer scv = (ScrollViewer)sender;
		scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta / 2);
		e.Handled = true;
	}

	private void RandomGameBtn_Click(object sender, RoutedEventArgs e)
	{
		Random random = new();
		int i = random.Next(0, Global.Games.Count - 1);
		new RandomGameWindow(Global.Games[i], i).Show();
    }
}
