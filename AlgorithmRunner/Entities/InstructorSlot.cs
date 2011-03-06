using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgorithmRunner.Entities
{
    public class InstructorSlot
    {
        public Instructor Instructor { get; private set; }
        public TimePattern Pattern { get; private set; }

        public InstructorSlot(Instructor instructor, TimePattern pattern)
        {
            Instructor = instructor;
            Pattern = pattern;
        }
    }
}
