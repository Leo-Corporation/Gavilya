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

namespace Gavilya.UserControls
{
    /// <summary>
    /// Logique d'interaction pour GameCard.xaml
    /// </summary>
    public partial class GameCard : UserControl
    {
        string location;
        public GameCard(GameInfo gameInfo)
        {
            InitializeComponent();
            InitializeUI(gameInfo);
        }

        /// <summary>
        /// Initialize the elements of the Interface.
        /// </summary>
        /// <param name="gameInfo"></param>
        private void InitializeUI(GameInfo gameInfo)
        {
            // Name of the game
            Title.Text = gameInfo.Name;

            // Version
            Version.Text = gameInfo.Version;

            // Location
            location = gameInfo.FileLocation;

            // Icon
            GameIcon.Source = gameInfo.Icon.Source;

            // Icon (in case the precedent didn't work)
            /*if (File.Exists(gameInfo.IconFileLocation))
            {
                BitmapImage bitmapImage = new BitmapImage(new Uri(gameInfo.IconFileLocation)); // Find the image
                GameIcon.Source = bitmapImage; // Set the image
            }*/
        }

        private void Button_Click(object sender, RoutedEventArgs e) // Play button
        {
            if (File.Exists(location)) // If the file exist
            {
                Process.Start(location); // Start the game
            }
        }
    }
}
