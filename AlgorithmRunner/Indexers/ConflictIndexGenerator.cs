using System;
using System.Collections.Generic;
using System.Linq;
using AlgorithmRunner.Entities;

namespace AlgorithmRunner.Indexers
{
    public class ConflictIndexGenerator
    {
        private readonly IEnumerable<SectionSlot> _sectionSlots;

        public ConflictIndexGenerator(IEnumerable<SectionSlot> sectionSlots)
        {
            _sectionSlots = sectionSlots;
        }

        public IDictionary<SectionSlot, ISet<SectionSlot>> Generate()
        {
            var roomConflictIndexGenerator = new RoomConflictIndexGenerator(_sectionSlots);
            // conflicts between section slots due to same room & time pattern
            var roomConflictIndex = roomConflictIndexGenerator.Generate();

            var instructorSlotConflictGenerator = new InstructorConflictIndexGenerator(_sectionSlots);
            //conflicts between section slots due to same instructor & time pattern
            var instructorConflictIndex = instructorSlotConflictGenerator.Generate();

            Console.WriteLine("Building index of all conflicts");
            var intermediate1 = from ss in _sectionSlots.AsParallel()
                                select new
                                           {
                                               slot = ss,
                                               roomConflicts = roomConflictIndex[ss].ToArray(),
                                               instructorConflicts = ss.HasInstructor()
                                                                         ? instructorConflictIndex[ss].ToArray()
                                                                         : new SectionSlot[0]
                                           };

            return intermediate1.ToArray().AsParallel()
                .ToDictionary(item => item.slot,
                              item => (ISet<SectionSlot>)
                                      new HashSet<SectionSlot>(
                                          item.roomConflicts.Union(item.instructorConflicts)
                                          ));
        }

    }
}
