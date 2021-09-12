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
using Gavilya.SDK.RAWG;
using Gavilya.UserControls;
using Gavilya.Windows;
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

namespace Gavilya.Pages
{
	/// <summary>
	/// Interaction logic for AddEditPage2.xaml
	/// </summary>
	public partial class AddEditPage2 : Page
	{
		internal AddGame AddGame { get; set; }
		internal EditGame EditGame { get; set; }

		internal List<Platform> Platforms { get; set; }
		internal List<Store> Stores { get; set; }
		internal string GameDescription { get; set; }
		internal int RAWGID { get; set; }
		internal GameCard GameCard { get; set; }

		GameInfo old; // Set
		bool isFromAdd;
		public AddEditPage2(AddGame addGame)
		{
			InitializeComponent();
			AddGame = addGame; // Set
			isFromAdd = true;
			Platforms = new();
			Stores = new();
			RAWGID = AddGame.RAWGID;
		}

		public AddEditPage2(EditGame editGame, GameCard gameCard)
		{
			InitializeComponent();
			EditGame = editGame; // Set
			isFromAdd = false;
			GameCard = EditGame.GameCard; // Set

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
		}

		private void NextBtn_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (isFromAdd)
				{
					Definitions.Games.Add(new()
					{
						Name = AddGame.GameName, // Set value
						Version = AddGame.GameVersion, // Set value
						Description = DescriptionTextBox.Text, // Set value
						FileLocation = AddGame.GameLocation, // Set value
						IconFileLocation = AddGame.GameIconLocation, // Set value
						IsFavorite = false, // Set value
						RAWGID = RAWGID, // Set value
						LastTimePlayed = 0, // Set value
						TotalTimePlayed = 0, // Set value
						ProcessName = "", // Set value
						Platforms = (Platforms.Count == 0) ? new List<SDK.RAWG.Platform> { Definitions.DefaultPlatform } : Platforms, // Get platforms
						Stores = Stores,
						AlwaysCheckIfRunning = false,
						IsUWP = AddGame.IsUWP,
						IsSteam = AddGame.IsSteam
					});

					new GameSaver().Save(Definitions.Games); // Save
					Global.ReloadAllPages(); // Refresh UI

					AddGame.Close();
				}
				else
				{
					GameCard.GameInfo.RAWGID = RAWGID; // Set
					GameCard.GameInfo.Description = DescriptionTextBox.Text; // Set
					GameCard.GameInfo.Platforms = Platforms; // Set
					GameCard.GameInfo.Stores = Stores; // Set

					Definitions.Games[Definitions.Games.IndexOf(old)] = GameCard.GameInfo; // Update
					new GameSaver().Save(Definitions.Games); // Save

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
	}
}
