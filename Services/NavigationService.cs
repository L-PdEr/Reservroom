using Reservroom.Stores;
using Reservroom.ViewModels;
using System;


namespace Reservroom.Services;

public class NavigationService<TViewModel> where TViewModel : ViewModelBase // wurde generic gemacht weil die view models durch dependency injection sonst nicht erstellt werden können
{
    private readonly NavigationStore _navigationStore;
    private readonly Func<TViewModel> _createViewModel;
    
    // Func ist ein View delegate
    public NavigationService(NavigationStore navigationStore, Func<TViewModel> createViewModel)
    {
        _navigationStore = navigationStore;
        _createViewModel = createViewModel;
    }
    public void Navigate()
    {
        _navigationStore.CurrentViewModel = _createViewModel();
    }
}
