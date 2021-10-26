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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Gavilya.Pages
{
	/// <summary>
	/// Interaction logic for Statistics.xaml
	/// </summary>
	public partial class Statistics : Page
	{

		public Statistics()
		{
			InitializeComponent();
			InitUI();
		}

		internal void InitUI()
		{
			if (Definitions.Games.Count > 0)
			{
				BorderContent.Visibility = Visibility.Visible; // Show
				PlaceholderBorder.Visibility = Visibility.Collapsed; // Hide

				// Controls
				Definitions.StatGameInfoControl = (StatGameInfoControl)GameInfoDisplayer.Children[0]; // Set content
				GamesInfoDisplayer.Children.Clear();

				// Values
				Dictionary<GameInfo, int> gameTimes = new(); // Create dictionnary
				List<GameInfo> mostPlayed = new(); // Create list

				for (int i = 0; i < Definitions.Games.Count; i++)
				{
					gameTimes.Add(Definitions.Games[i], Definitions.Games[i].TotalTimePlayed); // Add item
				}

				var items = from pair in gameTimes orderby pair.Value descending select pair; // Sort

				// Top 10 most played games
				int c = 0; // Counter
				foreach (KeyValuePair<GameInfo, int> keyValuePair in items)
				{
					if (c < 10)
					{
						GamesInfoDisplayer.Children.Add(new StatInfoCard(keyValuePair.Key, c + 1)); // Add item
						mostPlayed.Add(keyValuePair.Key); // Add to the list
						c++; // Increment counter
					}
					else
					{
						break;
					}
				}

				// Text
				TotalTimePlayedTxt.Text = $"{Global.GetTotalTimePlayed() / 3600}{Properties.Resources.HourShort}"; // Set text
				SortTxt.Text = isDescending ? Properties.Resources.MostPlayed : Properties.Resources.LeastPlayed; // Set text

				// Graph
				GraphDisplayer.Content = new StatGraph(mostPlayed);
			}
			else
			{
				BorderContent.Visibility = Visibility.Collapsed; // Hide
				PlaceholderBorder.Visibility = Visibility.Visible; // Show
			}
		}

		internal void UnCheckAllStatItems()
		{
			try
			{
				for (int i = 0; i < GamesInfoDisplayer.Children.Count; i++)
				{
					if (GamesInfoDisplayer.Children[i] is StatInfoCard)
					{
						((StatInfoCard)GamesInfoDisplayer.Children[i]).UnCheck(); // Uncheck
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		bool isDescending = true;
		private void SortBtn_Click(object sender, RoutedEventArgs e)
		{
			if (Definitions.Games.Count > 0)
			{
				GamesInfoDisplayer.Children.Clear(); // Clear
				isDescending = !isDescending; // Set
				SortTxt.Text = isDescending ? Properties.Resources.MostPlayed : Properties.Resources.LeastPlayed; // Set text

				// Values
				Dictionary<GameInfo, int> gameTimes = new(); // Create dictionnary
				List<GameInfo> mostPlayed = new(); // Create list

				for (int i = 0; i < Definitions.Games.Count; i++)
				{
					gameTimes.Add(Definitions.Games[i], Definitions.Games[i].TotalTimePlayed); // Add item
				}

				var items = from pair in gameTimes orderby pair.Value descending select pair; // Sort
				var items1 = from pair in gameTimes orderby pair.Value ascending select pair; // Sort

				// Top 10 most played games
				if (isDescending)
				{
					int c = 0; // Counter
					foreach (KeyValuePair<GameInfo, int> keyValuePair in items)
					{
						if (c < 10)
						{
							GamesInfoDisplayer.Children.Add(new StatInfoCard(keyValuePair.Key, c + 1)); // Add item
							mostPlayed.Add(keyValuePair.Key); // Add to the list
							c++; // Increment counter
						}
						else
						{
							break;
						}
					}
				}
				else
				{
					int c = gameTimes.Count; // Counter
					foreach (KeyValuePair<GameInfo, int> keyValuePair in items1)
					{
						if (c > 0 && c <= gameTimes.Count + (10 - gameTimes.Count))
						{
							GamesInfoDisplayer.Children.Add(new StatInfoCard(keyValuePair.Key, c)); // Add item
							mostPlayed.Add(keyValuePair.Key); // Add to the list
							c--; // Decrement counter
						}
						else
						{
							c--; // Decrement counter
						}
					}
				}
			}
		}
	}
}
