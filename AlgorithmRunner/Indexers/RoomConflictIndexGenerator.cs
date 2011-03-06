using System;
using System.Collections.Generic;
using System.Linq;
using AlgorithmRunner.Entities;

namespace AlgorithmRunner.Indexers
{
    public class RoomConflictIndexGenerator
    {
        private readonly IEnumerable<SectionSlot> _sectionSlots;

        public RoomConflictIndexGenerator(IEnumerable<SectionSlot> sectionSlots)
        {
            _sectionSlots = sectionSlots;
        }

        public IDictionary<SectionSlot, ISet<SectionSlot>> Generate()
        {
            Console.WriteLine("Building index of room+time combinations");
            var timeSlotIndexGenerator = new RoomIndexGenerator(_sectionSlots);
            //room + time index
            var timeSlotIndex = timeSlotIndexGenerator.Generate();

            Console.WriteLine("Building index of room+time conflicts");
            return _sectionSlots.AsParallel()
                .ToDictionary(ss => ss,
                              ss => (ISet<SectionSlot>)
                                    new HashSet<SectionSlot>(
                                        timeSlotIndex[ss.Slot].Except(new[] {ss})));
        }

    }
}
