using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace AlgorithmRunner.Entities
{

    public class SectionLoader
    {
        private readonly string _fileName;

        public SectionLoader(string fileName)
        {
            _fileName = fileName;
        }

        public IDictionary<string, Section> Load()
        {

            var doc = XDocument.Load(_fileName);

            return doc.Root.Elements("COURSE.SECTIONS")
                .Select(element => new
                                       {
                                           id = element.Attribute("_ID").Value,
                                           section = new Section(
                                       string.Format("{0} {1}",
                                                     element.Attribute("SEC.NAME").Value,
                                                     element.Attribute("SEC.SHORT.TITLE").Value
                                           ),
                                       Convert.ToInt32(element.Attribute("SEC.CAPACITY").Value),
                                       element
                                           .Elements("SEC.EQUIPMENT_MV")
                                           .Elements("SEC.EQUIPMENT_MS")
                                           .Attributes("SEC.EQUIPMENT")
                                           .Select(a => a.Value)
                                           .Distinct())
                                       })
                .Where(s => IsCoreAcademic(s.section))
                .ToDictionary(item => item.id, item => item.section);
        }

        private static bool IsCoreAcademic(Section section)
        {
            return new[] {"ENGL"}.Any(i => section.Name.StartsWith(i));
        }

    }
}