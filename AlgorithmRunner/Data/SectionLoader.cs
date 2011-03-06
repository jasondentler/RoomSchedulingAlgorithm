using System.Linq;
using System.Xml.Linq;

namespace AlgorithmRunner.Data
{
    public class SectionLoader
    {
        private readonly string _sectionsFileName;
        private readonly Database _database;

        public SectionLoader(string sectionsFileName, Database database)
        {
            _sectionsFileName = sectionsFileName;
            _database = database;
        }

        public void Load()
        {
            var doc = XDocument.Load(_sectionsFileName);
            foreach (var section in doc.Root.Elements("COURSE.SECTIONS"))
            {
                var id = section.Attribute("_ID").Value;

                _database.AddSection(
                    id,
                    section.Attribute("SEC.CAPACITY"),
                    section
                        .Elements("SEC.EQUIPMENT_MV")
                        .Elements("SEC.EQUIPMENT_MS")
                        .Attributes("SEC.EQUIPMENT")
                        .Select(a => a.Value)
                        .Distinct());
            }
        }

    }
}
