using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AlgorithmRunner.Data
{

    public class RoomLoader
    {

        private readonly string _roomsFileName;
        private readonly Database _database;

        public RoomLoader(string roomsFileName, Database database)
        {
            _roomsFileName = roomsFileName;
            _database = database;
        }

        public void Load()
        {
            var doc = XDocument.Load(_roomsFileName);
            var roomElements = doc.Root.Elements("ROOMS")
                .Where(e => e.Attributes("ROOM.CATEGORY.CODE")
                                .Any(a => new[] { "CLAS", "THEA", "LAB" }.Contains(a.Value)));

            foreach (var room in roomElements)
            {
                var roomId = room.Attribute("_ID").Value;
                _database.AddRoom(roomId,
                                  room.Attribute("ROOM.CAPACITY"),
                                  room
                                      .Elements("ROOM.EQUIP_MV")
                                      .Elements("ROOM.EQUIP_MS")
                                      .Attributes("ROOM.EQUIPMENT.AVAIL")
                                      .Select(a => a.Value)
                                      .Distinct());
            }

        }

    }

}
