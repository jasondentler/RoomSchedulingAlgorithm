using System.Collections.Generic;

namespace AlgorithmRunner.Entities
{
    public class Section
    {
        public string Name { get; private set; }
        public int Capacity { get; private set; }
        public Instructor Instructor { get; private set; }
        public ISet<string> Equipment { get; private set; }
        
        public Section(string name, int capacity, IEnumerable<string> equipment)
        {
            Name = name;
            Capacity = capacity;
            Equipment = new HashSet<string>(equipment);
        }

        public void AssignInstructor(Instructor instructor)
        {
            Instructor = instructor;
        }

    }
}
