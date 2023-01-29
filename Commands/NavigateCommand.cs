using Reservroom.Stores;
using Reservroom.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservroom.Commands;

public class NavigateCommand : CommandBase
{
    private readonly NavigationStore _navigationStore;

    public NavigateCommand(NavigationStore navigationStore)
    {
        _navigationStore = navigationStore;
    }

    public override void Execute(object parameter)
    {
        _navigationStore.CurrentViewModel = new MakeReservationViewModel(new Models.Hotel(""));
    }
}
