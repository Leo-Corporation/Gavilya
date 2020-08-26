using Gavilya.Classes;
using Gavilya.Pages;
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
            SetLanguageDictonnary(); // Set the language of the app.
            GamesCardsPages gamesCardsPages = new GamesCardsPages(); // GamesCardsPage
            Definitions.GamesCardsPages = gamesCardsPages; // Define the GamesCardsPage
            PageContent.Content = gamesCardsPages; // Show the page
            Definitions.MainWindow = this; // Define the Main Window
            LoadPage(); // Load the button on the button corresponding to the active page
        }

        /// <summary>
        /// Set the language of the application.
        /// </summary>
        private void SetLanguageDictonnary()
        {
            ResourceDictionary resourceDictionary = new ResourceDictionary(); // Ressource dictonnary

            switch (Thread.CurrentThread.CurrentUICulture.ToString()) // For each case
            {
                case "en-US": // Language: English (United States)
                    resourceDictionary.Source = new Uri("\\Resources\\StringsRessources.xaml", UriKind.Relative); // Set the source
                    break;
                case "fr-FR": // Language: French (France)
                    resourceDictionary.Source = new Uri("\\Resources\\StringsRessources.fr-FR.xaml", UriKind.Relative); // Set the source
                    break;
                default: // Languagae (default): English (United States)
                    resourceDictionary.Source = new Uri("\\Resources\\StringsRessources.xaml", UriKind.Relative); // Set the source
                    break;
            }
            Application.Current.Resources.MergedDictionaries.Add(resourceDictionary); // Add the dictonnary to the ressources of the application
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
            Close(); // Close
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
            }
            else
            {
                maximizeButton.Visibility = Visibility.Visible; // Show
                restoreButton.Visibility = Visibility.Collapsed; // Hide
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
        }

        private void AppListButton_Click(object sender, RoutedEventArgs e)
        {
            RemoveShadowElement(AppCardButton); // Remove the shadow effect from other buttons
            RemoveShadowElement(RecentButton); // Remove the shadow effect from other buttons

            ColorElement(AppCardButton, new SolidColorBrush(Color.FromRgb(90, 90, 112))); // Change the backcolor
            ColorElement(RecentButton, new SolidColorBrush(Color.FromRgb(90, 90, 112))); // Change the backcolor

            ShadowElement(AppListButton); // Put a shadow under the control

            ColorElement(AppListButton, Definitions.HomeButtonBackColor); // Change the background
        }
    }
}
