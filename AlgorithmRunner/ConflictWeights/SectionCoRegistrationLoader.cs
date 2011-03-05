using System.Collections.Generic;
using System.Linq;

namespace AlgorithmRunner.ConflictWeights
{

    public class SectionCoRegistrationLoader
    {
        private readonly IDictionary<string, int> _courseCoRegistrations;
        private readonly IDictionary<string, ISet<string>> _courseToSectionsMap;

        public SectionCoRegistrationLoader(
            IDictionary<string, int> courseCoRegistrations,
            IDictionary<string, ISet<string>> courseToSectionsMap)
        {
            _courseCoRegistrations = courseCoRegistrations;
            _courseToSectionsMap = courseToSectionsMap;
        }

        public IDictionary<string, int> Load()
        {
            var result = new Dictionary<string, int>();

            foreach (var coursePair in _courseCoRegistrations)
            {
                var courses = coursePair.Key.Split('*');
                var course1 = courses[0];
                var course2 = courses[1];
                ISet<string> sections1;
                ISet<string> sections2;

                if (_courseToSectionsMap.TryGetValue(course1, out sections1) &&
                    _courseToSectionsMap.TryGetValue(course2, out sections2))
                {
                    var crossJoin = from section1 in sections1
                                    from section2 in sections2
                                    where string.Compare(section1, section2) < 0
                                    select new {a = section1, b = section2};
                    foreach (var sectionPair in crossJoin)
                    {
                        var compositeId = string.Format("{0}*{1}", sectionPair.a, sectionPair.b);
                        result[compositeId] = coursePair.Value;
                    }
                }
            }

            return result;
        }

    }

}
