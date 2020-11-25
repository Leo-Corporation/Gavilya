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
using Gavilya.Windows;
using LeoCorpLibrary;
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
using System.Windows.Threading;

namespace Gavilya.Pages
{
    /// <summary>
    /// Logique d'interaction pour GameInfoPage.xaml
    /// </summary>
    public partial class GameInfoPage : Page
    {
        GameInfo GameInfo { get; set; }
        UIElement parentUIElement = new UIElement();
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
        /// <param name="parent">The parent element.</param>
        public void InitializeUI(GameInfo gameInfo, UIElement parent = null)
        {
            // Var
            gameLocation = gameInfo.FileLocation;
            GameInfo = gameInfo; // Define the game info
            parentUIElement = parent; // Define the parent element

            // Text
            PlayBtnToolTip.Content = Properties.Resources.PlayLowerCase + Properties.Resources.PlayTo + gameInfo.Name; // Set the tooltip
            GameNameTxt.Text = gameInfo.Name; // Set the name of the game
            
            if (gameInfo.TotalTimePlayed != 0) // If the game was played
            {
                DisplayTotalTimePlayed(gameInfo.TotalTimePlayed); // Set the text
            }
            else
            {
                TotalTimePlayedTxt.Text = $"{Properties.Resources.TotalTimePlayed} {Properties.Resources.Never}"; // Set the text
            }

            if (gameInfo.LastTimePlayed != 0) // If the game was played
            {
                DateTime LastTimePlayed = Global.UnixTimeToDateTime(gameInfo.LastTimePlayed); // Get the date time
                LastTimePlayedTxt.Text = $"{Properties.Resources.LastTimePlayed} {LastTimePlayed.Day} {Global.NumberToMonth(LastTimePlayed.Month)} {LastTimePlayed.Year}"; // Last time played
            }
            else
            {
                LastTimePlayedTxt.Text = $"{Properties.Resources.LastTimePlayed} {Properties.Resources.Never}"; // Set the text
            }

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

        DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) }; // Create a timer
        bool gameStarted = false;

        private void PlayBtn_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(gameLocation)) // If the file exist
            {
                Process.Start(gameLocation); // Start the game
                
                if (parentUIElement is GameCard) // If the parent element is a game card
                {
                    GameCard gameCard = (GameCard)parentUIElement; // Create a game card
                    gameCard.GameInfo.LastTimePlayed = Env.GetUnixTime(); // Set the last time played
                    new GameSaver().Save(Definitions.Games); // Save the changes

                    timer.Tick += Timer_Tick; // Define the tick event
                    timer.Start(); // Start the timer

                    DateTime LastTimePlayed = Global.UnixTimeToDateTime(GameInfo.LastTimePlayed); // Get the date time
                    LastTimePlayedTxt.Text = $"{Properties.Resources.LastTimePlayed} {LastTimePlayed.Day} {Global.NumberToMonth(LastTimePlayed.Month)} {LastTimePlayed.Year}"; // Last time played
                }
                else if (parentUIElement is GameItem)
                {
                    GameItem gameItem = (GameItem)parentUIElement; // Create a game item
                    gameItem.GameInfo.LastTimePlayed = Env.GetUnixTime(); // Set the last time played
                    Definitions.Games[Definitions.Games.IndexOf(gameItem.GameInfo)].LastTimePlayed = gameItem.GameInfo.LastTimePlayed; // Update the games
                    new GameSaver().Save(Definitions.Games); // Save the changes

                    timer.Tick += Timer_Tick; // Define the tick event
                    timer.Start(); // Start the timer

                    DateTime LastTimePlayed = Global.UnixTimeToDateTime(GameInfo.LastTimePlayed); // Get the date time
                    LastTimePlayedTxt.Text = $"{Properties.Resources.LastTimePlayed} {LastTimePlayed.Day} {Global.NumberToMonth(LastTimePlayed.Month)} {LastTimePlayed.Year}"; // Last time played
                }

                Definitions.RecentGamesPage.LoadGames(); // Reload the games
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            string processName = System.IO.Path.GetFileNameWithoutExtension(GameInfo.FileLocation); // Get the process name

            if (Global.IsProcessRunning(processName)) // If the game is running
            {
                gameStarted = true; // The game has started
                GameInfo.TotalTimePlayed += 1; // Increment the time played
            }
            else
            {
                if (gameStarted) // If the game has been started
                {
                    new GameSaver().Save(Definitions.Games); // Save
                    DisplayTotalTimePlayed(GameInfo.TotalTimePlayed); // Update the text
                    timer.Stop(); // Stop
                }
            }
        }

        private void DisplayTotalTimePlayed(int timePlayed)
        {
            GameTimePlayed gameTimePlayed = GameTimePlayed.GetTimePlayed(timePlayed); // Create a GameTimePlayed
            string finalString = $"{Properties.Resources.TotalTimePlayed} "; // The final message

            string hoursPlurial = (gameTimePlayed.Hours > 1) ? "s" : ""; // Determine if a plurial is necessary
            string minutesPlurial = (gameTimePlayed.Minutes > 1) ? "s" : ""; // Determine if a plurial is necessary
            string secondsPlurial = (gameTimePlayed.Seconds > 1) ? "s" : ""; // Determine if a plurial is necessary

            if (gameTimePlayed.Hours != 0) // If the game was played more than an hour
            {
                finalString += $"{gameTimePlayed.Hours} {Properties.Resources.HourMin + hoursPlurial} "; // Add the hours
            }

            finalString += $"{gameTimePlayed.Minutes} {Properties.Resources.MinuteMin + minutesPlurial} "; // Add the minutes

            if (gameTimePlayed.Seconds != 0)
            {
                finalString += $"{gameTimePlayed.Seconds} {Properties.Resources.SecondMin + secondsPlurial}"; // Add the hours
            }

            TotalTimePlayedTxt.Text = finalString; // Set the text
        }

        private GameProperties GameProperties { get => new GameProperties(GameInfo); }
        private void PropertiesBtn_Click(object sender, RoutedEventArgs e)
        {
            GameProperties.Show();
        }
    }
}
