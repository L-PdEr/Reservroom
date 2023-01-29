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
    private readonly Hotel _hotel;

    // Es kann nicht direkt zu Reservation gebunden werden da es kein INotifyPropertyChanged implementiert und es zu memory leaks kommen kann
    // Deshalb wird mit ReservationViewModel gebunden | Reservation -> ReservationViewModel -> ReservationListingViewModel
    private readonly ObservableCollection<ReservationViewModel> _reservations;
    // IEnumarable für capsolation 
    public IEnumerable<ReservationViewModel> Reservations => _reservations;
    public ICommand? MakeReservationCommand { get;}
    
    public ReservationListingViewModel(Hotel hotel, NavigationService makeReservationNavigationService)
    {
        _hotel = hotel;
        _reservations = new ObservableCollection<ReservationViewModel>();

        MakeReservationCommand = new NavigateCommand(makeReservationNavigationService);

        UpdateReservations();
    }

    private void UpdateReservations()
    {
        _reservations.Clear();
        foreach (var reservation in _hotel.GetAllReservations())
        {
            ReservationViewModel reservationViewModel = new ReservationViewModel(reservation);
            _reservations.Add(reservationViewModel);
        }
    }
}
