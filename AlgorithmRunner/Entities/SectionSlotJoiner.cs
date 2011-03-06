using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AlgorithmRunner.Entities
{
    public class SectionSlotJoiner
    {
        private readonly IEnumerable<Section> _sections;
        private readonly IEnumerable<Timeslot> _slots;

        public SectionSlotJoiner(IEnumerable<Section> sections, IEnumerable<Timeslot> slots)
        {
            _sections = sections;
            _slots = slots;
        }

        public IEnumerable<SectionSlot> Generate()
        {
            return from section in _sections.AsParallel()
                    from slot in _slots.AsParallel()
                    where SectionSlot.IsValid(section, slot)
                    select new SectionSlot(section, slot);
        }
    }
}
