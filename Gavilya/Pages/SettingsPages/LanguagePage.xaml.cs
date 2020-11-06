using Gavilya.Classes;
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

namespace Gavilya.Pages.SettingsPages
{
    /// <summary>
    /// Logique d'interaction pour LanguagePage.xaml
    /// </summary>
    public partial class LanguagePage : Page
    {
        public LanguagePage()
        {
            InitializeComponent();
            Initialize(); // Load the UI
        }

        private void Initialize()
        {
            Languages.Items.Add(Properties.Resources.Default); // Add the default item

            for (int i = 0; i < Definitions.Languages.Count; i++) // For each item
            {
                Languages.Items.Add(Definitions.Languages[i]); // Add an item
            }

            if (Properties.Settings.Default.Language != "_default") // If the language is not default
            {
                Languages.SelectedIndex = Definitions.LanguagesCodes.IndexOf(Properties.Settings.Default.Language) + 1; // Set the selected index
            }
            else
            {
                Languages.SelectedIndex = 0; // Set the selected index
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if ((string)Languages.SelectedItem != Properties.Resources.Default) // If the language is not default
            {
                Properties.Settings.Default.Language = Definitions.LanguagesCodes[Definitions.Languages.IndexOf((string)Languages.SelectedItem)];
            }
            else
            {
                Properties.Settings.Default.Language = "_default"; // Set the language to default
            }
        }
    }
}
