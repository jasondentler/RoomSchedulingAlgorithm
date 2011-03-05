using System;
using System.Collections.Generic;

namespace AlgorithmRunner.Entities
{

    public class TimePatternGenerator
    {

        public TimePatternGenerator()
        {
        }

        public IEnumerable<TimePattern> Generate()
        {
            var mwf = DaysOfWeekBitMaskExtensions.Convert(new[]
                                                              {
                                                                  DaysOfWeekBitMask.Monday,
                                                                  DaysOfWeekBitMask.Wednesday,
                                                                  DaysOfWeekBitMask.Friday
                                                              });
            var tth = DaysOfWeekBitMaskExtensions.Convert(new[]
                                                              {
                                                                  DaysOfWeekBitMask.Tuesday,
                                                                  DaysOfWeekBitMask.Thursday
                                                              });
            var start = DateTime.Today.AddHours(8);
            while (start.Hour < 17)
            {
                var end = start.AddMinutes(50);
                yield return new TimePattern(mwf, start, end);
                start = start.AddHours(1);
            }

            start = DateTime.Today.AddHours(8);
            while (start.Hour < 12)
            {
                var end = start.AddMinutes(80);
                yield return new TimePattern(tth, start, end);
                start = start.AddMinutes(90);
            }

            start = DateTime.Today.AddHours(13);
            while (start.Hour < 17)
            {
                var end = start.AddMinutes(80);
                yield return new TimePattern(tth, start, end);
                start = start.AddMinutes(90);
            }

        }

    }

}
