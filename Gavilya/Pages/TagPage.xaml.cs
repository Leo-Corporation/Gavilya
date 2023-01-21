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
using Gavilya.UserControls;
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

namespace Gavilya.Pages;
/// <summary>
/// Interaction logic for TagPage.xaml
/// </summary>
public partial class TagPage : Page
{
	public TagPage()
	{
		InitializeComponent();
		InitUI();
	}

	internal void InitUI()
	{
		SectionDisplayer.Children.Clear();
		for (int i = 0; i < Definitions.Settings.GameTags.Count; i++)
		{
			SectionDisplayer.Children.Add(new TagCategory(Definitions.Settings.GameTags[i]));
		}
		SectionDisplayer.Children.Add(new TagCategory(new(Properties.Resources.Other, "#eeeeee")));

		for (int i = 0; i < Definitions.Games.Count; i++)
		{
			Add(Definitions.Games[i]);
		}
	}

	internal void Add(GameInfo gameInfo)
	{
		if (gameInfo.AssociatedTags.Count == 0)
		{
			((TagCategory)SectionDisplayer.Children[^1]).GamePresenter.Children.Add(new GameCard(gameInfo, Enums.GavilyaPages.Tags, true));
			return;
		}


		for (int i = 0; i < gameInfo.AssociatedTags.Count; i++)
		{
			try
			{
				((TagCategory)SectionDisplayer.Children[Definitions.GuidIndex[gameInfo.AssociatedTags[i].Guid]]).GamePresenter.Children.Add(new GameCard(gameInfo, Enums.GavilyaPages.Tags, true));
			}
			catch { }
		}
	}
}