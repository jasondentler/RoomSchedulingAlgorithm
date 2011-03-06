using System.Collections.Generic;
using System.Linq;
using AlgorithmRunner.Entities;

namespace AlgorithmRunner.Indexers
{
    public class RoomIndexGenerator 
    {
        private readonly IEnumerable<SectionSlot> _sectionSlots;

        public RoomIndexGenerator(IEnumerable<SectionSlot> sectionSlots)
        {
            _sectionSlots = sectionSlots;
        }

        public IDictionary<Timeslot, ISet<SectionSlot>> Generate()
        {
            return _sectionSlots.AsParallel()
                .GroupBy(ss => ss.Slot)
                .ToDictionary(item => item.Key,
                              item => (ISet<SectionSlot>) new HashSet<SectionSlot>(item));
        }

    }
}
