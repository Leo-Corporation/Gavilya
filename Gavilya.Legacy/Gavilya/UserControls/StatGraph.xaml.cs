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
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Gavilya.UserControls;

/// <summary>
/// Interaction logic for StatGraph.xaml
/// </summary>
public partial class StatGraph : UserControl
{
	List<GameInfo> Games { get; init; }
	public StatGraph(List<GameInfo> gameInfos)
	{
		InitializeComponent();
		Games = gameInfos; // Set

		InitUI(); // Load the UI
	}

	private void InitUI()
	{
		GraphPanel.Children.Clear(); // Clear
		int longestPlayed = Games[0].TotalTimePlayed;

		for (int i = 0; i < Games.Count; i++)
		{
			double h = Games[i].TotalTimePlayed * GraphPanel.Height / longestPlayed;

			Rectangle rectangle = new()
			{
				Margin = new(10, 0, 10, 0),
				Fill = Global.GetSolidColor("Graph"),
				Height = h,
				Width = 50,
				RadiusX = 5,
				RadiusY = 5,
				VerticalAlignment = VerticalAlignment.Bottom
			};

			GraphPanel.Children.Add(rectangle); // Add bar
		}
	}

	private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
	{
		InitUI();
	}
}
