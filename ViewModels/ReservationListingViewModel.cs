using Reservroom.Models;
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
    // IEnumarable für capsolation 
    public IEnumerable<ReservationViewModel> Reservations => _reservations;
    public ICommand? MakeReservationCommand { get;}
    
    public ReservationListingViewModel()
    {
        _reservations = new ObservableCollection<ReservationViewModel>();

        MakeReservationCommand = new NavigationCommand();

        // Hardcode reservations
        _reservations.Add(new ReservationViewModel(new Reservation(new RoomID(1,2), "Max", DateTime.Now, DateTime.Now.AddHours(1))));
        _reservations.Add(new ReservationViewModel(new Reservation(new RoomID(3, 2), "Max", DateTime.Now.AddHours(2), DateTime.Now.AddHours(3))));
        _reservations.Add(new ReservationViewModel(new Reservation(new RoomID(2, 4), "Max", DateTime.Now.AddHours(4), DateTime.Now.AddHours(5))));
        _reservations.Add(new ReservationViewModel(new Reservation(new RoomID(4, 2), "Max", DateTime.Now.AddHours(6), DateTime.Now.AddHours(7))));
    }
}
