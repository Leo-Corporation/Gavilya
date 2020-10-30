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
using Gavilya.Enums;
using Gavilya.Pages.SettingsPages;
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
using System.Windows.Shapes;

namespace Gavilya.Windows
{
    /// <summary>
    /// Logique d'interaction pour Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        SaveOptionsPage saveOptionsPage = new SaveOptionsPage(); // Create a page
        public Settings()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized; // Minimize the window
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close(); // Close the window
        }

        private void HideAllPages()
        {
            saveOptionsPage.Visibility = Visibility.Hidden; // Hide the page
        }

        private void NavigateToPage(SettingsPages settingsPage)
        {
            HideAllPages(); // Hide all the pages

            switch (settingsPage)
            {
                case SettingsPages.Languages:
                    //TODO
                    break;
                case SettingsPages.SaveOptions:
                    saveOptionsPage.Visibility = Visibility.Visible; // Show the page
                    OptionsDisplayer.Navigate(saveOptionsPage); // Navigate
                    break;
            }
        }

        private void SaveOptions_Click(object sender, RoutedEventArgs e)
        {
            NavigateToPage(SettingsPages.SaveOptions); // Show the "SaveOptions" page
        }

        private void LanguageOptions_Click(object sender, RoutedEventArgs e)
        {
            NavigateToPage(SettingsPages.Languages); // Show the "Languages" page
        }
    }
}
