using System.Collections.Generic;
using System.Linq;

namespace AlgorithmRunner.ConflictWeights
{
    public class CourseCoRegistrationLoader
    {
        private readonly IDictionary<string, ISet<string>> _studentCourses;

        /// <summary>
        /// Builds a list of Course1*Course2 to # of co-registrations
        /// </summary>
        /// <param name="studentCourses"></param>
        public CourseCoRegistrationLoader(IDictionary<string, ISet<string>> studentCourses)
        {
            _studentCourses = studentCourses;
        }

        public IDictionary<string, int> Load()
        {
            var result = new Dictionary<string, int>();
            foreach (var student in _studentCourses)
            {
                var courses = student.Value.OrderBy(c => c).ToArray();
                for (var index1 = 0; index1 < courses.Length; index1++)
                {
                    var course1 = courses[index1];
                    for (var index2 = index1; index2 < courses.Length; index2++)
                    {
                        var course2 = courses[index2];
                        // If student signed up for course twice, ignore it.
                        if (string.Compare(course1, course2) != 0)
                        {
                            var compositeId = string.Format("{0}*{1}", course1, course2);
                            int count;
                            if (result.TryGetValue(compositeId, out count))
                            {
                                result[compositeId] = count + 1;
                            }
                            else
                            {
                                result[compositeId] = 1;
                            }
                        }
                    }
                }
            }
            return result;
        }

    }
}
