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
using System;
using System.Threading;
using System.Windows;

namespace Gavilya.Windows
{
	/// <summary>
	/// Logique d'interaction pour PopupMenu.xaml
	/// </summary>
	public partial class PopupMenu : Window
	{
		public PopupMenu()
		{
			InitializeComponent();
			if (Properties.Settings.Default.Language != "_default")
			{
				Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Properties.Settings.Default.Language); // Change 
			}
		}

		private void Window_Deactivated(object sender, EventArgs e)
		{
			if (Definitions.IsMenuShown)
			{
				Hide(); // Close
				Definitions.IsMenuShown = false; // Define
			}
		}

		private void SettingsBtn_Click(object sender, RoutedEventArgs e)
		{
			Settings settings = new(); // Settings window
			settings.Show(); // Show the Settings window
		}

		private void AboutBtn_Click(object sender, RoutedEventArgs e)
		{
			About about = new(); // About window
			about.Show(); // Show the About window
		}
	}
}
