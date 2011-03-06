using AlgorithmRunner.Entities;

namespace AlgorithmRunner.Filters
{
    public class InstructorRoomFilters : FilterContainer<Instructor, Room>
    {

        public static readonly InstructorRoomFilters Instance;

        static InstructorRoomFilters()
        {
            Instance = new InstructorRoomFilters();
        }
        
        private InstructorRoomFilters()
        {
        }

    }
}
