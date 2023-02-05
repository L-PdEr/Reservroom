using Reservroom.Commands;
using Reservroom.Models;
using Reservroom.Services;
using Reservroom.Stores;
using System;
using System.Windows.Input;

namespace Reservroom.ViewModels;

public class MakeReservationViewModel : ViewModelBase
{
    private string? _username;
    public string? Username
    {
        get => _username;
        set
        {
            _username = value;
            OnPropertyChanged(nameof(Username));
        }
    }
    private int _floorNumber;
    public int FloorNumber
    {
        get => _floorNumber;
        set
        {
            _floorNumber = value;
            OnPropertyChanged(nameof(FloorNumber));
        }
    }

    private int _roomNumber;
    public int RoomNumber
    {
        get => _roomNumber;
        set
        {
            _roomNumber = value;
            OnPropertyChanged(nameof(RoomNumber));
        }
    }

    private DateTime _startDate = new DateTime(2023, 1, 1);
    public DateTime StartDate
    {
        get => _startDate;
        set
        {
            _startDate = value;
            OnPropertyChanged(nameof(StartDate));
        }
    }

    private DateTime _endDate = new DateTime(2023, 1, 8);


    public DateTime EndDate
    {
        get => _endDate;
        set
        {
            _endDate = value;
            OnPropertyChanged(nameof(EndDate));
        }
    }
    
    public ICommand? SubmitCommand { get; }
    public ICommand? CancelCommand { get; }

    public MakeReservationViewModel(HotelStore hotelStore, NavigationService reservationViewNavigationService)
    {
        SubmitCommand = new MakeReservationCommand(this, hotelStore, reservationViewNavigationService); // wir müssen mit this arbeiten, weil wir die Daten aus dem ViewModel brauchen
        CancelCommand = new NavigateCommand(reservationViewNavigationService);
    }
}
