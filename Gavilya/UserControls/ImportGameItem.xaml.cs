using Gavilya.Classes;
using LeoCorpLibrary;
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

		public ImportGameItem(GameInfo gameInfo)
		{
			InitializeComponent();
			GameInfo = gameInfo;

			InitUI(); // Load the UI
		}

		private void InitUI()
		{
			NameTxt.Text = GameInfo.Name; // Set text
			LocationTxt.Text = (GameInfo.FileLocation.Length > 32) ? GameInfo.FileLocation.Substring(0, 32) + "..." : GameInfo.FileLocation; // Set text
			LocationToolTip.Content = GameInfo.FileLocation; // Set content
			IconLocationTxt.Text = (GameInfo.IconFileLocation.Length > 32) ? GameInfo.IconFileLocation.Substring(0, 32) + "..." : GameInfo.IconFileLocation; // Set text
			IconLocationToolTip.Content = GameInfo.IconFileLocation; // Set content

			GetRAWGImageBtn.Visibility = (GameInfo.RAWGID != -1) ? Visibility.Visible : Visibility.Collapsed; // Set
		}

		private void BrowseBtn_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new(); // OpenFileDialog
			openFileDialog.Filter = "EXE|*.exe"; // Filter

			if (openFileDialog.ShowDialog() ?? true)
			{
				GameInfo.FileLocation = openFileDialog.FileName; // Set value
				InitUI();
			}
		}

		private void IconBrowseBtn_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new(); // OpenFileDialog
			openFileDialog.Filter = "PNG|*.png|JPG|*.jpg|Bitmap|*.bmp|All Files|*.*"; // Filter

			if (openFileDialog.ShowDialog() ?? true)
			{
				try
				{
					GameInfo.IconFileLocation = openFileDialog.FileName; // Set
					InitUI();
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); // Show the error
				}
			}
		}

		private async void GetRAWGImageBtn_Click(object sender, RoutedEventArgs e)
		{
			if (await NetworkConnection.IsAvailableAsync())
			{
				GameInfo.IconFileLocation = await Global.GetCoverImageAsync(GameInfo.RAWGID); // Set path
				InitUI(); // Refresh the UI 
			}
			else
			{
				MessageBox.Show(Properties.Resources.FeatureNeedsInternet, Properties.Resources.MainWindowTitle, MessageBoxButton.OK, MessageBoxImage.Information);
			}
		}
	}
}
