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
using Gavilya.Pages;
using System.Collections.Generic;
using System.Threading;
using System.Windows;

namespace Gavilya.Windows;

/// <summary>
/// Logique d'interaction pour AddGame.xaml
/// </summary>
public partial class AddGame : Window
{
	public string GameIconLocation = string.Empty;
	public string GameDescription = string.Empty; // The description of the game
	public List<SDK.RAWG.Platform> Platforms = new();
	public List<SDK.RAWG.Store> Stores = new();
	public int RAWGID = -1;
	public string GameName = string.Empty;
	public string GameVersion = string.Empty;
	public string GameLocation = string.Empty;
	public bool IsUWP { get; set; }
	public bool IsSteam { get; set; }

	readonly AddEditPage AddEditPage;
	readonly AddEditPage2 AddEditPage2;
	public AddGame(bool isUWP, bool isSteam)
	{
		InitializeComponent();
		IsUWP = isUWP; // Set
		IsSteam = isSteam; // Set
		AddEditPage = new(this);
		AddEditPage2 = new(this);

		ChangePage(0);
	}

	internal void ChangePage(int id)
	{
		Content.Content = id switch
		{
			0 => AddEditPage,
			1 => AddEditPage2,
			_ => AddEditPage
		}; // Set
	}

	private void Window_Loaded(object sender, RoutedEventArgs e)
	{
		Title = Thread.CurrentThread.CurrentUICulture.ToString() switch // Language of the user
		{
			// Language: French (France)
			"fr-FR" => "Ajouter un jeu",// Change the title
										// Language: English (United States)
			"en-US" => "Add a game",// Change the title
									// Language: English (United States)
			_ => "Add a game",// Change the title
		};
	}

	private void Button_Click(object sender, RoutedEventArgs e)
	{
		WindowState = WindowState.Minimized; // Minimize the window
	}

	private void Button_Click_1(object sender, RoutedEventArgs e)
	{
		Close(); // Close the window
	}

	//private void Button_Click_2(object sender, RoutedEventArgs e)
	//{
	//	OpenFileDialog openFileDialog = new(); // OpenFileDialog
	//	openFileDialog.Filter = "PNG|*.png|JPG|*.jpg|Bitmap|*.bmp|All Files|*.*"; // Filter

	//	if (openFileDialog.ShowDialog() ?? true) // If the user selected a file
	//	{
	//		try
	//		{
	//			BitmapImage image = new(new Uri(openFileDialog.FileName)); // Create the image
	//			GameImg.Source = image; // Set the GameImg's source to the image
	//			GameIconLocation = openFileDialog.FileName; // Set the path to the image
	//		}
	//		catch (Exception ex)
	//		{
	//			MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); // Show the error
	//		}
	//	}
	//}

	//private async void BrowseBtn_Click(object sender, RoutedEventArgs e)
	//{
	//	OpenFileDialog openFileDialog = new(); // OpenFileDialog
	//	openFileDialog.Filter = "EXE|*.exe"; // Filter

	//	if (openFileDialog.ShowDialog() ?? true) // If the user selected a file
	//	{
	//		FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(openFileDialog.FileName); // Get the version

	//		nameTxt.Text = string.IsNullOrEmpty(fileVersionInfo.ProductName) ? System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName) : fileVersionInfo.ProductName; // Name of the file
	//		versionTxt.Text = fileVersionInfo.FileVersion; // Version of the file
	//		locationTxt.Text = openFileDialog.FileName; // Location of the file
	//		if (GameIconLocation == string.Empty) // If there is no image
	//		{
	//			try
	//			{
	//				GameIconLocation = await Global.GetCoverImageAsync(string.IsNullOrEmpty(fileVersionInfo.ProductName) ? System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName) : fileVersionInfo.ProductName);

	//				if (GameIconLocation == string.Empty)
	//				{
	//					Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(openFileDialog.FileName); // Grab the icon of the game
	//					GameImg.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()); // Show the image
	//				}
	//				else
	//				{
	//					var bitmap = new BitmapImage(); // Create Bitmap
	//					var stream = File.OpenRead(GameIconLocation); // Create a stream

	//					bitmap.BeginInit(); // Init bitmap
	//					bitmap.CacheOption = BitmapCacheOption.OnLoad;
	//					bitmap.StreamSource = stream;
	//					bitmap.EndInit(); // End init bitmap
	//					stream.Close(); // Close the stream
	//					stream.Dispose(); // Release ressources
	//					bitmap.Freeze(); // Freeze bitmap

	//					GameImg.Source = bitmap; // Show the image
	//				}
	//			}
	//			catch
	//			{

	//			}
	//		}
	//	}
	//}

	//private void AddBtn_Click(object sender, RoutedEventArgs e)
	//{
	//	if (!(string.IsNullOrEmpty(nameTxt.Text) || string.IsNullOrEmpty(locationTxt.Text))) /// If the fields are filled
	//	{
	//		GameInfo gameInfo = new()
	//		{
	//			FileLocation = locationTxt.Text, // The file location of the game
	//			Name = nameTxt.Text, // The name of the game
	//			Version = versionTxt.Text, // The version of the game
	//			IconFileLocation = GameIconLocation, // The location of the icon of the game
	//			IsFavorite = false, // The game is not a favorite by default
	//			RAWGID = RAWGID, // The RAWG Id of the game
	//			Description = GameDescription, // The description of the game
	//			Platforms = (Platforms.Count == 0) ? new List<SDK.RAWG.Platform> { Definitions.DefaultPlatform } : Platforms, // Get platforms
	//			LastTimePlayed = 0, // Never played
	//			TotalTimePlayed = 0, // Never played
	//			ProcessName = string.Empty, // Default
	//			Stores = Stores
	//		}; // Create a GameInfo class

	//		Definitions.GamesCardsPages.GamePresenter.Children.Add(new GameCard(gameInfo, GavilyaPages.Cards)); // Add the game
	//		Definitions.Games.Add(gameInfo);
	//		new GameSaver().Save(Definitions.Games);
	//		Global.RemoveWelcomeScreen(); // Remove
	//		Global.SortGames(); // Sort
	//		Definitions.RecentGamesPage.LoadGames(); // Reload the games
	//		Definitions.GamesListPage.LoadGames(); // Reload the page
	//		Close(); // Close the Window
	//	}
	//	else
	//	{
	//		MessageBox.Show(Properties.Resources.GameFieldsEmpty);
	//	}
	//}

	//private void CancelBtn_Click(object sender, RoutedEventArgs e)
	//{
	//	Close(); // Close the Window
	//}

	//private void Button_Click_3(object sender, RoutedEventArgs e)
	//{
	//	new SearchGameCover(this, GameAssociationActions.Search).Show(); // Show the window
	//}

	//private void AssociateGameLink_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	//{
	//	new SearchGameCover(this, GameAssociationActions.Associate).Show(); // Show the window
	//}

	//private void DescriptionLink_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	//{
	//	new DescriptionWindow(GameDescription, this).Show(); // Show the Description window
	//}
}
