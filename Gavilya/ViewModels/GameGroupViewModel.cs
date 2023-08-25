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

using Gavilya.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Gavilya.ViewModels;

public class GameGroupViewModel : ViewModelBase
{
	private readonly MainViewModel _mainViewModel;
	public string Title { get; }
	public GameList Games { get; }
	public List<GameCardViewModel> GamesVm => Games.Select(g => new GameCardViewModel(g, Games, _tags, _mainViewModel)).ToList();

	private SolidColorBrush _tagColor;
	public SolidColorBrush TagColor { get => _tagColor; set { _tagColor = value; OnPropertyChanged(nameof(TagColor)); } }

	private Visibility _tagVis = Visibility.Collapsed;
	private readonly List<Tag> _tags;

	public Visibility TagVis { get => _tagVis; set { _tagVis = value; OnPropertyChanged(nameof(TagVis)); } }

	public GameGroupViewModel(string title, GameList games, List<Tag> tags, MainViewModel mainViewModel)
	{
		Title = title;
		Games = games;
		_tags = tags;
		_mainViewModel = mainViewModel;

		if (Games.TagColor is not null)
		{
			TagColor = new() { Color = (Color)ColorConverter.ConvertFromString(Games.TagColor) };
			TagVis = Visibility.Visible;
		}
	}
}
