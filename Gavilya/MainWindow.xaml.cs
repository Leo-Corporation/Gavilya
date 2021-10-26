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
using Gavilya.Pages;
using Gavilya.UserControls;
using Gavilya.Windows;
using LeoCorpLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

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

			GamesCardsPages gamesCardsPages = new(); // GamesCardsPage
			Definitions.GamesCardsPages = gamesCardsPages; // Define the GamesCardsPage

			Definitions.LibraryPage.PageDisplayer.Content = Definitions.Settings.PageId switch
			{
				0 => Definitions.GamesCardsPages,
				1 => Definitions.RecentGamesPage,
				2 => Definitions.GamesListPage,
				_ => Definitions.GamesCardsPages
			}; // Show the page

			Definitions.LibraryPage.CheckedButton = Definitions.Settings.PageId switch
			{
				0 => Definitions.LibraryPage.GameCardTabBtn,
				1 => Definitions.LibraryPage.RecentTabBtn,
				2 => Definitions.LibraryPage.GameListTabBtn,
				_ => Definitions.LibraryPage.GameCardTabBtn
			}; // Check

			Definitions.LibraryPage.RefreshTabUI();

			Definitions.MainWindow = this; // Define the Main Window
			Global.SortGames();

			LoadGames();
			WindowState = Definitions.Settings.IsMaximized ? WindowState.Maximized : WindowState.Normal; // Set the window state
			RefreshMaximizeRestoreButton(); // Refresh

			RefreshNavigationsButton(); // Refresh the navigations button state
			LoadProfilesUI(); // Load the profile picture
			Global.AutoSave(); // Run autosave

			CheckUpdateOnStart(); // Check update on start

			// Tabs
			PageContent.Navigate(Definitions.HomePage);
			CheckedTabButton = HomeTabBtn;
			CheckButton();
		}
		System.Windows.Forms.NotifyIcon notifyIcon = new System.Windows.Forms.NotifyIcon();
		private async void CheckUpdateOnStart()
		{
			if (await NetworkConnection.IsAvailableAsync())
			{
				notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(AppDomain.CurrentDomain.BaseDirectory + @"\Gavilya.exe");
				notifyIcon.BalloonTipClicked += (o, e) =>
				{
					new UpdateAvailable().Show();
				};

				if (Update.IsAvailable(await Update.GetLastVersionAsync(Definitions.LastVersionLink), Definitions.Version))
				{
					notifyIcon.Visible = true; // Show
					notifyIcon.ShowBalloonTip(5000, Properties.Resources.MainWindowTitle, Properties.Resources.UpdateAvMessageNotify, System.Windows.Forms.ToolTipIcon.Info);
					notifyIcon.Visible = false; // Hide
				}
			}
		}

		/// <summary>
		/// Refresh the navigations button state.
		/// </summary>
		private void RefreshNavigationsButton()
		{
			BackBtn.IsEnabled = PageContent.CanGoBack; // Enable or not the button
			ForwardBtn.IsEnabled = PageContent.CanGoForward; // Enable or not the button

			BackBtn.Foreground = BackBtn.IsEnabled ? new SolidColorBrush { Color = Colors.White } : new SolidColorBrush { Color = Color.FromRgb(198, 198, 198) }; // Define the color
			ForwardBtn.Foreground = ForwardBtn.IsEnabled ? new SolidColorBrush { Color = Colors.White } : new SolidColorBrush { Color = Color.FromRgb(198, 198, 198) }; // Define the color

		}

		internal void LoadProfilesUI()
		{
			if (Definitions.Profiles[Definitions.Settings.CurrentProfileIndex].PictureFilePath != "_default")
			{
				if (File.Exists(Definitions.Profiles[Definitions.Settings.CurrentProfileIndex].PictureFilePath))
				{
					var bitmap = new BitmapImage();
					var stream = File.OpenRead(Definitions.Profiles[Definitions.Settings.CurrentProfileIndex].PictureFilePath);

					bitmap.BeginInit();
					bitmap.CacheOption = BitmapCacheOption.OnLoad;
					bitmap.StreamSource = stream;
					bitmap.EndInit();
					stream.Close();
					stream.Dispose();
					bitmap.Freeze();

					ProfilePicture.ImageSource = bitmap; // Set image
				}
			}
			else
			{
				ProfilePicture.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Assets/DefaultPP.png")); // Set image
			}
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

			float dpiX, dpiY;
			double scaling = 100; // Default scaling = 100%

			using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromHwnd(IntPtr.Zero))
			{
				dpiX = graphics.DpiX; // Get the DPI
				dpiY = graphics.DpiY; // Get the DPI

				scaling = dpiX switch
				{
					96 => 100, // Get the %
					120 => 125, // Get the %
					144 => 150, // Get the %
					168 => 175, // Get the %
					192 => 200, // Get the % 
					_ => 100
				};
			}

			double factor = scaling / 100d; // Calculate factor

			MaxHeight = currentScreen.WorkingArea.Height / factor; // Set max size
			MaxWidth = currentScreen.WorkingArea.Width / factor; // Set max size
		}

		private void Window_StateChanged(object sender, EventArgs e)
		{
			RefreshMaximizeRestoreButton(); // Refresh
			DefineMaximumSize();
			Definitions.Settings.IsMaximized = WindowState switch
			{
				WindowState.Maximized => true,
				WindowState.Normal => false,
				WindowState.Minimized => false,
				_ => false
			};
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			WindowState = WindowState.Minimized; // Minimize
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

		/// <summary>
		/// Put a shadow under an <see cref="UIElement"/>.
		/// </summary>
		/// <param name="uIElement">The <see cref="UIElement"/> to put a shadow to.</param>
		private void ShadowElement(UIElement uIElement)
		{
			DropShadowEffect dropShadowEffect = new(); // Shadow Effect

			Color color = new(); // Color of the shadow
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
			DropShadowEffect dropShadowEffect = new(); // Shadow Effect

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


		private void SelectBtn_Click(object sender, RoutedEventArgs e)
		{
			if (PageContent.Content is GamesCardsPages && Definitions.GamesCardsPages.GamePresenter.Children.Count > 0) // If there is game(s)
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
				HideAllCheckboxes(); // Hide all checkboxes
				ColorElement(SelectBtn, new SolidColorBrush { Color = Colors.Transparent }); // Change the background
			}
		}

		private void HideAllCheckboxes()
		{
			for (int i = 0; i < Definitions.GamesCardsPages.GamePresenter.Children.Count; i++) // For each element
			{
				if (Definitions.GamesCardsPages.GamePresenter.Children[i] is GameCard) // If the element is a GameCard
				{
					GameCard gameCard = (GameCard)Definitions.GamesCardsPages.GamePresenter.Children[i];
					gameCard.CheckBox.Visibility = Visibility.Hidden; // The checkbox isn't visible
					ColorElement(SelectBtn, new SolidColorBrush { Color = Colors.Transparent }); // Change the background
					RemoveShadowElement(SelectBtn); // Remove shadow

					Definitions.IsGamesCardsPagesCheckBoxesVisible = gameCard.CheckBox.IsVisible; // Set the property
				}
			}
		}

		/// <summary>
		/// Checks if games are selected or not.
		/// </summary>
		/// <returns></returns>
		private bool IsGameCardsSelected()
		{
			for (int i = 0; i < Definitions.GamesCardsPages.GamePresenter.Children.Count; i++)
			{
				var game = (GameCard)Definitions.GamesCardsPages.GamePresenter.Children[i];
				if (game.CheckBox.IsChecked.Value)
				{
					return true;
				}
			}
			return false;
		}

		private void DeleteBtn_Click(object sender, RoutedEventArgs e)
		{
			if (Definitions.GamesCardsPages.GamePresenter.Children.Count > 0 && IsGameCardsSelected())
			{
				if (MessageBox.Show(Properties.Resources.DeleteConfirmMessage, Properties.Resources.MainWindowTitle, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
				{
					Definitions.GamesCardsPages.WelcomeHost.Visibility = Visibility.Collapsed; // Hidden
					Definitions.GamesCardsPages.GamePresenter.Visibility = Visibility.Visible; // Visible
					List<GameCard> games = new(); // List of all the games

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
							List<FavoriteGameCard> favoriteGameCards = new();
							foreach (FavoriteGameCard favoriteGameCard in Definitions.HomePage.FavoriteBar.Children) // Foreach favorite
							{
								favoriteGameCards.Add(favoriteGameCard); // Add to the list
							}

							foreach (FavoriteGameCard favoriteGameCard1 in favoriteGameCards)
							{
								if (favoriteGameCard1.GameInfo == gameCard1.GameInfo) // If the favorite is corresponding to the game
								{
									Definitions.HomePage.FavoriteBar.Children.Remove(favoriteGameCard1); // Remove the favorite
								}
							}
						}
						Definitions.GamesCardsPages.GamePresenter.Children.Remove(gameCard1); // Remove the game
						Definitions.Games.Remove(gameCard1.GameInfo); // Remove the game
						new GameSaver().Save(Definitions.Games); // Update the save file
						Definitions.RecentGamesPage.LoadGames(); // Reload the games
						Definitions.GamesListPage.LoadGames(); // Reload the page
					}

					if (Definitions.GamesCardsPages.GamePresenter.Children.Count <= 0) // If there is no items
					{
						WelcomeAddGames welcomeAddGames = new(); // New WelcomeAddGames
						welcomeAddGames.VerticalAlignment = VerticalAlignment.Stretch; // Center
						welcomeAddGames.HorizontalAlignment = HorizontalAlignment.Stretch; // Center
						Definitions.GamesCardsPages.WelcomeHost.Visibility = Visibility.Visible; // Visible
						Definitions.GamesCardsPages.GamePresenter.Visibility = Visibility.Collapsed; // Hidden
						Definitions.GamesCardsPages.WelcomeHost.Children.Add(welcomeAddGames); // Add the welcome screen
					}
				}
			}
		}

		PopupMenu PopupMenu = new(); // The menu
		private void MoreBtn_Click(object sender, RoutedEventArgs e)
		{
			if (Definitions.IsMenuShown) // If the menu is visible
			{
				PopupMenu.Hide(); // Close
				Definitions.IsMenuShown = false; // Is not shown
			}
			else
			{
				double factor = PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice.M11; // Get factor for DPI


				PopupMenu.WindowStartupLocation = WindowStartupLocation.Manual; // Set the startup position to manual
				PopupMenu.Left = (PointToScreen(Mouse.GetPosition(this)).X - PopupMenu.Width / 2) / factor; // Calculate the X position
				PopupMenu.Top = PointToScreen(Mouse.GetPosition(this)).Y / factor + 5; // Calculate the Y position
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

		private void BackBtn_Click(object sender, RoutedEventArgs e)
		{
			if (PageContent.CanGoBack) // If can go back
			{
				PageContent.GoBack(); // Go back
			}
			RefreshNavigationsButton(); // Refresh the navigations button state
		}

		private void ForwardBtn_Click(object sender, RoutedEventArgs e)
		{
			if (PageContent.CanGoForward) // If can go forward
			{
				PageContent.GoForward(); // Go forward
			}
			RefreshNavigationsButton(); // Refresh the navigations button state
		}

		private void PageContent_Navigated(object sender, NavigationEventArgs e)
		{
			RefreshNavigationsButton(); // Refresh the navigations button state
			RefreshTabUI(); // Refresh the tab UI status
		}

		private void RefreshTabUI()
		{
			if (PageContent.Content is HomePage)
			{
				CheckedTabButton = HomeTabBtn; // Check
			}
			else if (PageContent.Content is LibraryPage)
			{
				CheckedTabButton = LibraryTabBtn; // Check
			}
			CheckButton(); // Refresh
		}

		internal ProfilesPopupMenu ProfilesPopupMenu = new();
		private void ProfileBtn_Click(object sender, RoutedEventArgs e)
		{
			if (Definitions.IsProfileMenuVisible) // If the menu is visible
			{
				ProfilesPopupMenu.Hide(); // Close
				Definitions.IsProfileMenuVisible = false; // Is not shown
			}
			else
			{
				double factor = PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice.M11; // Get factor for DPI

				ProfilesPopupMenu.WindowStartupLocation = WindowStartupLocation.Manual; // Set the startup position to manual
				ProfilesPopupMenu.Left = (PointToScreen(Mouse.GetPosition(this)).X - ProfilesPopupMenu.Width / 2) / factor; // Calculate the X position
				ProfilesPopupMenu.Top = PointToScreen(Mouse.GetPosition(this)).Y / factor + 5; // Calculate the Y position
				ProfilesPopupMenu.Show(); // Show
				Definitions.IsProfileMenuVisible = true; // Is shown
			}
		}

		Button CheckedTabButton { get; set; }
		private void HomeTabBtn_Click(object sender, RoutedEventArgs e)
		{
			CheckedTabButton = HomeTabBtn; // Set the checked button
			CheckButton(); // Update the UI

			PageContent.Navigate(Definitions.HomePage); // Show the Home page
		}

		private void LibraryTabBtn_Click(object sender, RoutedEventArgs e)
		{
			CheckedTabButton = LibraryTabBtn; // Set the checked button
			CheckButton(); // Update the UI

			PageContent.Navigate(Definitions.LibraryPage); // Show the Library page
		}

		private void ProfileTabBtn_Click(object sender, RoutedEventArgs e)
		{
			CheckedTabButton = ProfileTabBtn; // Set the checked button
			CheckButton(); // Update the UI

			//TODO: Profile page
		}

		private void HomeTabBtn_MouseLeave(object sender, MouseEventArgs e)
		{
			Button button = (Button)sender; // Get the button
			if (button == CheckedTabButton)
			{
				button.Background = new SolidColorBrush { Color = Color.FromRgb(102, 0, 255) };
			}
		}

		private void CheckButton()
		{
			// Reset
			HomeTabBtn.Background = new SolidColorBrush { Color = Colors.Transparent }; // Reset the background color
			LibraryTabBtn.Background = new SolidColorBrush { Color = Colors.Transparent }; // Reset the background color
			ProfileTabBtn.Background = new SolidColorBrush { Color = Colors.Transparent }; // Reset the background color

			CheckedTabButton.Background = new SolidColorBrush { Color = Color.FromRgb(102, 0, 255) }; // Check
		}
	}
}
