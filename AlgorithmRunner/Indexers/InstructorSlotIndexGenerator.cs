using System.Collections.Generic;
using System.Linq;
using AlgorithmRunner.Entities;

namespace AlgorithmRunner.Indexers
{
    public class InstructorSlotIndexGenerator 
    {
        private readonly IEnumerable<SectionSlot> _sectionSlots;

        public InstructorSlotIndexGenerator(IEnumerable<SectionSlot> sectionSlots)
        {
            _sectionSlots = sectionSlots;
        }

        public IDictionary<InstructorSlot, ISet<SectionSlot>> Generate()
        {
            return _sectionSlots.AsParallel()
                .Where(ss => ss.HasInstructor())
                .GroupBy(ss => ss.InstructorSlot)
                .ToDictionary(item => item.Key,
                              item => (ISet<SectionSlot>) new HashSet<SectionSlot>(item));
        }

    }
}
