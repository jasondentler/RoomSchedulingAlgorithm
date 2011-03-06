//using System.Linq;
//using AlgorithmRunner.Entities;

//namespace AlgorithmRunner.Filters.Rules
//{
//    /// <summary>
//    /// Ensures the assigned room has all the necessary equipment
//    /// </summary>
//    public class RoomEquipmentRule : IFilter<Section, Room>
//    {
//        public bool IsValid(Section section, Room room)
//        {
//            return !section.Equipment.Except(room.Equipment).Any();
//        }
//    }
//}
