using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using AlgorithmRunner.Entities;

namespace AlgorithmRunner.Indexers
{
    public class InstructorConflictIndexGenerator
    {
        private readonly IEnumerable<SectionSlot> _sectionSlots;

        public InstructorConflictIndexGenerator(IEnumerable<SectionSlot> sectionSlots)
        {
            _sectionSlots = sectionSlots;
        }

        public IDictionary<SectionSlot, ISet<SectionSlot>> Generate()
        {
            Console.WriteLine("Building index of instructor+time combinations");
            var instructorSlotIndexGenerator = new InstructorSlotIndexGenerator(_sectionSlots);
            //room + time index
            var instructorSlotIndex = instructorSlotIndexGenerator.Generate();

            Console.WriteLine("Building index of instructor+time conflicts");
            return _sectionSlots.AsParallel()
                .Where(ss => ss.HasInstructor())
                .ToDictionary(ss => ss,
                              ss => (ISet<SectionSlot>)
                                    new HashSet<SectionSlot>(
                                        instructorSlotIndex[ss.InstructorSlot].Except(new[] {ss})));
        }

    }
}
