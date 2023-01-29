using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservroom.Models
{
    public class Reservation
    {
        public Reservation(RoomID roomID, string userName, DateTime startTime, DateTime endTime)
        {
            RoomID = roomID;
            Username = userName;
            StartTime = startTime;
            EndTime = endTime;
        }

        public RoomID RoomID { get; }
        public string Username { get; }
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }
        public TimeSpan Length => EndTime.Subtract(StartTime);

        public bool Conflics(Reservation reservation)
        {
            if (reservation.RoomID != RoomID)
            {
                return false;
            }
            return reservation.StartTime < EndTime && StartTime < reservation.EndTime;
        }
    }
}
