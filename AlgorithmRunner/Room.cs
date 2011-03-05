using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgorithmRunner
{
    public struct Room
    {
        public readonly string Building;
        public readonly string RoomNumber;
        public readonly int Capacity;
        public readonly ISet<string> Equipment;

        public Room(string building, string roomNumber, int capacity)
        {
            Building = building;
            RoomNumber = roomNumber;
            Capacity = capacity;
            Equipment = new HashSet<string>();
        }

        public override string ToString()
        {
            return RoomNumber;
        }

    }
}
