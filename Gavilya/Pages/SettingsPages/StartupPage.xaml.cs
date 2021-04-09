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

namespace Gavilya.Pages.SettingsPages
{
	/// <summary>
	/// Logique d'interaction pour StartupPage.xaml
	/// </summary>
	public partial class StartupPage : Page
	{
		public StartupPage()
		{
			InitializeComponent();
			InitComboBox();
		}

		/// <summary>
		/// Add items to the combobox.
		/// </summary>
		private void InitComboBox()
		{
			string[] pagesNames = Properties.Resources.PagesNames.Split(new string[] { ";" }, StringSplitOptions.None); // Get pages names

			for (int i = 0; i < pagesNames.Length; i++) // For each item
			{
				PagesComboBox.Items.Add(pagesNames[i]); // Add items
			}

			PagesComboBox.SelectedIndex = Definitions.Settings.PageId; // Set the default page
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			Definitions.Settings.PageId = PagesComboBox.SelectedIndex; // Update settings
			SettingsSaver.Save(); // Save settings
		}
	}
}
