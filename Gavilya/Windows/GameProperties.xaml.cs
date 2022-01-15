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
using System.Diagnostics;
using System.Windows;

namespace Gavilya.Windows;

/// <summary>
/// Logique d'interaction pour GameProperties.xaml
/// </summary>
public partial class GameProperties : Window
{
	private GameInfo GameInfo { get; init; }

	/// <summary>
	/// The <see cref="GameProperties"/> window.
	/// </summary>
	/// <param name="gameInfo">The informations of the game.</param>
	public GameProperties(GameInfo gameInfo)
	{
		InitializeComponent();
		GameInfo = gameInfo; // Pass the argument
		LoadUI();
	}

	private void LoadUI()
	{
		GameNameTxt.Text = GameInfo.Name; // Display the name
		GameVersionTxt.Text = GameInfo.Version; // Display the version
		GameLocationTxt.Text = (GameInfo.FileLocation.Length > 18) ? GameInfo.FileLocation[..18] + "..." : GameInfo.FileLocation; // Display the location
		GameProcessName.Text = GameInfo.ProcessName; // Display the ProcessName
		PathToolTip.Content = GameInfo.FileLocation; // Set the tooltip content
		AlwaysCheckGameRunningChk.IsChecked = GameInfo.AlwaysCheckIfRunning; // Set IsChecked
	}

	private void Button_Click(object sender, RoutedEventArgs e)
	{
		WindowState = WindowState.Minimized; // Minimize the window
	}

	private void CloseBtn_Click(object sender, RoutedEventArgs e)
	{
		Close(); // Close the window
	}

	private void OKBtn_Click(object sender, RoutedEventArgs e)
	{
		SaveChanges(); // Save the changes
		Close(); // Close the window
	}

	private void SaveChanges()
	{
		if (GameInfo.ProcessName != GameProcessName.Text) // If different
		{
			Definitions.Games[Definitions.Games.IndexOf(GameInfo)].ProcessName = GameProcessName.Text; // Set the new value
		}

		Definitions.Games[Definitions.Games.IndexOf(GameInfo)].AlwaysCheckIfRunning = AlwaysCheckGameRunningChk.IsChecked.Value; // Set
		GameSaver.Save(Definitions.Games); // Save the changes
	}

	private void CancelBtn_Click(object sender, RoutedEventArgs e)
	{
		Close(); // Close the window
	}

	private void BrowseBtn_Click(object sender, RoutedEventArgs e)
	{
		Process.Start(new ProcessStartInfo("explorer.exe", System.IO.Path.GetDirectoryName(GameInfo.FileLocation)));
	}

	private void ProcessHelpBtn_Click(object sender, RoutedEventArgs e)
	{
		MessageBox.Show(Properties.Resources.ProcessNameHelp, Properties.Resources.Help, MessageBoxButton.OK, MessageBoxImage.Question); // Show a message
	}

	private void AlwaysCheckGameRunningChk_Checked(object sender, RoutedEventArgs e)
	{

	}
}
