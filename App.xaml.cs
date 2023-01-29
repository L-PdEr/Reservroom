using Reservroom.Exceptions;
using Reservroom.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Reservroom
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Hotel hotel = new Hotel("SingletonSean Suites");
            try
            {
                hotel.MakeReservation(new Reservation(
                    new RoomID(1, 3),
                    "SingletonSean",
                    new DateTime(2000, 1, 3),
                    new DateTime(2000, 1, 4)));

                hotel.MakeReservation(new Reservation(
                    new RoomID(2, 4),
                    "Marek",
                    new DateTime(2000, 1, 4),
                    new DateTime(2000, 1, 5)));
            }
            catch (ReservationConflictException ex)
            {

                throw;
            }


            IEnumerable<Reservation> reservations = hotel.GetAllReservations();
            
            base.OnStartup(e);
        }
    }
}
