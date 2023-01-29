using Reservroom.ViewModels;
using System;


namespace Reservroom.Stores;

public class NavigationStore
{
    public ViewModelBase _currentViewModel;

    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            _currentViewModel = value;
            OnCurrentViewModelChanged();
        }
    }
    
    public event Action CurrentViewModelChanged;
    private void OnCurrentViewModelChanged()
    {
        CurrentViewModelChanged?.Invoke();
    }
}
