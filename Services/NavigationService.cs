using Reservroom.Stores;
using Reservroom.ViewModels;
using System;


namespace Reservroom.Services;

public class NavigationService
{
    private readonly NavigationStore _navigationStore;
    private readonly Func<ViewModelBase> _createViewModel;
    
    // Func ist ein View delegate
    public NavigationService(NavigationStore navigationStore, Func<ViewModelBase> createViewModel)
    {
        _navigationStore = navigationStore;
        _createViewModel = createViewModel;
    }
    public void Navigate()
    {
        _navigationStore.CurrentViewModel = _createViewModel();
    }
}
