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
using System.Windows;
using System.Windows.Controls;

namespace Gavilya.Pages.FirstRunPages
{
	/// <summary>
	/// Logique d'interaction pour FinishPage.xaml
	/// </summary>
	public partial class FinishPage : Page
	{
		FirstRun FirstRun;
		public FinishPage(FirstRun firstRun)
		{
			InitializeComponent();
			FirstRun = firstRun; // Define
		}

		private void NextPage()
		{
			new MainWindow().Show(); // Show the main window
			FirstRun.Close(); // Close the window
			Definitions.Settings.IsFirstRun = false; // Set the FirstRun Settings to false
			SettingsSaver.Save();
		}

		private void NextBtn_Click(object sender, RoutedEventArgs e)
		{
			NextPage(); // Change page
		}
	}
}
