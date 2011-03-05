using System.Collections.Generic;

namespace AlgorithmRunner.ConflictWeights
{
    public class StudentCourseLoader
    {

        private readonly IDictionary<string, ISet<string>> _studentSections;
        private readonly IDictionary<string, string> _sectionCourseMap;

        /// <summary>
        /// Builds a dictionary of student -> set of courses
        /// </summary>
        /// <param name="studentSections">dictionary of student -> set of sections</param>
        /// <param name="sectionCourseMap">dictionary of section -> course</param>
        /// <remarks>
        /// studentSections is the result of StudentSectionLoader
        /// sectionCourseMap is the result of SectionCourseLoader
        /// </remarks>
        public StudentCourseLoader(
            IDictionary<string, ISet<string>> studentSections,
            IDictionary<string, string> sectionCourseMap)
        {
            _studentSections = studentSections;
            _sectionCourseMap = sectionCourseMap;
        }

        /// <summary>
        /// Builds dictionary of student -> set of courses
        /// </summary>
        /// <returns>Dictionary of student -> set of courses</returns>
        public IDictionary<string, ISet<string>> Load()
        {
            var result = new Dictionary<string, ISet<string>>();
            foreach (var student in _studentSections)
            {
                var set = new HashSet<string>();

                foreach (var section in student.Value)
                {
                    string course;
                    if (_sectionCourseMap.TryGetValue(section, out course))
                        set.Add(course);
                }

                result[student.Key] = set;
            }
            return result;
        }

    }
}
