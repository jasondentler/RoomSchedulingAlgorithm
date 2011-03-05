using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace AlgorithmRunner.ConflictWeights
{
    public class CourseSectionLoader
    {

        private readonly string _fileName;

        /// <summary>
        /// Creates a dictionary of course to sets of sections
        /// </summary>
        /// <param name="fileName">File name of a term's COURSE.SECTION records</param>
        /// <remarks>
        /// SELECT COURSE.SECTIONS WITH SEC.TERM EQ '210FAB' '210FA' '210FAM1' '210FAM2'
        /// LIST COURSE.SECTIONS TOXML ALL TO _HOLD_/Sections.Fall2010
        /// </remarks>
        public CourseSectionLoader(string fileName)
        {
            _fileName = fileName;
        }

        /// <summary>
        /// Builds dictionaries of of course to sets of sections
        /// </summary>
        public IDictionary<string, ISet<string>> Load()
        {
            var result = new Dictionary<string, ISet<string>>();

            var doc = XDocument.Load(_fileName);
            var entries = from record in doc.Root.Elements("COURSE.SECTIONS")
                          select new
                                     {
                                         Section = record.Attribute("_ID").Value,
                                         Course = record.Attribute("SEC.COURSE").Value
                                     };
            foreach (var entry in entries)
            {
                ISet<string> set;
                if (!result.TryGetValue(entry.Course, out set))
                {
                    set = new HashSet<string>();
                    result[entry.Course] = set;
                }
                set.Add(entry.Section);
            }
            return result;
        }

    }
}
