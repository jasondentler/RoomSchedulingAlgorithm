using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace AlgorithmRunner.Entities
{
    public class SectionInstructorJoiner
    {
        private readonly string _fileName;
        private readonly IDictionary<string, Section> _sections;
        private readonly IDictionary<string, Instructor> _instructors;

        public SectionInstructorJoiner(
            string fileName, 
            IDictionary<string, Section> sections,
            IDictionary<string, Instructor> instructors)
        {
            _fileName = fileName;
            _sections = sections;
            _instructors = instructors;
        }

        public IEnumerable<Section> Load()
        {
            var doc = XDocument.Load(_fileName);
            var associations = doc.Root.Elements("COURSE.SEC.FACULTY")
                .Select(e => new
                                 {
                                     sectionId = e.Attribute("CSF.COURSE.SECTION").Value,
                                     instructorId = e.Attribute("CSF.FACULTY").Value
                                 });
            var missingSections = associations
                .Where(a => !_sections.ContainsKey(a.sectionId));
            var missingInstructors = associations
                .Where(a => !_instructors.ContainsKey(a.instructorId));

            if (missingSections.Any())
                Console.WriteLine("{0} missing sections when joining instructors",
                    missingSections.Count());
            if (missingInstructors.Any())
                Console.WriteLine("{0} missing instructors when joining sections",
                                  missingInstructors.Count());

            var completeAssociations = associations
                .Except(missingSections)
                .Except(missingInstructors)
                .Select(e => new
                                 {
                                     section = _sections[e.sectionId],
                                     instructor = _instructors[e.instructorId]
                                 });

            foreach (var assoc in completeAssociations)
                assoc.section.AssignInstructor(assoc.instructor);

            return completeAssociations
                .Select(a => a.section)
                .Distinct();

        }

    }
}
