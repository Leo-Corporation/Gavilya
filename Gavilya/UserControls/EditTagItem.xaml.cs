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
using Gavilya.Pages.SettingsPages;
using PeyrSharp.Core.Converters;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Gavilya.UserControls;
/// <summary>
/// Interaction logic for EditTagItem.xaml
/// </summary>
public partial class EditTagItem : UserControl
{
	internal int ID { get; init; }
	GameTag GameTag { get; set; }
	GamesOptionsPage Parent { get; init; }

	public EditTagItem(GameTag gameTag, int id, GamesOptionsPage parent)
	{
		GameTag = gameTag;
		ID = id;
		Parent = parent;
		InitializeComponent();
		InitUI();
	}

	private void InitUI()
	{
		NameTextBox.Text = GameTag.Name;
		var rgb = new HEX(GameTag.Color).ToRgb().Color;
		var color = new SolidColorBrush { Color = Color.FromRgb(rgb.R, rgb.G, rgb.B) }; // Set color
		ForegroundBorder.Background = color;
	}

	private void EditBtn_Click(object sender, RoutedEventArgs e)
	{
		if (string.IsNullOrEmpty(NameTextBox.Text))
		{
			MessageBox.Show(Properties.Resources.NameNeededMsg, Properties.Resources.ManageGameTags, MessageBoxButton.OK, MessageBoxImage.Information);
			return;
		}
		GameTag.Name = NameTextBox.Text;

		Definitions.Settings.GameTags[ID] = GameTag;
		SettingsSaver.Save();

		for (int i = 0; i < Definitions.Games.Count; i++)
		{
			for (int j = 0; j < Definitions.Games[i].AssociatedTags.Count; i++)
			{
				if (Definitions.Games[i].AssociatedTags[j].Guid == Definitions.Settings.GameTags[ID].Guid)
				{
					Definitions.Games[i].AssociatedTags[j] = GameTag;
					break;
				}
			}
		}
		GameSaver.Save(Definitions.Games);

		Parent.InitUI();
	}

	private void ForegroundBorder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		System.Windows.Forms.ColorDialog colorDialog = new()
		{
			AllowFullOpen = true,
		}; // Create color picker/dialog

		if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) // If the user selected a color
		{
			var color = new SolidColorBrush { Color = Color.FromRgb(colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B) }; // Set color

			RGB rgb = new(colorDialog.Color);
			GameTag.Color = rgb.ToHex().Value;

			ForegroundBorder.Background = color;
		}
	}

	private void DeleteBtn_Click(object sender, RoutedEventArgs e)
	{
		if (MessageBox.Show(Properties.Resources.DeleteTagMsg, Properties.Resources.DeleteTag, MessageBoxButton.YesNoCancel, MessageBoxImage.Question) == MessageBoxResult.Yes)
		{
			for (int i = 0; i < Definitions.Games.Count; i++)
			{
				for (int j = 0; j < Definitions.Games[i].AssociatedTags.Count; i++)
				{
					if (Definitions.Games[i].AssociatedTags[j].Guid == Definitions.Settings.GameTags[ID].Guid)
					{
						Definitions.Games[i].AssociatedTags.RemoveAt(j);
						break;
					}
				}
			}

			Definitions.Settings.GameTags.RemoveAt(ID);
			SettingsSaver.Save();
			GameSaver.Save(Definitions.Games);
			Parent.InitUI();
		}
	}
}
