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
using PeyrSharp.Env;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Gavilya.Pages.SettingsPages;
/// <summary>
/// Interaction logic for FpsOptionsPage.xaml
/// </summary>
public partial class FpsOptionsPage : Page
{
	public FpsOptionsPage()
	{
		InitializeComponent();
		InitUI();
	}

	private void InitUI()
	{
		FpsShortcutTxt.Text = string.Format(Properties.Resources.OpenFpsCounter, "Ctrl+Shift+F");
		OpacitySlider.Value = (Definitions.Settings.FpsCounterOpacity ?? 1) * 100;
	}

	private void OpacitySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
	{
		SaveButton.Visibility = Visibility.Visible; // Show the Save button
	}

	private void SaveButton_Click(object sender, RoutedEventArgs e)
	{
		Definitions.Settings.FpsCounterOpacity = OpacitySlider.Value / 100d;
		SettingsSaver.Save();

		if (Definitions.IsFpsToggled)
		{
			// End the process
			var proc = new Process();
			proc.StartInfo.FileName = "cmd.exe";
			proc.StartInfo.Arguments = "/c taskkill /f /im Gavilya.Fps.exe";
			proc.StartInfo.UseShellExecute = false;
			proc.StartInfo.CreateNoWindow = true;
			proc.Start();

			// Restart the app
			var proc2 = new Process();
			proc2.StartInfo.FileName = "cmd.exe";
			proc2.StartInfo.Arguments = $"/c \"{Definitions.CurrentAppDirectory}/Gavilya.Fps.exe\" {Definitions.Settings.FpsCounterOpacity}";
			proc2.StartInfo.UseShellExecute = false;
			proc2.StartInfo.CreateNoWindow = true;
			proc2.Start();
			Definitions.IsFpsToggled = !Definitions.IsFpsToggled;
		}
	}
}
