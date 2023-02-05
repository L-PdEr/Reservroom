using Reservroom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservroom.Stores
{
    public class HotelStore
    {
        private readonly Hotel _hotel;
        private Lazy<Task> _initializeLazy; // Threadsafe alternative zu einer flag
        private readonly List<Reservation> _reservations;

        public IEnumerable<Reservation> Reservations => _reservations;

        // Reactivität nur mit events möglich
        public event Action<Reservation> ReservationMade; // kann auch mit events update und delete gemacht werden
                                                          // selbes verfahren wie bei ReservationMade

        public HotelStore(Hotel hotel)
        {
            _hotel = hotel;
            
            _initializeLazy = new Lazy<Task>(Initialize); // factory function die wir einmal intializiert haben wollen,
                                                          // wenn es mehr parameter nehmen würde dann lambda. Achtung mit () wird es sofort ausgeführt
            _reservations = new List<Reservation>();
        }

        public async Task Load()
        {
            try
            {
                await _initializeLazy.Value;
            }
            catch (Exception)
            {
                _initializeLazy = new Lazy<Task>(Initialize);
                throw;
            }

        }

        public async Task MakeReservation(Reservation reservation)
        {
            await _hotel.MakeReservation(reservation);

            _reservations.Add(reservation); // in memory

            onReservationMade(reservation); // onReservationMade() -> Generate parameter (Invoke dort was sie machen soll -> stell im viewModel bereit)
        }

        private void onReservationMade(Reservation reservation)
        {
            ReservationMade?.Invoke(reservation);
        }

        private async Task Initialize()
        {
            IEnumerable<Reservation> reservations = await _hotel.GetAllReservations();
            _reservations.Clear();
            _reservations.AddRange(reservations);
        }
    }
}
