using Gavilya.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Gavilya.ViewModels;
public class NavBarViewModel : ViewModelBase
{
    public ICommand HomePageCommand { get; }
    public NavBarViewModel()
    {
        HomePageCommand = new RelayCommand(HomePage);
    }

    private void HomePage(object? obj)
    {
        
    }
}
