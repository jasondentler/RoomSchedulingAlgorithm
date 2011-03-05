using System.Collections.Generic;
using System.Linq;

namespace AlgorithmRunner.Entities
{

    public class TimeslotGenerator
    {

        private readonly IEnumerable<Room> _rooms;
        private readonly IEnumerable<TimePattern> _patterns;
        
        public TimeslotGenerator(IEnumerable<Room> rooms, IEnumerable<TimePattern> patterns)
        {
            _rooms = rooms;
            _patterns = patterns;
        }

        public IEnumerable<Timeslot> Generate()
        {
            return from r in _rooms
                   from p in _patterns
                   select new Timeslot(r, p);
        }

    }

}
