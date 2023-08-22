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
using Gavilya.UserControls;
using Gavilya.Windows;
using System.Windows;
using System.Windows.Controls;

namespace Gavilya.Pages;
/// <summary>
/// Interaction logic for AddEditPage3.xaml
/// </summary>
public partial class AddEditPage3 : Page
{
	AddGame AddGame { get; init; }
	EditGame EditGame { get; init; }
	internal GameCard GameCard { get; set; }
	bool isFromAdd = false;

	readonly GameInfo old; // Set
	public AddEditPage3(AddGame addGame)
	{
		InitializeComponent();
		AddGame = addGame;
		isFromAdd = true;
	}

	public AddEditPage3(EditGame editGame, GameCard gameCard)
	{
		InitializeComponent();

		EditGame = editGame; // Set
		GameCard = EditGame.GameCard; // Set
		old = gameCard.GameInfo;
		InitUI();
	}

	private void InitUI()
	{
		ProcessTextBox.Text = GameCard.GameInfo.ProcessName;
		AlwaysCheckGameRunningChk.IsChecked = GameCard.GameInfo.AlwaysCheckIfRunning;
	}

	private void BackBtn_Click(object sender, RoutedEventArgs e)
	{
		if (isFromAdd)
		{
			AddGame.ChangePage(1); // Go back to last page
		}
		else
		{
			EditGame.ChangePage(1); // Go back to last page
		}
	}

	internal void NextBtn_Click(object sender, RoutedEventArgs e)
	{
		if (isFromAdd)
		{
			AddGame.GameInfo.AlwaysCheckIfRunning = AlwaysCheckGameRunningChk.IsChecked ?? false;
			AddGame.GameInfo.ProcessName = ProcessTextBox.Text;
			Global.Games.Add(AddGame.GameInfo);

			GameSaver.Save(Global.Games); // Save
			Global.ReloadAllPages(); // Refresh UI

			AddGame.Close();
		}
		else
		{
			GameCard.GameInfo.AlwaysCheckIfRunning = AlwaysCheckGameRunningChk.IsChecked ?? false;
			GameCard.GameInfo.ProcessName = ProcessTextBox.Text;

			Global.Games[Global.Games.IndexOf(old)] = GameCard.GameInfo; // Update
			GameSaver.Save(Global.Games); // Save

			Global.ReloadAllPages(); // Refresh UI

			EditGame.Close();
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

	private void ProcessHelpBtn_Click(object sender, RoutedEventArgs e)
	{
		MessageBox.Show(Properties.Resources.ProcessNameHelp, Properties.Resources.Help, MessageBoxButton.OK, MessageBoxImage.Question); // Show a message
	}
}
