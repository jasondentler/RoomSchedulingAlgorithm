using AlgorithmRunner.Filters;

namespace AlgorithmRunner.Entities
{

    public class SectionSlot
    {

        public Section Section { get; private set; }
        public Timeslot Slot { get; private set; }
        public InstructorSlot InstructorSlot { get; private set; }

        public SectionSlot(Section section, Timeslot slot)
        {
            Section = section;
            Slot = slot;
            if (HasInstructor())
                InstructorSlot = new InstructorSlot(section.Instructor, slot.Pattern);
        }

        public static bool IsValid(Section section, Timeslot slot)
        {
            // RoomPatterFilters already run by RoomPatternJoiner
            return SectionRoomFilters.Instance.IsValid(section, slot.Room)
                   && SectionPatternFilters.Instance.IsValid(section, slot.Pattern)
                   && (section.Instructor == null
                           ? true
                           : InstructorPatternFilters.Instance.IsValid(section.Instructor, slot.Pattern) 
                           && InstructorRoomFilters.Instance.IsValid(section.Instructor, slot.Room));
        }

        public bool HasInstructor()
        {
            return Section.Instructor != null;
        }

    }

}
