using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace AlgorithmRunner
{

    public class TimePatternLoader
    {

        private readonly string _fileName;

        public TimePatternLoader(string fileName)
        {
            _fileName = fileName;
        }

        public StringWriter Load()
        {
            var sw = new StringWriter();

            var doc = XDocument.Load(_fileName);
            var meetings = doc.Root.Elements("COURSE.SEC.MEETING")
                .Where(m => m != null &&
                            m.Attribute("CSM.START.TIME") != null &&
                            m.Attribute("CSM.END.TIME") != null)
                .Select(m =>
                        new
                            {
                                start = m.Attribute("CSM.START.TIME").Value,
                                end = m.Attribute("CSM.END.TIME").Value,
                                days = DaysOfWeekBitMaskExtensions.Convert(m.Elements("CSM.DAYS.ASSOC_MV")
                                                                               .Elements("CSM.DAYS.ASSOC_MS")
                                                                               .Attributes("CSM.DAYS")
                                                                               .Select(a => a.Value))
                            })
                .Select(m => new TimePattern(
                                 m.days,
                                 Convert.ToDateTime(m.start),
                                 Convert.ToDateTime(m.end)))
                .GroupBy(m => m)
                .Select(group => new {Pattern = group.First(), Frequency = group.Count()})
                .ToArray();

            foreach (DaysOfWeekBitMask day in Enum.GetValues(typeof(DaysOfWeekBitMask)))
            {
                var day1 = day;
                var startTimes = meetings
                    .Where(m => m.Pattern.Days.Contains(day1))
                    .Select(p => new {start = p.Pattern.Start, frequency = p.Frequency})
                    .GroupBy(p => p.start)
                    .Select(p => new {start = p.Key, frequency = p.Sum(f => f.frequency)})
                    .OrderBy(p => p.start);
                var endTimes = meetings
                    .Where(m => m.Pattern.Days.Contains(day1))
                    .Select(p => new {end = p.Pattern.End, frequency = p.Frequency})
                    .GroupBy(p => p.end)
                    .Select(p => new {end = p.Key, frequency = p.Sum(f => f.frequency)})
                    .OrderBy(p => p.end);
                sw.WriteLine("{0} Start Times:", day1);
                foreach (var start in startTimes)
                    sw.WriteLine("{0}: {1}", start.start.ToShortTimeString(), start.frequency);
                sw.WriteLine("{0} End Times:" , day1);
                foreach (var end in endTimes)
                    sw.WriteLine("{0}: {1}", end.end.ToShortTimeString(), end.frequency);
            }
            return sw;
        }




    }

}
