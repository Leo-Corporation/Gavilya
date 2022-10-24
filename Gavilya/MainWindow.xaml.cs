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
using Gma.System.MouseKeyHook;
using LeoCorpLibrary;
using LeoCorpLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace Gavilya;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
	public MainWindow(GavilyaWindowPages? startupPage = null)
	{
		InitializeComponent();

		Title = Properties.Resources.MainWindowTitle;
		Global.SetWindowIcon(this); // Set the icon of the window

		GamesCardsPages gamesCardsPages = new(); // GamesCardsPage
		Definitions.GamesCardsPages = gamesCardsPages; // Define the GamesCardsPage

		Definitions.LibraryPage.PageDisplayer.Content = Definitions.Settings.PageId switch
		{
			0 => Definitions.GamesCardsPages,
			2 => Definitions.GamesListPage,
			_ => Definitions.GamesCardsPages
		}; // Show the page

		Definitions.LibraryPage.CheckedButton = Definitions.Settings.PageId switch
		{
			0 => Definitions.LibraryPage.GameCardTabBtn,
			2 => Definitions.LibraryPage.GameListTabBtn,
			_ => Definitions.LibraryPage.GameCardTabBtn
		}; // Check

		Definitions.LibraryPage.RefreshTabUI();

		Definitions.MainWindow = this; // Define the Main Window
		Global.SortGames();

		new System.Threading.Thread(delegate ()
		{
			LoadGames();
		}).Start();
		WindowState = Definitions.Settings.IsMaximized ? WindowState.Maximized : WindowState.Normal; // Set the window state
		RefreshMaximizeRestoreButton(); // Refresh

		RefreshNavigationsButton(); // Refresh the navigations button state
		LoadProfilesUI(); // Load the profile picture
		Global.AutoSave(); // Run autosave

		CheckUpdateOnStart(); // Check update on start

		// Tabs

		PageContent.Navigate((startupPage ?? Definitions.Settings.DefaultGavilyaHomePage) switch
		{
			GavilyaWindowPages.Home => Definitions.HomePage,
			GavilyaWindowPages.Library => Definitions.LibraryPage,
			GavilyaWindowPages.Profile => Definitions.ProfilePage,
			GavilyaWindowPages.Recent => Definitions.RecentGamesPage,
			_ => Definitions.HomePage
		});
		CheckedTabButton = (startupPage ?? Definitions.Settings.DefaultGavilyaHomePage) switch
		{
			GavilyaWindowPages.Home => HomeTabBtn,
			GavilyaWindowPages.Library => LibraryTabBtn,
			GavilyaWindowPages.Profile => ProfileBtn,
			GavilyaWindowPages.Recent => RecentTabBtn,
			_ => HomeTabBtn
		};
		CheckButton();
		DisplayNotifications();

		// Search box
		SearchBox.Visibility = Definitions.Settings.HideSearchBar.Value ? Visibility.Collapsed : Visibility.Visible; // Hide
		SearchBtn.Visibility = !Definitions.Settings.HideSearchBar.Value ? Visibility.Collapsed : Visibility.Visible; // Hide
		SearchBox.MaxDropDownHeight = Definitions.Settings.NumberOfSearchResultsToDisplay.Value * 45; // Set the max drop down height (45 = height of SearchItem)

		// FPS
		var fps = Combination.FromString("Control+Shift+F");

		Action fpsAction = () =>
		{
			var proc = new Process();
			proc.StartInfo.FileName = "cmd.exe";
			proc.StartInfo.Arguments = !Definitions.IsFpsToggled ? $"/c \"{Env.CurrentAppDirectory}/Gavilya.Fps.exe\" {Definitions.Settings.FpsCounterOpacity}" : "/c taskkill /f /im Gavilya.Fps.exe";
			proc.StartInfo.UseShellExecute = false;
			proc.StartInfo.CreateNoWindow = true;
			proc.Start();
			Definitions.IsFpsToggled = !Definitions.IsFpsToggled;
		};
		var assignment = new Dictionary<Combination, Action>
		{
			{ fps, fpsAction }
		};
		Hook.GlobalEvents().OnCombination(assignment);

		// Add Popup
		if (Env.WindowsVersion != WindowsVersion.Windows10 && Env.WindowsVersion != WindowsVersion.Windows11)
		{
			AddUWPBtn.Visibility = Visibility.Collapsed; // Hide
		}
		else
		{
			AddUWPBtn.Visibility = Visibility.Visible; // Show
		}
	}

	readonly System.Windows.Forms.NotifyIcon notifyIcon = new();

	private void DisplayNotifications()
	{
		// Unused games notification
		if (Definitions.LeastUsedGames is not null)
		{
			NotificationPanel.Children.Add(
				new NotificationItem(Properties.Resources.UnusedGameNotification,
				string.Format(Properties.Resources.UnusedGame, Definitions.LeastUsedGames.Keys.ElementAt(0).Name),
				"\uF451", Properties.Resources.Show, Properties.Resources.Close,
				() => { Definitions.GameInfoPage.InitializeUI(Definitions.LeastUsedGames.Keys.ElementAt(0), Definitions.LeastUsedGames.Values.ElementAt(0)); Definitions.MainWindow.PageContent.Navigate(Definitions.GameInfoPage); }, null));
		}
	}

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

				NotificationPanel.Children.Add(new NotificationItem(Properties.Resources.UpdateAv, Properties.Resources.UpdateAvMessageNotify, "\uF191", Properties.Resources.Install, Properties.Resources.Cancel, () => { new UpdateAvailable().Show(); }, null));
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
				bitmap.DecodePixelWidth = 30;
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

	private static void LoadGames()
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

	private void MaximizeButton_Click(object sender, RoutedEventArgs e)
	{
		WindowState = WindowState.Maximized; // Maximize
	}

	private void RestoreButton_Click(object sender, RoutedEventArgs e)
	{
		WindowState = WindowState.Normal; // Restore
	}

	private void Button_Click_2(object sender, RoutedEventArgs e)
	{
		SettingsSaver.Save(); // Save settings

		Global.CreateJumpLists();
		Application.Current.Shutdown(); // Quit the app
	}

	/// <summary>
	/// Refresh the visibility of the Restore/Maximize buttons of the <see cref="Window"/>.
	/// </summary>
	private void RefreshMaximizeRestoreButton()
	{
		if (WindowState == WindowState.Maximized) // If Maximized
		{
			MaximizeButton.Visibility = Visibility.Collapsed; // Hide
			RestoreButton.Visibility = Visibility.Visible; // Show
		}
		else
		{
			MaximizeButton.Visibility = Visibility.Visible; // Show
			RestoreButton.Visibility = Visibility.Collapsed; // Hide
		}
		WindowBorder.Margin = WindowState == WindowState.Maximized ? new(10, 10, 0, 0) : new(10); // Set
	}

	/// <summary>
	/// Put a shadow under an <see cref="UIElement"/>.
	/// </summary>
	/// <param name="uIElement">The <see cref="UIElement"/> to put a shadow to.</param>
	private static void ShadowElement(UIElement uIElement)
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
	private static void RemoveShadowElement(UIElement uIElement)
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
	private static void ColorElement(Button button, Brush color)
	{
		button.Background = color;
	}


	private void SelectBtn_Click(object sender, RoutedEventArgs e)
	{
		if (PageContent.Content is LibraryPage && Definitions.LibraryPage.PageDisplayer.Content is GamesCardsPages && Definitions.GamesCardsPages.GamePresenter.Children.Count > 0) // If there is game(s)
		{
			for (int i = 0; i < Definitions.GamesCardsPages.GamePresenter.Children.Count; i++) // For each element
			{
				if (Definitions.GamesCardsPages.GamePresenter.Children[i] is GameCard gameCard) // If the element is a GameCard
				{
					if (gameCard.SelectModeToggled) // If the check box is visible
					{
						gameCard.CheckBox.Visibility = Visibility.Hidden; // The checkbox isn't visible
						ColorElement(SelectBtn, new SolidColorBrush { Color = Colors.Transparent }); // Change the background
						gameCard.GameCardBorder.BorderThickness = new(0); // Set the border thickness; // Show the controls
						RemoveShadowElement(SelectBtn); // Remove shadow
					}
					else
					{
						gameCard.CheckBox.Visibility = Visibility.Visible; // The checkbox is visible
						gameCard.GameCardBorder.BorderThickness = gameCard.CheckBox.IsChecked ?? false ? new(3) : new(0); // Set the border thickness; // Show the controls
						ColorElement(SelectBtn, Definitions.HomeButtonBackColor); // Change the background
						ShadowElement(SelectBtn); // Shadow
					}
					gameCard.SelectModeToggled = !gameCard.SelectModeToggled;
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
			if (Definitions.GamesCardsPages.GamePresenter.Children[i] is GameCard gameCard) // If the element is a GameCard
			{
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
	private static bool IsGameCardsSelected()
	{
		for (int i = 0; i < Definitions.GamesCardsPages.GamePresenter.Children.Count; i++)
		{
			var game = (GameCard)Definitions.GamesCardsPages.GamePresenter.Children[i];
			if (game.CheckBox.IsChecked.Value && game.SelectModeToggled)
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
					// Convert the element to a GameCard
					if (uIElement is GameCard gameCard) // If the element is a GameCard
					{
						if (gameCard.CheckBox.IsChecked.Value && (gameCard.CheckBox.Visibility == Visibility.Visible)) // If the element is checked
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
						List<FavoriteSideBarItem> favoriteSideBarItems = new();
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

						foreach (FavoriteSideBarItem favoriteSideBarItem in Definitions.MainWindow.FavoriteSideBar.Children)
						{
							favoriteSideBarItems.Add(favoriteSideBarItem); // Add to the list
							
						}
						foreach (FavoriteSideBarItem favoriteSideBarItem1 in favoriteSideBarItems)
						{
							if (favoriteSideBarItem1.Parent is GameCard && (GameCard)favoriteSideBarItem1.Parent == gameCard1)
							{
								Definitions.MainWindow.FavoriteSideBar.Children.Remove(favoriteSideBarItem1);
							}
						}
					}

					// Clear search box
					// 1. Find the SearchItem in the SearchBox
					// 2. Remove the SearchItem

					for (int i = 0; i < SearchBox.Items.Count; i++)
					{
						if (SearchBox.Items[i] is SearchItem searchItem)
						{
							if (searchItem.ParentGameCard == gameCard1)
							{
								SearchBox.Items.Remove(searchItem);
							}
						}
					}

					Definitions.GamesCardsPages.GamePresenter.Children.Remove(gameCard1); // Remove the game
					Definitions.Games.Remove(gameCard1.GameInfo); // Remove the game
					GameSaver.Save(Definitions.Games); // Update the save file
					Definitions.RecentGamesPage.LoadGames(); // Reload the games
					Definitions.GamesListPage.LoadGames(); // Reload the page
				}

				if (Definitions.GamesCardsPages.GamePresenter.Children.Count <= 0) // If there is no items
				{
					WelcomeAddGames welcomeAddGames = new()
					{
						VerticalAlignment = VerticalAlignment.Stretch, // Center
						HorizontalAlignment = HorizontalAlignment.Stretch // Center
					}; // New WelcomeAddGames
					Definitions.GamesCardsPages.WelcomeHost.Visibility = Visibility.Visible; // Visible
					Definitions.GamesCardsPages.GamePresenter.Visibility = Visibility.Collapsed; // Hidden
					Definitions.GamesCardsPages.WelcomeHost.Children.Add(welcomeAddGames); // Add the welcome screen
				}
			}
		}
	}

	readonly PopupMenu PopupMenu = new(); // The menu
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
		else if (PageContent.Content is LibraryPage or GameInfoPage)
		{
			CheckedTabButton = LibraryTabBtn; // Check
		}
		else if (PageContent.Content is ProfilePage)
		{
			CheckedTabButton = ProfileBtn; // Check
		}
		else if (PageContent.Content is RecentGamesPage)
		{
			CheckedTabButton = RecentTabBtn; // Check
		}
		CheckButton(); // Refresh
	}

	private void ProfileBtn_Click(object sender, RoutedEventArgs e)
	{

		CheckedTabButton = ProfileBtn; // Set the checked button
		CheckButton(); // Update the UI

		Definitions.ProfilePage.InitUI(); // Refresh the content
		PageContent.Navigate(Definitions.ProfilePage); // Show the Library page
	}

	Button CheckedTabButton { get; set; }
	private void HomeTabBtn_Click(object sender, RoutedEventArgs e)
	{
		CheckedTabButton = HomeTabBtn; // Set the checked button
		CheckButton(); // Update the UI

		Definitions.Statistics.InitUI(); // Refresh
		PageContent.Navigate(Definitions.HomePage); // Show the Home page
	}

	private void LibraryTabBtn_Click(object sender, RoutedEventArgs e)
	{
		CheckedTabButton = LibraryTabBtn; // Set the checked button
		CheckButton(); // Update the UI

		PageContent.Navigate(Definitions.LibraryPage); // Show the Library page
	}

	private void HomeTabBtn_MouseLeave(object sender, MouseEventArgs e)
	{
		Button button = (Button)sender; // Get the button
		if (button == CheckedTabButton)
		{
			button.BorderBrush = new SolidColorBrush { Color = Color.FromRgb(102, 0, 255) };
			CheckedTabButton.Background = new SolidColorBrush { Color = Color.FromRgb(40, 40, 60) }; // Check
		}
	}

	private void CheckButton()
	{
		// Reset
		HomeTabBtn.BorderBrush = new SolidColorBrush { Color = Colors.Transparent }; // Reset the background color
		LibraryTabBtn.BorderBrush = new SolidColorBrush { Color = Colors.Transparent }; // Reset the background color
		ProfileBtn.BorderBrush = new SolidColorBrush { Color = Colors.Transparent }; // Reset the background color
		RecentTabBtn.BorderBrush = new SolidColorBrush { Color = Colors.Transparent }; // Reset the background color
		SettingsBtn.BorderBrush = new SolidColorBrush { Color = Colors.Transparent }; // Reset the background color

		HomeTabBtn.Background = new SolidColorBrush { Color = Colors.Transparent }; // Reset the background color
		LibraryTabBtn.Background = new SolidColorBrush { Color = Colors.Transparent }; // Reset the background color
		ProfileBtn.Background = new SolidColorBrush { Color = Colors.Transparent }; // Reset the background color
		RecentTabBtn.Background = new SolidColorBrush { Color = Colors.Transparent }; // Reset the background color
		SettingsBtn.Background = new SolidColorBrush { Color = Colors.Transparent }; // Reset the background color

		CheckedTabButton.BorderBrush = new SolidColorBrush { Color = Color.FromRgb(102, 0, 255) }; // Check
		CheckedTabButton.Background = new SolidColorBrush { Color = Color.FromRgb(40, 40, 60) }; // Check
	}

	private void NotificationsBtn_Click(object sender, RoutedEventArgs e)
	{
		NotificationCenter.Visibility = NotificationCenter.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible; // Set visibility
		NotificationStatusTxt.Text = $"{Properties.Resources.YouHave} {NotificationPanel.Children.Count - 1} {((NotificationPanel.Children.Count - 1 > 1) ? Properties.Resources.NotificationsLower : Properties.Resources.NotificationLower)}";

		if (NotificationPanel.Children.Count - 1 < 1)
		{
			NotificationPlaceholder.Visibility = Visibility.Visible; // Show
			BadgeTxt.Visibility = Visibility.Hidden; // Hide
		}
		else
		{
			NotificationPlaceholder.Visibility = Visibility.Collapsed; // Hide
			BadgeTxt.Visibility = Visibility.Visible; // Show
		}
	}

	private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
	{
		SearchBox.IsDropDownOpen = true;
	}

	private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
	{
		SearchBox.IsDropDownOpen = true;
	}

	private void SearchBox_KeyUp(object sender, KeyEventArgs e)
	{
		if (e.Key == Key.Enter)
		{
			((SearchItem)SearchBox.SelectedItem).UserControl_MouseLeftButtonUp(this, null); // Click the selected item
		}
	}

	internal bool IsSearchVisible { get; set; } = false;
	private void SearchBtn_Click(object sender, RoutedEventArgs e)
	{
		IsSearchVisible = !IsSearchVisible; // Toggle
		if (IsSearchVisible)
		{
			ColorElement(SearchBtn, Definitions.HomeButtonBackColor); // Change the background
			SearchBox.Visibility = Visibility.Visible; // Show
		}
		else
		{
			ColorElement(SearchBtn, new SolidColorBrush(Colors.Transparent)); // Change the background
			SearchBox.Visibility = Visibility.Collapsed; // Hide
		}
	}

	private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
	{
		Application.Current.Shutdown();
	}

	private void RecentTabBtn_Click(object sender, RoutedEventArgs e)
	{
		CheckedTabButton = RecentTabBtn; // Set the checked button
		CheckButton(); // Update the UI

		PageContent.Navigate(Definitions.RecentGamesPage); // Show the Home page
	}

	private void SettingsBtn_Click(object sender, RoutedEventArgs e)
	{
		Windows.Settings settings = new(); // Settings window
		CheckedTabButton = SettingsBtn; // Set the checked button
		CheckButton(); // Update the UI
		settings.Show(); // Show the Settings window
	}

	private void AddBtn_Click(object sender, RoutedEventArgs e)
	{
		AddPopup.IsOpen = true;
	}

	private void AddGameBtn_Click(object sender, RoutedEventArgs e)
	{
		new AddGame(false, false).Show(); // Open the "Add Game" dialog
		AddPopup.IsOpen = false;

	}

	private void AddUWPBtn_Click(object sender, RoutedEventArgs e)
	{
		new AddGame(true, false).Show(); // Add game
		AddPopup.IsOpen = false;

	}

	private void AddSteamBtn_Click(object sender, RoutedEventArgs e)
	{
		new AddGame(false, true).Show(); // Add Steam Game
		AddPopup.IsOpen = false;

	}
}
