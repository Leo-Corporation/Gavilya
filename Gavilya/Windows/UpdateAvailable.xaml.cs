using Gavilya.Classes;
using LeoCorpLibrary;
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
using System.Windows.Shapes;

namespace Gavilya.Windows
{
    /// <summary>
    /// Logique d'interaction pour UpdateAvailable.xaml
    /// </summary>
    public partial class UpdateAvailable : Window
    {
        public UpdateAvailable()
        {
            InitializeComponent();
            DisplayLastVersion();
        }

        private async void DisplayLastVersion()
        {
            VersionTxt.Text = $"{Properties.Resources.UpdateVersion} {await Update.GetLastVersionAsync(Definitions.LastVersionLink)}"; // Show the last version
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close(); // Close the window
        }

        private void InstallBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Close(); // Close the window
        }
    }
}
