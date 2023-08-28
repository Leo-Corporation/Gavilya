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
using System.Windows.Input;

namespace Gavilya.ViewModels
{
	public class StatGameViewModel : ViewModelBase
	{
		private string _name;
		public string Name { get => _name; set { _name = value; OnPropertyChanged(nameof(Name)); } }

		private string _totalTimePlayed;
		private readonly Game _game;
		private readonly StatsViewModel _statsViewModel;

		public string TotalTimePlayed { get => _totalTimePlayed; set { _totalTimePlayed = value; OnPropertyChanged(nameof(TotalTimePlayed)); } }

		private string _index;
		public string Index { get => _index; set { _index = value; OnPropertyChanged(nameof(Index)); } }

		private string _coverFilePath;
		public string CoverFilePath { get => _coverFilePath; set { _coverFilePath = value; OnPropertyChanged(nameof(CoverFilePath)); } }

		public ICommand ClickCommand { get; }

		public StatGameViewModel(int i, Game game, StatsViewModel? statsViewModel)
		{
			_game = game;
			if (statsViewModel is not null) _statsViewModel = statsViewModel;

			Name = _game.Name;
			CoverFilePath = game.CoverFilePath;
			Index = $"#{i + 1}";
			if (_game.TotalTimePlayed != 0)
			{
				TotalTimePlayed = $"{_game.TotalTimePlayed / 3600d:0.0}{Properties.Resources.HourShort}";
			}
			else
			{
				TotalTimePlayed = Properties.Resources.Never;
			}
			ClickCommand = new RelayCommand(Click);
		}

		private void Click(object? obj)
		{
			_statsViewModel.Name = _game.Name;
			_statsViewModel.Description = _game.Description;
			_statsViewModel.TotalTimePlayed = $"{_game.TotalTimePlayed / 3600d:0.0}{Properties.Resources.HourShort}";
			_statsViewModel.CoverFilePath = _game.CoverFilePath;

			if (_game.LastTimePlayed == 0)
			{
				_statsViewModel.LastTimePlayed = Properties.Resources.Never;
				return;
			}
			var date = Time.UnixTimeToDateTime(_game.LastTimePlayed);
			string[] months = Properties.Resources.Months.Split(",");
			_statsViewModel.LastTimePlayed = $"{date.Day} {months[date.Month - 1]} {date.Year}";
		}
	}
}
