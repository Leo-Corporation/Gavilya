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
using System.Windows;
using System.Windows.Input;

namespace Gavilya.ViewModels;

internal class FavPageViewModel : ViewModelBase
{
    private GameList _games;
    private readonly MainViewModel _mainViewModel;
    public GameList Games { get => _games; set { _games = value; OnPropertyChanged(nameof(Games)); } }

    public List<GameCardViewModel> GamesVm => Games.Select(g => new GameCardViewModel(g, Games, _tags, _mainViewModel)).ToList();

    private ViewModelBase _currentViewModel;
    private readonly List<Tag> _tags;

    public ViewModelBase CurrentViewModel { get => _currentViewModel; set { _currentViewModel = value; OnPropertyChanged(nameof(CurrentViewModel)); } }

    private bool _isCardSelected;
    public bool IsCardSelected
    {
        get => _isCardSelected; set { _isCardSelected = value; OnPropertyChanged(nameof(IsCardSelected)); }
    }

    private bool _isTagSelected;
    public bool IsTagSelected { get => _isTagSelected; set { _isTagSelected = value; OnPropertyChanged(nameof(IsTagSelected)); } }

    private bool _isListSelected;
    public bool IsListSelected { get => _isListSelected; set { _isListSelected = value; OnPropertyChanged(nameof(IsListSelected)); } }


    public ICommand CardViewCommand { get; }
    public ICommand TagViewCommand { get; }
    public ICommand ListViewCommand { get; }

    public ICommand SortAlphaCommand { get; }
    public ICommand SortNoAlphaCommand { get; }

    public FavPageViewModel(GameList games, List<Tag> tags, MainViewModel mainViewModel)
    {
        Games = games;
        _tags = tags;
        _mainViewModel = mainViewModel;
     
        CurrentViewModel = _mainViewModel.CurrentSettings.DefaultView switch
        {
            View.List => new ListPageViewModel(Games, _tags, _mainViewModel),
            View.Tag => new TagPageViewModel(Games, _tags, _mainViewModel),
            _ => new CardPageViewModel(Games, _tags, _mainViewModel)
        };

        IsCardSelected = _mainViewModel.CurrentSettings.DefaultView == View.Card;
        IsTagSelected = _mainViewModel.CurrentSettings.DefaultView == View.Tag;
        IsListSelected = _mainViewModel.CurrentSettings.DefaultView == View.List;

        CardViewCommand = new RelayCommand(CardView);
        TagViewCommand = new RelayCommand(TagView);
        ListViewCommand = new RelayCommand(ListView);
        SortAlphaCommand = new RelayCommand(SortAlpha);
        SortNoAlphaCommand = new RelayCommand(SortNoAlpha);
    }

    private void CardView(object? obj)
    {
        CurrentViewModel = new CardPageViewModel(Games, _tags, _mainViewModel);
    }

    private void TagView(object? obj)
    {
        CurrentViewModel = new TagPageViewModel(Games, _tags, _mainViewModel);
    }

    private void ListView(object? obj)
    {
        CurrentViewModel = new ListPageViewModel(Games, _tags, _mainViewModel);
    }

    private void SortAlpha(object? obj)
    {
        Games = new(Games.OrderBy(g => g.Name));

        CurrentViewModel = CurrentViewModel switch
        {
            ListPageViewModel => new ListPageViewModel(Games, _tags, _mainViewModel),
            TagPageViewModel => new TagPageViewModel(Games, _tags, _mainViewModel),
            _ => new CardPageViewModel(Games, _tags, _mainViewModel)
        };
    }

    private void SortNoAlpha(object? obj)
    {
        Games = new(Games.OrderByDescending(g => g.Name));

        CurrentViewModel = CurrentViewModel switch
        {
            ListPageViewModel => new ListPageViewModel(Games, _tags, _mainViewModel),
            TagPageViewModel => new TagPageViewModel(Games, _tags, _mainViewModel),
            _ => new CardPageViewModel(Games, _tags, _mainViewModel)
        };
    }
}
