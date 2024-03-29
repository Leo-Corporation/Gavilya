﻿/*
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
using System.Collections.Generic;
using System.Windows;

namespace Gavilya.Windows;
/// <summary>
/// Interaction logic for AssociateTags.xaml
/// </summary>
public partial class AssociateTags : Window
{
	internal List<GameTag> NewTags { get; set; }

	AddEditPage2 AddEditPage2 { get; init; }
	public AssociateTags(AddEditPage2 addEditPage2)
	{
		InitializeComponent();
		NewTags = addEditPage2.Tags;
		AddEditPage2 = addEditPage2;

		InitUI();
	}

	private void InitUI()
	{
		for (int i = 0; i < Global.Settings.GameTags.Count; i++)
		{
			bool check = false;
			for (int j = 0; j < NewTags.Count; j++)
			{
				check = NewTags[j].Guid == Global.Settings.GameTags[i].Guid;
				if (check) break;
			}
			TagsDisplayer.Children.Add(new TagSelectItem(Global.Settings.GameTags[i], check));
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
		NewTags = tags;
		AddEditPage2.Tags = NewTags;
		Close();
	}
}
