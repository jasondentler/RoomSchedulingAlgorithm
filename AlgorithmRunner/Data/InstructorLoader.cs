using System.Linq;
using System.Xml.Linq;

namespace AlgorithmRunner.Data
{

    public class InstructorLoader
    {
        private readonly string _sectionFacultyFileName;
        private readonly Database _database;

        public InstructorLoader(string sectionFacultyFileName, Database database)
        {
            _sectionFacultyFileName = sectionFacultyFileName;
            _database = database;
        }

        public void Load()
        {
            var doc = XDocument.Load(_sectionFacultyFileName);
            var associations = doc.Root.Elements("COURSE.SEC.FACULTY")
                .Select(e => new
                                 {
                                     section = e.Attribute("CSF.COURSE.SECTION").Value,
                                     instructor = e.Attribute("CSF.FACULTY").Value
                                 });
            foreach (var item in associations)
                _database.AssignInstructorToSection(item.section, item.instructor);
        }
    }
}
