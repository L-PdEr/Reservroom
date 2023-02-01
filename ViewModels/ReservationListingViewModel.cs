﻿using Reservroom.Commands;
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

    public ICommand LoadReservationsCommand { get; }
    public ICommand MakeReservationCommand { get; }
    
    public ReservationListingViewModel(Hotel hotel, NavigationService makeReservationNavigationService)
    {
        _reservations = new ObservableCollection<ReservationViewModel>();

        LoadReservationsCommand = new LoadReservationsCommand(this, hotel);
        MakeReservationCommand = new NavigateCommand(makeReservationNavigationService);
    }
    
    public static ReservationListingViewModel LoadViewModel(Hotel hotel, NavigationService makeReservationNavigationService)
    {
        ReservationListingViewModel viewModel = new ReservationListingViewModel(hotel, makeReservationNavigationService);
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
