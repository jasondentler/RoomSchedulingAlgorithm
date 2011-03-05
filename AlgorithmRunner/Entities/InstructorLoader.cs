using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace AlgorithmRunner.Entities
{
    public class InstructorLoader
    {
        private readonly string _fileName;

        public InstructorLoader(string fileName)
        {
            _fileName = fileName;
        }

        public IDictionary<string, Instructor> Load()
        {
            var doc = XDocument.Load(_fileName);
            return doc.Root.Elements("PERSON")
                .Select(p => new
                                 {
                                     id = p.Attribute("_ID").Value,
                                     instructor =
                                 new Instructor(
                                     p.Attribute("FIRST.NAME").Value,
                                     p.Attribute("LAST.NAME").Value
                                 )
                                 })
                .ToDictionary(item => item.id, item => item.instructor);
        }

    }
}
