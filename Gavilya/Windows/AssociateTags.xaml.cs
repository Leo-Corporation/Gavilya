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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Gavilya.Windows;
/// <summary>
/// Interaction logic for AssociateTags.xaml
/// </summary>
public partial class AssociateTags : Window
{
	GameInfo GameInfo { get; set; }
	AddEditPage2 AddEditPage2 { get; init; }
	public AssociateTags(AddEditPage2 addEditPage2)
	{
		InitializeComponent();
		GameInfo = addEditPage2.GameCard.GameInfo;
		AddEditPage2 = addEditPage2;

		InitUI();
	}

	private void InitUI()
	{
		for (int i = 0; i < Definitions.Settings.GameTags.Count; i++)
		{
			bool check = false;
			for (int j = 0; j < GameInfo.AssociatedTags.Count; j++)
			{
				check = GameInfo.AssociatedTags[j].Guid == Definitions.Settings.GameTags[i].Guid;
			}
			TagsDisplayer.Children.Add(new TagSelectItem(Definitions.Settings.GameTags[i], check));
		}
	}

	private void CloseBtn_Click(object sender, RoutedEventArgs e)
	{
		Close();
    }

	private void SelectBtn_Click(object sender, RoutedEventArgs e)
	{
		List<GameTag> tags = new();
		for (int i = 0; i < TagsDisplayer.Children.Count; i++)
		{
			if (TagsDisplayer.Children[i] is TagSelectItem tagSelectItem && tagSelectItem.GameCheck.IsChecked.Value)
			{
				tags.Add(tagSelectItem.GameTag);
			}
		}
		GameInfo.AssociatedTags = tags;
		AddEditPage2.GameCard.GameInfo = GameInfo;
		Close();
    }
}
