using Reservroom.Services;
using Reservroom.Stores;
using Reservroom.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservroom.Commands;

public class NavigateCommand<TViewModel> : CommandBase where TViewModel : ViewModelBase // musst jetzt auch generic sein
{
    private readonly NavigationService<TViewModel> _navigationService;
    private readonly Func<ViewModelBase> _createViewModel;

    public NavigateCommand(NavigationService<TViewModel> navigationService)
    {
        _navigationService = navigationService;
    }

    public override void Execute(object parameter)
    {
        _navigationService.Navigate();
    }
}
