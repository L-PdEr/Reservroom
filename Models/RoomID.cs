using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservroom.Models
{
    public class RoomID
    {
        public RoomID(int roomNumber, int floorNumber)
        {
            RoomNumber = roomNumber;
            FloorNumber = floorNumber;
        }

        public int RoomNumber { get; }
        public int FloorNumber { get; }

        public override string ToString()
        {
            return $"{RoomNumber}{FloorNumber}";
        }
        // für exception aber equals wird nicht benutzt nur wir override == operator
        public override bool Equals(object? obj)
        {
            return obj is RoomID id &&
                   RoomNumber == id.RoomNumber &&
                   FloorNumber == id.FloorNumber;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FloorNumber, RoomNumber);
        }

        public static bool operator ==(RoomID roomID1, RoomID roomID2)
        {
            if (roomID1 is null && roomID2 is null) // == oder != im override führt zu overflow
            {
                return true;
            }
            return !(roomID1 is null) && roomID1.Equals(roomID2);    
        }

        public static bool operator !=(RoomID roomID1, RoomID roomID2)
        {
            return !(roomID1 == roomID2);
        }
    }
    
}
