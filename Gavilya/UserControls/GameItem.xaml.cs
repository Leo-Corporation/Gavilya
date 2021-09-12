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
using Gavilya.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Gavilya.UserControls
{
	/// <summary>
	/// Logique d'interaction pour GameItem.xaml
	/// </summary>
	public partial class GameItem : UserControl
	{
		public GameInfo GameInfo { get; set; }
		public DispatcherTimer Timer { get; set; }
		public GameItem(GameInfo gameInfo)
		{
			InitializeComponent();
			GameInfo = gameInfo;
			LoadInformations(); // Load the game's info

			Timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) }; // Define the timer
			Timer.Tick += Timer_Tick; // Add the event
		}

		bool gameStarted = false;
		private void Timer_Tick(object sender, EventArgs e)
		{
			string processName = (!string.IsNullOrEmpty(GameInfo.ProcessName)) ? GameInfo.ProcessName : System.IO.Path.GetFileNameWithoutExtension(GameInfo.FileLocation); // Get the process name

			Definitions.GameInfoPage.DisplayTotalTimePlayed((Definitions.GameInfoPage.GameInfo == null) ? GameInfo.TotalTimePlayed : Definitions.GameInfoPage.GameInfo.TotalTimePlayed); // Refresh
			Definitions.GameInfoPage2.DisplayTotalTimePlayed((Definitions.GameInfoPage2.GameInfo == null) ? GameInfo.TotalTimePlayed : Definitions.GameInfoPage2.GameInfo.TotalTimePlayed); // Refresh

			if (Global.IsProcessRunning(processName)) // If the game is running
			{
				gameStarted = true; // The game has started
				GameInfo.TotalTimePlayed += 1; // Increment the time played
			}
			else
			{
				if (gameStarted) // If the game has been started
				{
					new GameSaver().Save(Definitions.Games); // Save
					if (!GameInfo.AlwaysCheckIfRunning)
					{
						Timer.Stop();
					}
				}
			}
		}

		private void LoadInformations()
		{
			GameName.Text = GameInfo.Name; // Set the name
			GameNameToolTip.Content = GameInfo.Name;
		}

		private void GameBtn_Click(object sender, RoutedEventArgs e)
		{
			Definitions.GameInfoPage2.InitializeUI(GameInfo, this);
			Definitions.GamesListPage.GamePage.Navigate(Definitions.GameInfoPage2);
			CheckedChanged();
		}

		private void CheckedChanged()
		{
			foreach (UIElement uIElement in Definitions.GamesListPage.GameList.Children) // For each UIElement in the list
			{
				if (uIElement is GameItem) // If the UIElement is a GameItem
				{
					GameItem gameItem = (GameItem)uIElement; // Create a GameItem
					gameItem.GameBtn.Background = Definitions.TransparentColor; // Change the background color
				}
			}

			GameBtn.Background = new SolidColorBrush { Color = Color.FromRgb(40, 40, 60) }; // Change the background color
		}
	}
}
