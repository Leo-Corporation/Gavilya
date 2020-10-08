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
using System.Diagnostics;
using System.IO;
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

namespace Gavilya.Pages
{
    /// <summary>
    /// Logique d'interaction pour GameInfoPage.xaml
    /// </summary>
    public partial class GameInfoPage : Page
    {
        GameInfo GameInfo { get; set; }
        string gameLocation;

        public GameInfoPage(GameInfo gameInfo)
        {
            InitializeComponent();

            GameInfo = gameInfo;
            InitializeUI(gameInfo); // Initialize the UI
        }

        public GameInfoPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize the User Interface of the page.
        /// </summary>
        /// <param name="gameInfo">The game to load informations.</param>
        public void InitializeUI(GameInfo gameInfo)
        {
            // Var
            gameLocation = gameInfo.FileLocation;

            // Text
            PlayBtnToolTip.Content = Properties.Resources.PlayLowerCase + Properties.Resources.PlayTo + gameInfo.Name; // Set the tooltip
            GameNameTxt.Text = gameInfo.Name; // Set the name of the game
            LastTimePlayedTxt.Text = $"{Properties.Resources.LastTimePlayed} {gameInfo.LastTimePlayed}"; // Last time played
            DescriptionTxt.Text = gameInfo.Description;

            // Icon
            if (gameInfo.IconFileLocation != string.Empty) // If a custom image is used
            {
                var bitmap = new BitmapImage();
                var stream = File.OpenRead(gameInfo.IconFileLocation);

                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = stream;
                bitmap.EndInit();
                stream.Close();
                stream.Dispose();
                bitmap.Freeze();
                BackgroundImage.ImageSource = bitmap;
            }
            else
            {
                System.Drawing.Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(gameInfo.FileLocation);
                BackgroundImage.ImageSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()); // Show the image
            }

            // Platforms
            PlatformDisplayer.Children.Clear();
            PlatformDisplayer.Children.Add(
                new TextBlock
                {
                    Foreground = new SolidColorBrush { Color = Colors.White }, // Set the foreground to white
                    Margin = new Thickness { Left = 1, Bottom = 1, Right = 1, Top = 1 }, // Set the the margin
                    FontSize = 20, // Set the font size
                    FontWeight = FontWeights.Bold, // Set the font weight
                    Text = Properties.Resources.Platforms // Set the text
                }
            ); // Add the textblock

            foreach (SDK.RAWG.Platform platform in gameInfo.Platforms)
            {
                PlatformDisplayer.Children.Add(new TextBlock { Foreground = new SolidColorBrush { Color = Colors.White }, Margin = new Thickness { Left = 1, Bottom = 1, Right = 1, Top = 1 }, Text = platform.name }); // New textblock
            }
        }

        private void PlayBtn_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(gameLocation)) // If the file exist
            {
                Process.Start(gameLocation); // Start the game
            }
        }
    }
}
