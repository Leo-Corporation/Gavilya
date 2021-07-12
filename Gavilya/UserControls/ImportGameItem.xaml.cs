using Gavilya.Classes;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
	/// Interaction logic for ImportGameItem.xaml
	/// </summary>
	public partial class ImportGameItem : UserControl
	{
		public GameInfo GameInfo { get; set; }

		StackPanel StackPanel { get; init; }
		public ImportGameItem(GameInfo gameInfo, StackPanel stackPanel)
		{
			InitializeComponent();
			GameInfo = gameInfo;
			StackPanel = stackPanel;

			InitUI(); // Load the UI
		}

		private void InitUI()
		{
			NameTxt.Text = GameInfo.Name; // Set text
			LocationTxt.Text = (GameInfo.FileLocation.Length > 32) ? GameInfo.FileLocation.Substring(0, 32) + "..." : GameInfo.FileLocation; // Set text
			LocationToolTip.Content = GameInfo.FileLocation; // Set content
		}

		private void BrowseBtn_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new(); // OpenFileDialog
			openFileDialog.Filter = "EXE|*.exe"; // Filter

			if (openFileDialog.ShowDialog() ?? true)
			{
				GameInfo.FileLocation = openFileDialog.FileName; // Set value
				LocationTxt.Text = (GameInfo.FileLocation.Length > 32) ? GameInfo.FileLocation.Substring(0, 32) + "..." : GameInfo.FileLocation; // Set text
				LocationToolTip.Content = openFileDialog.FileName; // Set content
			}
		}

		private void DeleteBtn_Click(object sender, RoutedEventArgs e)
		{
			StackPanel.Children.Remove(this); // Delete
		}
	}
}
