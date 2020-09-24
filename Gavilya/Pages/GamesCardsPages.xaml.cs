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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Gavilya.Classes;
using Gavilya.UserControls;
using Gavilya.Windows;
using LeoCorpLibrary;

namespace Gavilya.Pages
{
    /// <summary>
    /// Logique d'interaction pour GamesCardsPages.xaml
    /// </summary>
    public partial class GamesCardsPages : Page
    {
        public GamesCardsPages()
        {
            InitializeComponent();
            Definitions.GamesCardsPages = this; // Define the GamesCardsPages
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            new AddGame().Show(); // Open the "Add Game" dialog
        }

        private void GamePresenter_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[]) e.Data.GetData(DataFormats.FileDrop); // Get all the files droped
                List<string> executables = new List<string>(); // The execuables files

                for (int i = 0; i < files.Length; i++) // For each file
                {
                    if (System.IO.Path.GetExtension(files[i]) == ".exe") // If the file is a .exe
                    {
                        executables.Add(files[i]); // Add the file to the executables
                    }
                }

                for (int i = 0; i < executables.Count; i++) // For each executables (or games)
                {
                    FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(executables[i]);
                    GameInfo gameInfo = new GameInfo // Create a new GameInfo class/object
                    {
                        FileLocation = executables[i],
                        IsFavorite = false,
                        Name = string.IsNullOrEmpty(fileVersionInfo.ProductName) ? System.IO.Path.GetFileNameWithoutExtension(executables[i]) : fileVersionInfo.ProductName,
                        LastTimePlayed = 0,
                        TotalTimePlayed = 0,
                        IconFileLocation = string.Empty,
                        Version = fileVersionInfo.FileVersion // Get the version
                    };
                    Definitions.Games.Add(gameInfo); // Add the games to the List<GameInfo>
                    Definitions.GamesCardsPages.GamePresenter.Children.Add(new GameCard(gameInfo)); // Add the games to the GamePresenter
                    new GameSaver().Save(Definitions.Games); // Save the added games
                }
            }
        }
    }
}
