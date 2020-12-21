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
using Gavilya.Enums;
using Gavilya.Pages;
using Gavilya.UserControls;
using Gavilya.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Gavilya
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Title = Properties.Resources.MainWindowTitle;
            Global.SetWindowIcon(this); // Set the icon of the window

            GamesCardsPages gamesCardsPages = new GamesCardsPages(); // GamesCardsPage
            Definitions.GamesCardsPages = gamesCardsPages; // Define the GamesCardsPage
            PageContent.Content = gamesCardsPages; // Show the page

            Definitions.MainWindow = this; // Define the Main Window

            LoadPage(); // Load the button on the button corresponding to the active page
            new GameSaver().Load(); // Load the .gav file in the Definitions class

            LoadGames();
            WindowState = Definitions.Settings.IsMaximized ? WindowState.Maximized : WindowState.Normal; // Set the window state
            RefreshMaximizeRestoreButton();
        }

        private void LoadGames()
        {
            Definitions.GamesCardsPages.LoadGames(); // Load the games
            Definitions.RecentGamesPage.LoadGames(); // Load the games
            Definitions.GamesListPage.LoadGames(); // Load the games
        }

        private void DefineMaximumSize()
        {
            System.Windows.Forms.Screen currentScreen = System.Windows.Forms.Screen.FromHandle(new System.Windows.Interop.WindowInteropHelper(this).Handle); // The current screen

            MaxHeight = currentScreen.WorkingArea.Height;
            MaxWidth = currentScreen.WorkingArea.Width;
        }

        private void LoadPage()
        {
            RemoveShadowElement(RecentButton); // Remove the shadow effect from other buttons
            RemoveShadowElement(AppListButton); // Remove the shadow effect from other buttons

            ColorElement(RecentButton, new SolidColorBrush(Color.FromRgb(90, 90, 112))); // Change the backcolor
            ColorElement(AppListButton, new SolidColorBrush(Color.FromRgb(90, 90, 112))); // Change the backcolor

            ShadowElement(AppCardButton); // Put a shadow under the button

            ColorElement(AppCardButton, Definitions.HomeButtonBackColor); // Change the background
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            RefreshMaximizeRestoreButton(); // Refresh
            DefineMaximumSize();
            Definitions.Settings.IsMaximized = WindowState switch
            {
                WindowState.Maximized => true,
                WindowState.Normal    => false,
                WindowState.Minimized => false,
                _                     => false
            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized; // Minimize
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void maximizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized; // Maximize
        }

        private void restoreButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Normal; // Restore
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            SettingsSaver.Save(); // Save settings
            Environment.Exit(0); // Quit the app
        }

        /// <summary>
        /// Refresh the visibility of the Restore/Maximize buttons of the <see cref="Window"/>.
        /// </summary>
        private void RefreshMaximizeRestoreButton()
        {
            if (WindowState == WindowState.Maximized) // If Maximized
            {
                maximizeButton.Visibility = Visibility.Collapsed; // Hide
                restoreButton.Visibility = Visibility.Visible; // Show
                WindowBorder.Margin = new Thickness { Bottom = 0, Left = 0, Right = 0, Top = 0 }; // Remove the margin
            }
            else
            {
                maximizeButton.Visibility = Visibility.Visible; // Show
                restoreButton.Visibility = Visibility.Collapsed; // Hide
                WindowBorder.Margin = new Thickness { Bottom = 10, Left = 10, Right = 10, Top = 10 }; // Add the margin
            }
        }

        private void AppCardButton_Click(object sender, RoutedEventArgs e)
        {
            RemoveShadowElement(RecentButton); // Remove the shadow effect from other buttons
            RemoveShadowElement(AppListButton); // Remove the shadow effect from other buttons

            ColorElement(RecentButton, new SolidColorBrush(Color.FromRgb(90, 90, 112))); // Change the backcolor
            ColorElement(AppListButton, new SolidColorBrush(Color.FromRgb(90, 90, 112))); // Change the backcolor

            ShadowElement(AppCardButton); // Put a shadow under the button

            ColorElement(AppCardButton, Definitions.HomeButtonBackColor); // Change the background

            PageContent.Content = Definitions.GamesCardsPages; // Show the page
        }

        /// <summary>
        /// Put a shadow under an <see cref="UIElement"/>.
        /// </summary>
        /// <param name="uIElement">The <see cref="UIElement"/> to put a shadow to.</param>
        private void ShadowElement(UIElement uIElement)
        {
            DropShadowEffect dropShadowEffect = new DropShadowEffect(); // Shadow Effect

            Color color = new Color(); // Color of the shadow
            color.ScA = 1; // Alpha
            color.ScR = 0; // Red
            color.ScG = 0; // Green
            color.ScB = 0; // Blue

            dropShadowEffect.Color = color; // Put the shadow color
            dropShadowEffect.Direction = 315; // Direction of the shadow
            dropShadowEffect.ShadowDepth = 0; // Shadow Depth
            dropShadowEffect.BlurRadius = 5; // Blur radius
            dropShadowEffect.Opacity = 0.6; // Opacity

            uIElement.Effect = dropShadowEffect; // Put the shadow to the control
        }

        /// <summary>
        /// Remove the shadow effect of a <see cref="UIElement"/>.
        /// </summary>
        /// <param name="uIElement">The <see cref="UIElement"/> to remove the shadow to.</param>
        private void RemoveShadowElement(UIElement uIElement)
        {
            DropShadowEffect dropShadowEffect = new DropShadowEffect(); // Shadow Effect

            dropShadowEffect.ShadowDepth = 0; // Put the shadow depth to 0
            dropShadowEffect.BlurRadius = 0; // Put the blur radius to 5
            dropShadowEffect.Color = Color.FromArgb(0, 0, 0, 0); // Put the color of the shadow to transparent

            uIElement.Effect = dropShadowEffect; // Put the effect as the drop shadow
        }

        /// <summary>
        /// Change the background color of a <see cref="Button"/>.
        /// </summary>
        /// <param name="button">The <see cref="Button"/> to change the backcolor to.</param>
        /// <param name="linearGradientBrush">The new background.</param>
        private void ColorElement(Button button, Brush color)
        {
            button.Background = color;
        }

        private void RecentButton_Click(object sender, RoutedEventArgs e)
        {
            RemoveShadowElement(AppCardButton); // Remove the shadow effect from other buttons
            RemoveShadowElement(AppListButton); // Remove the shadow effect from other buttons

            ColorElement(AppCardButton, new SolidColorBrush(Color.FromRgb(90, 90, 112))); // Change the backcolor
            ColorElement(AppListButton, new SolidColorBrush(Color.FromRgb(90, 90, 112))); // Change the backcolor

            ShadowElement(RecentButton); // Put a shadow under the control

            ColorElement(RecentButton, Definitions.HomeButtonBackColor); // Change the background

            PageContent.Content = Definitions.RecentGamesPage; // Show the page
        }

        private void AppListButton_Click(object sender, RoutedEventArgs e)
        {
            RemoveShadowElement(AppCardButton); // Remove the shadow effect from other buttons
            RemoveShadowElement(RecentButton); // Remove the shadow effect from other buttons

            ColorElement(AppCardButton, new SolidColorBrush(Color.FromRgb(90, 90, 112))); // Change the backcolor
            ColorElement(RecentButton, new SolidColorBrush(Color.FromRgb(90, 90, 112))); // Change the backcolor

            ShadowElement(AppListButton); // Put a shadow under the control

            ColorElement(AppListButton, Definitions.HomeButtonBackColor); // Change the background

            PageContent.Content = Definitions.GamesListPage; // Show the page
        }

        private void SelectBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Definitions.GamesCardsPages.GamePresenter.Children.Count > 0) // If there is game(s)
            {
                for (int i = 0; i < Definitions.GamesCardsPages.GamePresenter.Children.Count; i++) // For each element
                {
                    if (Definitions.GamesCardsPages.GamePresenter.Children[i] is GameCard) // If the element is a GameCard
                    {
                        GameCard gameCard = (GameCard)Definitions.GamesCardsPages.GamePresenter.Children[i];
                        if (gameCard.CheckBox.IsVisible) // If the check box is visible
                        {
                            gameCard.CheckBox.Visibility = Visibility.Hidden; // The checkbox isn't visible
                            ColorElement(SelectBtn, new SolidColorBrush { Color = Colors.Transparent }); // Change the background
                            RemoveShadowElement(SelectBtn); // Remove shadow
                        }
                        else
                        {
                            gameCard.CheckBox.Visibility = Visibility.Visible; // The checkbox is visible
                            ColorElement(SelectBtn, Definitions.HomeButtonBackColor); // Change the background
                            ShadowElement(SelectBtn); // Shadow
                        }
                        Definitions.IsGamesCardsPagesCheckBoxesVisible = gameCard.CheckBox.IsVisible; // Set the property
                    }
                }
            }
            else
            {
                Definitions.IsGamesCardsPagesCheckBoxesVisible = false; // Hide all checkboxes
                ColorElement(SelectBtn, new SolidColorBrush { Color = Colors.Transparent }); // Change the background
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Definitions.GamesCardsPages.GamePresenter.Children.Count > 0)
            {
                Definitions.GamesCardsPages.WelcomeHost.Visibility = Visibility.Collapsed; // Hidden
                Definitions.GamesCardsPages.GamePresenter.Visibility = Visibility.Visible; // Visible
                List<GameCard> games = new List<GameCard>(); // List of all the games

                foreach (UIElement uIElement in Definitions.GamesCardsPages.GamePresenter.Children) // Foreach elements
                {
                    if (uIElement is GameCard) // If the element is a GameCard
                    {
                        GameCard gameCard = (GameCard)uIElement; // Convert the element to a GameCard
                        if ((gameCard.CheckBox.IsChecked ?? true) && (gameCard.CheckBox.Visibility == Visibility.Visible)) // If the element is checked
                        {
                            games.Add(gameCard); // Add to the list the GameCard
                        }
                    }
                }

                foreach (GameCard gameCard1 in games) // For each games in the list
                {
                    if (gameCard1.GameInfo.IsFavorite) // If the game is a favorite
                    {
                        List<FavoriteGameCard> favoriteGameCards = new List<FavoriteGameCard>();
                        foreach (FavoriteGameCard favoriteGameCard in FavoriteBar.Children) // Foreach favorite
                        {
                            favoriteGameCards.Add(favoriteGameCard); // Add to the list
                        }

                        foreach (FavoriteGameCard favoriteGameCard1 in favoriteGameCards)
                        {
                            if (favoriteGameCard1.GameInfo == gameCard1.GameInfo) // If the favorite is corresponding to the game
                            {
                                FavoriteBar.Children.Remove(favoriteGameCard1); // Remove the favorite
                            }
                        }
                    }
                    Definitions.GamesCardsPages.GamePresenter.Children.Remove(gameCard1); // Remove the game
                    Definitions.Games.Remove(gameCard1.GameInfo); // Remove the game
                    new GameSaver().Save(Definitions.Games); // Update the save file
                    Definitions.RecentGamesPage.LoadGames(); // Reload the games
                    Definitions.GamesListPage.LoadGames(); // Reload the page
                }
            }

            if (Definitions.GamesCardsPages.GamePresenter.Children.Count <= 0) // If there is no items
            {
                WelcomeAddGames welcomeAddGames = new WelcomeAddGames(); // New WelcomeAddGames
                welcomeAddGames.VerticalAlignment = VerticalAlignment.Stretch; // Center
                welcomeAddGames.HorizontalAlignment = HorizontalAlignment.Stretch; // Center
                Definitions.GamesCardsPages.WelcomeHost.Visibility = Visibility.Visible; // Visible
                Definitions.GamesCardsPages.GamePresenter.Visibility = Visibility.Collapsed; // Hidden
                Definitions.GamesCardsPages.WelcomeHost.Children.Add(welcomeAddGames); // Add the welcome screen
            }
        }

        PopupMenu PopupMenu = new PopupMenu(); // The menu
        private void MoreBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Definitions.IsMenuShown) // If the menu is visible
            {
                PopupMenu.Hide(); // Close
                Definitions.IsMenuShown = false; // Is not shown
            }
            else
            {
                PopupMenu.WindowStartupLocation = WindowStartupLocation.Manual; // Set the startup position to manual
                PopupMenu.Left = PointToScreen(Mouse.GetPosition(this)).X - PopupMenu.Width / 2; // Calculate the X position
                PopupMenu.Top = PointToScreen(Mouse.GetPosition(this)).Y + 5; // Calculate the Y position
                PopupMenu.Show(); // Show
                Definitions.IsMenuShown = true; // Is shown
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DefineMaximumSize(); // Define the maximum size of the window
        }

        private void Window_LocationChanged(object sender, EventArgs e)
        {
            DefineMaximumSize(); // Define the maximum size of the window
        }
    }
}
