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
using Microsoft.Win32;
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
    /// Logique d'interaction pour SaveOptionsPage.xaml
    /// </summary>
    public partial class SaveOptionsPage : Page
    {
        public SaveOptionsPage()
        {
            InitializeComponent();
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new(); // Create an OpenFileDialog
            openFileDialog.Filter = $"{Properties.Resources.GavFiles}|*.gav"; // Extension
            openFileDialog.Title = Properties.Resources.ImportGames; // Title

            if (openFileDialog.ShowDialog() ?? true) // If the user opened a file
            {
                if (MessageBox.Show(Properties.Resources.ImportConfirmMsg, Properties.Resources.MainWindowTitle, MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                {
                    new GameSaver().Import(openFileDialog.FileName); // Import
                }
            }
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new(); // Create a SaveFileDialog
            saveFileDialog.FileName = "GavilyaGames.gav"; // File name
            saveFileDialog.Filter = $"{Properties.Resources.GavFiles}|*.gav"; // Extension
            saveFileDialog.Title = Properties.Resources.ExportGames; // Title

            if (saveFileDialog.ShowDialog() ?? true)
            {
                string fileLocation = saveFileDialog.FileName; // Location of the file
                new GameSaver().Export(Definitions.Games, fileLocation); // Export the games
            }
        }
    }
}
