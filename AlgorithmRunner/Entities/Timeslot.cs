namespace AlgorithmRunner.Entities
{
    public class Timeslot
    {

        public readonly Room Room;
        public readonly TimePattern Pattern;

        public Timeslot(Room room, TimePattern pattern)
        {
            Room = room;
            Pattern = pattern;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", Room, Pattern);
        }

    }
}
