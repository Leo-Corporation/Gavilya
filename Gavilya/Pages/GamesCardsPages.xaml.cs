using System;
using System.Collections.Generic;
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
    }
}
