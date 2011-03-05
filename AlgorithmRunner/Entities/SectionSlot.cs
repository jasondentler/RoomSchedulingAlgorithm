using System.Linq;

namespace AlgorithmRunner.Entities
{

    public class SectionSlot
    {

        public Section Section { get; private set; }
        public Timeslot Slot { get; private set; }

        public SectionSlot(Section section, Timeslot slot)
        {
            Section = section;
            Slot = slot;
        }

        public static bool IsValid(Section section, Timeslot slot)
        {
            if (slot.Room.Capacity < section.Capacity)
                return false;

            if (section.Equipment.Except(slot.Room.Equipment).Any())
                return false;

            return true;
        }

    }

}
