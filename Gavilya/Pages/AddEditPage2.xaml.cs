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
using Gavilya.SDK.RAWG;
using Gavilya.UserControls;
using Gavilya.Windows;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Gavilya.Pages;

/// <summary>
/// Interaction logic for AddEditPage2.xaml
/// </summary>
public partial class AddEditPage2 : Page
{
	internal AddGame AddGame { get; set; }
	internal EditGame EditGame { get; set; }

	internal List<Platform> Platforms { get; set; }
	internal List<Store> Stores { get; set; }
	internal List<GameTag> Tags { get; set; }
	internal string GameDescription { get; set; }
	internal int RAWGID { get; set; }
	internal GameCard GameCard { get; set; }

	readonly GameInfo old; // Set
	readonly bool isFromAdd;
	public AddEditPage2(AddGame addGame)
	{
		InitializeComponent();
		AddGame = addGame; // Set
		isFromAdd = true;
		Platforms = new();
		Stores = new();
		RAWGID = AddGame.GameInfo.RAWGID;
		Tags = new();
	}

	public AddEditPage2(EditGame editGame, GameCard gameCard)
	{
		InitializeComponent();
		EditGame = editGame; // Set
		isFromAdd = false;
		GameCard = EditGame.GameCard; // Set
		Tags = EditGame.GameCard.GameInfo.AssociatedTags;

		RAWGID = GameCard.GameInfo.RAWGID; // Set value
		old = gameCard.GameInfo;
		InitUI();
	}

	private void InitUI()
	{
		DescriptionTextBox.Text = GameCard.GameInfo.Description; // Set text
		if (RAWGID < 1)
		{
			Platforms = new(); // Set
			Stores = new(); // Set

			AssociateTxt.Text = Properties.Resources.NotAssociated; // Set
			AssociateIconTxt.Text = "\uE9D8"; // Set
		}
		else
		{
			Platforms = GameCard.GameInfo.Platforms; // Set
			Stores = GameCard.GameInfo.Stores; // Set

			AssociateTxt.Text = Properties.Resources.Associated; // Set
			AssociateIconTxt.Text = "\uE98E"; // Set
		}

		if (!GameCard.GameInfo.IsSteam && !GameCard.GameInfo.IsUWP) // If is EXE game
		{
			SteamBorder.Visibility = Visibility.Visible; // Show the Steam converter only if the game is a Win32 app
		}
	}

	private void NextBtn_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			if (isFromAdd)
			{
				AddGame.GameInfo.Platforms = (Platforms.Count == 0) ? new List<SDK.RAWG.Platform> { Definitions.DefaultPlatform } : Platforms; // Get platforms
				AddGame.GameInfo.Stores = Stores;
				AddGame.GameInfo.AssociatedTags = Tags;
				AddGame.GameInfo.Description = DescriptionTextBox.Text;
				Definitions.Games.Add(AddGame.GameInfo);

				GameSaver.Save(Definitions.Games); // Save
				Global.ReloadAllPages(); // Refresh UI

				AddGame.Close();
			}
			else
			{
				GameCard.GameInfo.RAWGID = RAWGID; // Set
				GameCard.GameInfo.Description = DescriptionTextBox.Text; // Set
				GameCard.GameInfo.Platforms = Platforms; // Set
				GameCard.GameInfo.Stores = Stores; // Set
				GameCard.GameInfo.AssociatedTags = Tags; // Set

				if (ConvertSteamPanel.Visibility != Visibility.Collapsed) // If the steam convert process is launched
				{
					ConvertSteamBtn_Click(this, null);
				}

				Definitions.Games[Definitions.Games.IndexOf(old)] = GameCard.GameInfo; // Update
				GameSaver.Save(Definitions.Games); // Save

				Global.ReloadAllPages(); // Refresh UI

				EditGame.Close();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Properties.Resources.MainWindowTitle, MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}

	private void CancelBtn_Click(object sender, RoutedEventArgs e)
	{
		if (isFromAdd)
		{
			AddGame.Close();
		}
		else
		{
			EditGame.Close();
		}
	}

	private void AssociateBtn_Click(object sender, RoutedEventArgs e)
	{
		new SearchGameCover(this, GameAssociationActions.Associate).Show();
	}

	private void BackBtn_Click(object sender, RoutedEventArgs e)
	{
		if (isFromAdd)
		{
			AddGame.ChangePage(0); // Go back to last page
		}
		else
		{
			EditGame.ChangePage(0); // Go back to last page
		}
	}

	private void ConvertSteamBtn_Click(object sender, RoutedEventArgs e)
	{
		if (ConvertSteamPanel.Visibility == Visibility.Collapsed)
		{
			ConvertSteamBtn.Content = Properties.Resources.ConvertToSteam; // Set new text of the button
			ConvertSteamPanel.Visibility = Visibility.Visible; // Show the panel
		}
		else
		{
			if (string.IsNullOrEmpty(SteamAppIdTextBox.Text))
			{
				MessageBox.Show(Properties.Resources.GameNeedsName, Properties.Resources.MainWindowTitle, MessageBoxButton.OK, MessageBoxImage.Exclamation); // Show message
				return;
			}

			// Ask a confirmation to the user
			if (MessageBox.Show(Properties.Resources.ConvertToSteamMsg, Properties.Resources.MainWindowTitle, MessageBoxButton.YesNoCancel, MessageBoxImage.Warning) == MessageBoxResult.Yes)
			{
				GameCard.GameInfo.IsSteam = true; // Convert to steam game
				GameCard.GameInfo.FileLocation = $"steam://rungameid/{SteamAppIdTextBox.Text}";
				GameCard.GameInfo.ProcessName = !string.IsNullOrEmpty(GameProcessTextBox.Text) ? GameProcessTextBox.Text : GameCard.GameInfo.ProcessName;

				// Save other changes
				GameCard.GameInfo.RAWGID = RAWGID; // Set
				GameCard.GameInfo.Description = DescriptionTextBox.Text; // Set
				GameCard.GameInfo.Platforms = Platforms; // Set
				GameCard.GameInfo.Stores = Stores; // Set

				Definitions.Games[Definitions.Games.IndexOf(old)] = GameCard.GameInfo; // Update
				GameSaver.Save(Definitions.Games); // Save

				Global.ReloadAllPages(); // Refresh UI

				EditGame.Close();
			}
		}
	}

	private void SteamAppIdTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
	{
		Regex regex = new("[^0-9]+");
		e.Handled = regex.IsMatch(e.Text);
	}

	private void AddTagsBtn_Click(object sender, RoutedEventArgs e)
	{
		new AssociateTags(this).Show();
	}
}
