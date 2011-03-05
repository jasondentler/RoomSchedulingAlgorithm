using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace AlgorithmRunner.ConflictWeights
{

    public class StudentSectionLoader
    {

        private readonly string _fileName;

        /// <summary>
        /// Creates a dictionary of students to sections
        /// </summary>
        /// <param name="fileName">File name of a term's STUDENT.COURSE.SEC records</param>
        /// <remarks>
        /// SELECT COURSE.SECTIONS WITH SEC.TERM EQ '210FAB' '210FA' '210FAM1' '210FAM2'
        /// SELECT COURSE.SECTIONS SAVING UNIQUE SEC.STUDENTS
        /// LIST STUDENT.COURSE.SEC TOXML ALL TO _HOLD_/StudentSections.Fall2010
        /// </remarks>
        public StudentSectionLoader(string fileName)
        {
            _fileName = fileName;
        }

        /// <summary>
        /// Builds dictionary of student -> set of sections
        /// </summary>
        /// <returns>Dictionary of student -> set of sections</returns>
        public IDictionary<string, ISet<string>> Load()
        {
            var result = new Dictionary<string, ISet<string>>();
            var doc = XDocument.Load(_fileName);
            var entries = from record in doc.Root.Elements("STUDENT.COURSE.SEC")
                          select new
                                     {
                                         Student = record.Attribute("SCS.STUDENT").Value,
                                         Section = record.Attribute("SCS.COURSE.SECTION").Value
                                     };
            foreach (var entry in entries)
            {
                ISet<string> set;
                if (!result.TryGetValue(entry.Student, out set))
                {
                    set = new HashSet<string>();
                    result[entry.Student] = set;
                }
                set.Add(entry.Section);
            }
            return result;
        }
    }

}
