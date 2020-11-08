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

namespace Gavilya.Pages.SettingsPages
{
    /// <summary>
    /// Logique d'interaction pour LanguagePage.xaml
    /// </summary>
    public partial class LanguagePage : Page
    {
        public LanguagePage()
        {
            InitializeComponent();
            Initialize(); // Load the UI
        }

        private void Initialize()
        {
            Languages.Items.Add(Properties.Resources.Default); // Add the default item

            for (int i = 0; i < Definitions.Languages.Count; i++) // For each item
            {
                Languages.Items.Add(Definitions.Languages[i]); // Add an item
            }

            if (Definitions.Settings.Language != "_default") // If the language is not default
            {
                Languages.SelectedIndex = Definitions.LanguagesCodes.IndexOf(Definitions.Settings.Language) + 1; // Set the selected index
            }
            else
            {
                Languages.SelectedIndex = 0; // Set the selected index
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if ((string)Languages.SelectedItem != Properties.Resources.Default) // If the language is not default
            {
                Definitions.Settings.Language = Definitions.LanguagesCodes[Definitions.Languages.IndexOf((string)Languages.SelectedItem)];
            }
            else
            {
               Definitions.Settings.Language = "_default"; // Set the language to default
            }
            SettingsSaver.Save();
        }
    }
}
