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
	/// Interaction logic for Statistics.xaml
	/// </summary>
	public partial class Statistics : Page
	{
		public Statistics()
		{
			InitializeComponent();
			InitUI();
		}

		private void InitUI()
		{
			// Values
			Dictionary<GameInfo, int> gameTimes = new(); // Create dictionnary

			for (int i = 0; i < Definitions.Games.Count; i++)
			{
				gameTimes.Add(Definitions.Games[i], Definitions.Games[i].TotalTimePlayed); // Add item
			}

			var items = from pair in gameTimes orderby pair.Value descending select pair; // Sort

			// Top 10 most played games
			int c = 0; // Counter
			foreach (KeyValuePair<GameInfo, int> keyValuePair in items)
			{
				if (c <= 10)
				{
					GamesInfoDisplayer.Children.Add(new StatInfoCard(keyValuePair.Key)); // Add item
					c++; // Increment counter
				}
				else
				{
					break;
				}
			}

			// Text
			TotalTimePlayedTxt.Text = $"{Global.GetTotalTimePlayed() / 3600}{Properties.Resources.HourShort}"; // Set text
		}
	}
}
