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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace Gavilya.ViewModels
{
	public class TagSelectorViewModel : ViewModelBase
	{
		public List<Tag> SelectedTags { get; set; }
		public Tag Tag { get; }

		public ICommand SelectCommand { get; }

		private bool _selected;
		public bool Selected { get => _selected; set { _selected = value; OnPropertyChanged(nameof(Selected)); } }

		private string _name;
		public string Name { get => _name; set { _name = value; OnPropertyChanged(nameof(Name)); } }

		public TagSelectorViewModel(List<Tag> selectedTags, Tag tag, bool selected)
		{
			Tag = tag;
			Selected = selected;
			SelectedTags = selectedTags;

			Name = tag.Name;


			SelectCommand = new RelayCommand(Select);
		}

		private void Select(object? obj)
		{
			if (SelectedTags.Contains(Tag))
			{
				SelectedTags.Remove(Tag);
				return;
			}
			SelectedTags.Add(Tag);
		}
	}
}
