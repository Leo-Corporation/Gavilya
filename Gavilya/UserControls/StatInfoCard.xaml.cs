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

namespace Gavilya.UserControls
{
	/// <summary>
	/// Interaction logic for StatInfoCard.xaml
	/// </summary>
	public partial class StatInfoCard : UserControl
	{
		GameInfo GameInfo { get; init; }
		internal bool isChecked = false;
		public StatInfoCard(GameInfo gameInfo, int pos)
		{
			InitializeComponent();
			GameInfo = gameInfo; // Set

			InitUI(pos); // Load the UI
		}

		private void InitUI(int pos)
		{
			// Calc
			double time = (double)GameInfo.TotalTimePlayed / 3600; // Convert seconds to hours

			// Text
			GamePosTxt.Text = $"#{pos}"; // Set text
			GameNameTxt.Text = GameInfo.Name; // Set text
			GameTimeTxt.Text = $"{string.Format("{0:0.#}", time)}{Properties.Resources.HourShort}"; // Set text

			isChecked = (pos == 1) ? true : false; // Set
			if (isChecked)
			{
				ItemBorder.Background = new SolidColorBrush { Color = Color.FromRgb(60, 60, 80) }; // Set background color 
				Definitions.StatGameInfoControl.InitUI(GameInfo); // Load UI
			}
		}

		private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			Definitions.Statistics.UnCheckAllStatItems(); // Clear
			ItemBorder.Background = new SolidColorBrush { Color = Color.FromRgb(60, 60, 80) }; // Set background color
			isChecked = true;
			Definitions.StatGameInfoControl.InitUI(GameInfo); // Load UI
		}

		private void ItemBorder_MouseEnter(object sender, MouseEventArgs e)
		{
			ItemBorder.Background = new SolidColorBrush { Color = Color.FromRgb(40, 40, 60) }; // Set background color
		}

		private void ItemBorder_MouseLeave(object sender, MouseEventArgs e)
		{
			if (!isChecked)
			{
				ItemBorder.Background = new SolidColorBrush { Color = Colors.Transparent }; // Set background color 
			}
			else
			{
				ItemBorder.Background = new SolidColorBrush { Color = Color.FromRgb(60, 60, 80) }; // Set background color
			}
		}

		internal void UnCheck()
		{
			isChecked = false;
			ItemBorder.Background = new SolidColorBrush { Color = Colors.Transparent }; // Set background color 
		}
	}
}
