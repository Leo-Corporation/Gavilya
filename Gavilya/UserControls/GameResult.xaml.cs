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

namespace Gavilya.UserControls
{
    /// <summary>
    /// Logique d'interaction pour GameResult.xaml
    /// </summary>
    public partial class GameResult : UserControl
    {
        public GameResult(string gameName, int id)
        {
            InitializeComponent();
            GameName.Text = gameName; // Put the name of the game
            Id = id; // Define the Game Id
        }

        /// <summary>
        /// The game's Id.
        /// </summary>
        public int Id { get; set; }
    }
}
