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
using System.Collections.Generic;
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

namespace Gavilya.Pages
{
	/// <summary>
	/// Interaction logic for LibraryPage.xaml
	/// </summary>
	public partial class LibraryPage : Page
	{
		public LibraryPage()
		{
			InitializeComponent();
		}

		private void GameCardTabBtn_Click(object sender, RoutedEventArgs e)
		{
			CheckedButton = GameCardTabBtn; // Set
			RefreshTabUI();

			PageDisplayer.Content = Definitions.GamesCardsPages; // Set page content
		}

		private void RecentTabBtn_Click(object sender, RoutedEventArgs e)
		{
			CheckedButton = RecentTabBtn; // Set
			RefreshTabUI();

			PageDisplayer.Content = Definitions.RecentGamesPage; // Set page content
		}

		private void GameListTabBtn_Click(object sender, RoutedEventArgs e)
		{
			CheckedButton = GameListTabBtn; // Set
			RefreshTabUI();

			PageDisplayer.Content = Definitions.GamesListPage; // Set page content
		}

		Button CheckedButton { get; set; }
		private void RefreshTabUI()
		{
			GameCardTabBtn.BorderBrush = new SolidColorBrush { Color = Colors.Transparent }; // Change color 
			RecentTabBtn.BorderBrush = new SolidColorBrush { Color = Colors.Transparent }; // Change color 
			GameListTabBtn.BorderBrush = new SolidColorBrush { Color = Colors.Transparent }; // Change color 

			CheckedButton.BorderBrush = new SolidColorBrush { Color = Color.FromRgb(102, 0, 255) }; // Change color
		}

		private void GameCardTabBtn_MouseEnter(object sender, MouseEventArgs e)
		{
			Button button = (Button)sender; // Create button

			button.BorderBrush = new SolidColorBrush { Color = Color.FromRgb(102, 0, 255) }; // Change color
		}

		private void GameCardTabBtn_MouseLeave(object sender, MouseEventArgs e)
		{
			Button button = (Button)sender; // Create button

			if (CheckedButton != button)
			{
				button.BorderBrush = new SolidColorBrush { Color = Colors.Transparent }; // Change color 
			}
		}
	}
}
