using Reservroom.Commands;
using Reservroom.Models;
using Reservroom.Services;
using Reservroom.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Reservroom.ViewModels;

public class ReservationListingViewModel : ViewModelBase
{

    
    // Es kann nicht direkt zu Reservation gebunden werden da es kein INotifyPropertyChanged implementiert und es zu memory leaks kommen kann
    // Deshalb wird mit ReservationViewModel gebunden | Reservation -> ReservationViewModel -> ReservationListingViewModel
    private readonly ObservableCollection<ReservationViewModel> _reservations;
    private HotelStore _hotelStore;

    // IEnumarable für capsolation 
    public IEnumerable<ReservationViewModel> Reservations => _reservations;

    private string _errorMessage; // kann auch eine liste von strings sein falls mehrere fehlermeldungen angezeigt werden sollen
    public string ErrorMessage
    {
        get { return _errorMessage; }
        set
        {
            _errorMessage = value;
            OnPropertyChanged(nameof(ErrorMessage));
            OnPropertyChanged(nameof(HasErrorMessage)); // wenn ui updatet kann es den wert wieder holen
        }
    }
    public bool HasErrorMessage => !string.IsNullOrEmpty(ErrorMessage); // Hat eine abhängigkeit von ErrorMessage

    private bool _isLoading;

    public bool IsLoading
    {
        get { return _isLoading; }
        set 
        { 
            _isLoading = value;
            OnPropertyChanged(nameof(IsLoading));
        }
    }

    public ICommand LoadReservationsCommand { get; }
    public ICommand MakeReservationCommand { get; }
    
    public ReservationListingViewModel(HotelStore hotelStore, NavigationService<MakeReservationViewModel> makeReservationNavigationService)
    {
        _hotelStore = hotelStore;
        _reservations = new ObservableCollection<ReservationViewModel>();

        LoadReservationsCommand = new LoadReservationsCommand(this, hotelStore);
        MakeReservationCommand = new NavigateCommand<MakeReservationViewModel>(makeReservationNavigationService);

        _hotelStore.ReservationMade += OnReservationMade; // Jedes mal wenn man etwas subscribte sollte man auf memory leaks aufpassen
                                                         // hier in diesem fall erhält der subscriber das viewmodel am leben und es kommt
                                                         // nicht zu einem garbage collection                                                  
    }

    //~ReservationListingViewModel() // destructor wird aufgerufen wenn das viewmodel nicht mehr gebraucht wird
    //{                              // zu reinen testzwecken ob der destructor aufgerufen wird kann man ihn setzen und mit breakpoint rein debuggen
    //}

    public override void Dispose()
    {
        // Unsubscribe from events
        _hotelStore.ReservationMade -= OnReservationMade;
        base.Dispose();
    }

    private void OnReservationMade(Reservation reservation)
    {
        ReservationViewModel reservationViewModel = new ReservationViewModel(reservation); // wrap reservation in ReservationViewModel
        _reservations.Add(reservationViewModel);
    }

    public static ReservationListingViewModel LoadViewModel(HotelStore hotelStore, NavigationService<MakeReservationViewModel> makeReservationNavigationService)
    {
        ReservationListingViewModel viewModel = new ReservationListingViewModel(hotelStore, makeReservationNavigationService);
        viewModel.LoadReservationsCommand.Execute(null);
        return viewModel;
    }

    public void UpdateReservations(IEnumerable<Reservation> reservations)
    {
        _reservations.Clear();
        foreach (var reservation in reservations)
        {
            ReservationViewModel reservationViewModel = new ReservationViewModel(reservation);
            _reservations.Add(reservationViewModel);
        }
    }
}
