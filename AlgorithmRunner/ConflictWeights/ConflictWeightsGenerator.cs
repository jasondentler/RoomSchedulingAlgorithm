using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace AlgorithmRunner.ConflictWeights
{

    public class ConflictWeightsGenerator
    {
        private readonly string _historicalSectionsFilename;
        private readonly string _currentSectionsFilename;
        private readonly string _historicalStudentSectionsFilename;

        public ConflictWeightsGenerator(
            string historicalSectionsFilename,
            string currentSectionsFilename,
            string historicalStudentSectionsFilename)
        {
            _historicalSectionsFilename = historicalSectionsFilename;
            _currentSectionsFilename = currentSectionsFilename;
            _historicalStudentSectionsFilename = historicalStudentSectionsFilename;
        }

        public void Generate(string weightsFilename)
        {
            var sectionCoRegistrations = ProbableSectionCoRegistrations()
                .OrderByDescending(item => item.Value).ToArray();

            var doc = new XDocument(new XElement("Root"));
            var root = doc.Root;
            foreach (var item in sectionCoRegistrations)
            {
                var sections = item.Key.Split('*');
                var section1 = sections[0];
                var section2 = sections[1];
                var e1 = new XElement("ConflictWeight",
                                      new XAttribute("Section1", section1),
                                      new XAttribute("Section2", section2),
                                      item.Value);
                var e2 = new XElement("ConflictWeight",
                                      new XAttribute("Section1", section2),
                                      new XAttribute("Section2", section1),
                                      item.Value);
                root.Add(e1, e2);
            }

            doc.Save(weightsFilename);

        }

        private IDictionary<string, ISet<string>> HistoricalStudentSections()
        {
            var loader = new StudentSectionLoader(_historicalStudentSectionsFilename);
            return loader.Load();
        }

        private IDictionary<string, string> HistoricalSectionToCourseMap()
        {
            var loader = new SectionCourseLoader(_historicalSectionsFilename);
            return loader.Load();
        }

        private IDictionary<string, ISet<string>> HistoricalStudentCourses()
        {
            var studentSections = HistoricalStudentSections();
            var sectionCourseMap = HistoricalSectionToCourseMap();
            var loader = new StudentCourseLoader(studentSections, sectionCourseMap);
            return loader.Load();
        }

        private IDictionary<string, int> HistoricalCourseCoRegistrations()
        {
            var studentCourses = HistoricalStudentCourses();
            var loader = new CourseCoRegistrationLoader(studentCourses);
            return loader.Load();
        }

        private IDictionary<string, ISet<string>> CurrentCourseToSectionsMap()
        {
            var loader = new CourseSectionLoader(_currentSectionsFilename);
            return loader.Load();
        }

        private IDictionary<string, int> ProbableSectionCoRegistrations()
        {
            var courseCoRegistrations = HistoricalCourseCoRegistrations();
            var courseToSections = CurrentCourseToSectionsMap();

            var loader = new SectionCoRegistrationLoader(courseCoRegistrations, courseToSections);
            return loader.Load();
        }


    }

}
