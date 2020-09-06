using Gavilya.Classes;
using Gavilya.Windows;
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
using System.Windows.Media.Effects;
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
        string location; // Location

        /// <summary>
        /// The infos of the game
        /// </summary>
        public GameInfo GameInfo { get; set; }

        public GameCard(GameInfo gameInfo)
        {
            InitializeComponent();
            GameInfo = gameInfo; // Define the info
            InitializeUI(gameInfo); // Load the UI
        }

        /// <summary>
        /// Initialize the elements of the Interface.
        /// </summary>
        /// <param name="gameInfo"><see cref="Classes.GameInfo"/></param>
        /// <param name="isFromEdit"><see cref="true"/> if is called from the <see cref="Windows.EditGame"/> window.</param>
        internal void InitializeUI(GameInfo gameInfo, bool isFromEdit = false)
        {
            // Name of the game
            Title.Text = gameInfo.Name;

            // Version
            Version.Text = gameInfo.Version;

            // Location
            location = gameInfo.FileLocation;

            // Icon
            if (gameInfo.IconFileLocation != string.Empty) // If a custom image is used
            {
                GameIcon.ImageSource = new BitmapImage(new Uri(gameInfo.IconFileLocation));
            }
            else
            {
                System.Drawing.Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(gameInfo.FileLocation);
                GameIcon.ImageSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()); // Show the image
            }

            // Favorite
            if (gameInfo.IsFavorite && !isFromEdit) // If the game is a favorite
            {
                FavoriteGameCard = new FavoriteGameCard(gameInfo);
                Definitions.MainWindow.FavoriteBar.Children.Add(FavoriteGameCard); // Add the game to the favorite bar
                FavBtn.Content = ""; // Change icon
            }

            // Checkbox visibility
            if (Definitions.IsGamesCardsPagesCheckBoxesVisible) // If the checkboxes are visibles
            {
                CheckBox.Visibility = Visibility.Visible; // Visible
            }
            else
            {
                CheckBox.Visibility = Visibility.Hidden; // Hiddent
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e) // Play button
        {
            if (File.Exists(location)) // If the file exist
            {
                Process.Start(location); // Start the game
            }
        }

        private void StackPanel_MouseEnter(object sender, MouseEventArgs e)
        {
            Infos.Background = new SolidColorBrush { Color = Color.FromArgb(100, 50, 50, 72), Opacity = 0.8 }; // Show the background
            Title.Visibility = Visibility.Visible; // Show
            Version.Visibility = Visibility.Visible; // Show
        }

        private void StackPanel_MouseLeave(object sender, MouseEventArgs e)
        {
            Infos.Background = new SolidColorBrush { Opacity = 0 }; // Hide the background
            Title.Visibility = Visibility.Hidden; // Hide
            Version.Visibility = Visibility.Hidden; // Hide
        }

        FavoriteGameCard FavoriteGameCard;

        private void FavBtn_Click(object sender, RoutedEventArgs e)
        {
            if (GameInfo.IsFavorite) // If the game is a favorite
            {
                GameInfo.IsFavorite = false; // The game is no longer a favorite
                Definitions.MainWindow.FavoriteBar.Children.Remove(FavoriteGameCard); // Remove from favorite bar
                FavBtn.Content = ""; // Change icon
            }
            else
            {
                GameInfo.IsFavorite = true; // Set the game to be a favorite
                FavoriteGameCard = new FavoriteGameCard(GameInfo);
                Definitions.MainWindow.FavoriteBar.Children.Add(FavoriteGameCard); // Add to favorite bar
                FavBtn.Content = ""; // Change icon
            }
            new GameSaver().Save(Definitions.Games); // Save the changes
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            new EditGame(this).Show(); // Show the EditGame window
        }
    }
}
