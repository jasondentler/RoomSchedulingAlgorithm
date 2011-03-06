using System;
using AlgorithmRunner.Entities;

namespace AlgorithmRunner.Data
{

    public class TimePatternGenerator
    {

        private readonly Database _database;

        public TimePatternGenerator(Database database)
        {
            _database = database;
        }

        public void Generate()
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
                var pattern = new TimePattern(mwf, start, end);
                _database.AddTimePattern(pattern);
                start = start.AddHours(1);
            }

            start = DateTime.Today.AddHours(8);
            while (start.Hour < 12)
            {
                var end = start.AddMinutes(80);
                var pattern = new TimePattern(tth, start, end);
                _database.AddTimePattern(pattern);
                start = start.AddMinutes(90);
            }

            start = DateTime.Today.AddHours(13);
            while (start.Hour < 17)
            {
                var end = start.AddMinutes(80);
                var pattern = new TimePattern(tth, start, end);
                _database.AddTimePattern(pattern);
                start = start.AddMinutes(90);
            }

        }

    }

}
