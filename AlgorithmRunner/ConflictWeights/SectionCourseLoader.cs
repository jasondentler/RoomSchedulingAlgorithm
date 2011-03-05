using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace AlgorithmRunner.ConflictWeights
{
    public class SectionCourseLoader
    {

        private readonly string _fileName;

        /// <summary>
        /// Creates a dictionary of sections to courses
        /// </summary>
        /// <param name="fileName">File name of a term's COURSE.SECTION records</param>
        /// <remarks>
        /// SELECT COURSE.SECTIONS WITH SEC.TERM EQ '210FAB' '210FA' '210FAM1' '210FAM2'
        /// LIST COURSE.SECTIONS TOXML ALL TO _HOLD_/Sections.Fall2010
        /// </remarks>
        public SectionCourseLoader(string fileName)
        {
            _fileName = fileName;
        }

        /// <summary>
        /// Builds dictionaries of sections -> courses
        /// </summary>
        public IDictionary<string, string> Load()
        {
            var result = new Dictionary<string, string>();
            var doc = XDocument.Load(_fileName);
            var entries = from record in doc.Root.Elements("COURSE.SECTIONS")
                          select new
                                     {
                                         Section = record.Attribute("_ID").Value,
                                         Course = record.Attribute("SEC.COURSE").Value
                                     };
            foreach (var entry in entries)
                result[entry.Section] = entry.Course;
            return result;
        }

    }
}
