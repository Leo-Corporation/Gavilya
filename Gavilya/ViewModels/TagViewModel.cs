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

using Gavilya.Commands;
using Gavilya.Models;
using PeyrSharp.Core.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Gavilya.ViewModels
{
	public class TagViewModel : ViewModelBase
	{
		private string _name;
		public string Name { get => _name; set { _name = value; OnPropertyChanged(nameof(Name)); } }

		private string _color;
		public string Color { get => _color; set { _color = value; SolidColorBrush = GetBrushFromHex(value); OnPropertyChanged(nameof(Color)); } }

		private SolidColorBrush _solidColorBrush;
		public SolidColorBrush SolidColorBrush { get => _solidColorBrush; set { _solidColorBrush = value; OnPropertyChanged(nameof(SolidColorBrush)); } }


		private List<Tag> _tags;
		private readonly GameList _games;
		private readonly Action _refresh;
		private Tag _tag;

		public ICommand EditCommand { get; }
		public ICommand DeleteCommand { get; }
		public ICommand ChangeColorCommand { get; }
		public TagViewModel(Tag tag, List<Tag> tags, GameList games, Action refresh)
		{
			Name = tag.Name;
			Color = tag.HexColorCode;

			_tags = tags;
			_games = games;
			_refresh = refresh;
			_tag = tag;

			EditCommand = new RelayCommand(Edit);
			DeleteCommand = new RelayCommand(Delete);
			ChangeColorCommand = new RelayCommand(ChangeColor);
		}

		private void Edit(object? obj)
		{
			_tag.Name = Name;
			_tag.HexColorCode = Color;
			_tags[_tags.IndexOf(_tag)] = _tag;

			foreach (Game game in _games)
			{
				if (game.Tags?.Contains(_tag) ?? false)
				{
					game.Tags[game.Tags.IndexOf(_tag)] = _tag;
				}
			}
			_refresh();
		}

		private void Delete(object? obj)
		{
			_tags.Remove(_tag);
			foreach (Game game in _games)
			{
				if (game.Tags?.Contains(_tag) ?? false)
				{
					game.Tags.Remove(_tag);
				}
			}
			_refresh();
		}

		private void ChangeColor(object? obj)
		{
			System.Windows.Forms.ColorDialog colorDialog = new()
			{
				AllowFullOpen = true,
			}; // Create color picker/dialog
			if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) // If the user selected a color
			{
				RGB rgb = new(colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B);
				_tag.HexColorCode = rgb.ToHex().Value;
			}
		}

		private SolidColorBrush GetBrushFromHex(string hex)
		{
			var color = new HEX(hex).ToRgb().Color;
			return new SolidColorBrush { Color = System.Windows.Media.Color.FromRgb(color.R, color.G, color.B) };
		}
	}
}
