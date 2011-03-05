using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace AlgorithmRunner
{
    public class RoomLoader
    {
        private readonly string _fileName;

        public RoomLoader(string fileName)
        {
            _fileName = fileName;
        }

        public IEnumerable<Room> Load()
        {
            var doc = XDocument.Load(_fileName);
            var roomElements = doc.Root.Elements("ROOMS")
                .Where(e => e.Attributes("ROOM.CATEGORY.CODE")
                                .Any(a => new[] {"CLAS", "THEA", "LAB"}.Contains(a.Value)));

            foreach (var roomElement in roomElements)
            {
                var idParts = roomElement.Attribute("_ID").Value.Split('*');
                var building = idParts[0];
                var roomNumber = idParts[1];
                int capacity = 0;
                var capacityA = roomElement.Attribute("ROOM.CAPACITY");
                if (capacityA != null && !string.IsNullOrWhiteSpace(capacityA.Value))
                    capacity = Convert.ToInt32(capacityA.Value);
                var room = new Room(building, roomNumber, capacity);

                var equipment = roomElement
                    .Elements("ROOM.EQUIP_MV")
                    .Elements("ROOM.EQUIP_MS")
                    .Attributes("ROOM.EQUIPMENT.AVAIL")
                    .Select(a => a.Value);

                foreach (var equip in equipment)
                    room.Equipment.Add(equip);

                if (room.Building.Length == 1)
                    yield return room;
            }
        }

    }
}
