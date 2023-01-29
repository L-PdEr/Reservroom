using Reservroom.Exceptions;
using Reservroom.Models;
using Reservroom.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Reservroom.Commands
{
    public class MakeReservationCommand : CommandBase
    {
        private readonly MakeReservationViewModel _makeReservationViewModel;
        private readonly Hotel _hotel;
        
        public MakeReservationCommand(MakeReservationViewModel makeReservationViewModel, Hotel hotel)
        {
            _makeReservationViewModel = makeReservationViewModel;
            _hotel = hotel;

            _makeReservationViewModel.PropertyChanged += OnViewModelPropertyChanged;

        }


        public override bool CanExecute(object parameter)
        {
            // Raised canexecute changed event when the username changes
            // dafür müssen wir ein event abonieren
            return 
                !string.IsNullOrEmpty(_makeReservationViewModel.Username) 
                && _makeReservationViewModel.FloorNumber > 0
                && base.CanExecute(parameter);
        }
        public override void Execute(object parameter)
        {
            Reservation reservation = new Reservation(
                new RoomID(_makeReservationViewModel.FloorNumber, _makeReservationViewModel.RoomNumber),
                _makeReservationViewModel.Username,
                _makeReservationViewModel.StartDate,
                _makeReservationViewModel.EndDate);
            _hotel.MakeReservation(reservation);

            try
            {
                _hotel.MakeReservation(reservation);
                MessageBox.Show($"Successfully reserved room.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (ReservationConflictException e)
            {
                MessageBox.Show($"This room is already taken.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            // || statt && weil hier meldet sich was sich ändern und zwei werte gleichzeitig zu ändern ist unmöglich
            if(e.PropertyName == nameof(MakeReservationViewModel.Username) ||
                e.PropertyName == nameof(MakeReservationViewModel.FloorNumber))
            {
                OnCanExecuteChanged();
            }
        }
    }
}
    