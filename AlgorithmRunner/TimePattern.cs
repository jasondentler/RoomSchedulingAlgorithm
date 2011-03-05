using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgorithmRunner
{

    public struct TimePattern
    {
        public DaysOfWeekBitMask Days;
        public DateTime Start;
        public DateTime End;

        public TimePattern(DaysOfWeekBitMask days, DateTime start, DateTime end)
        {
            Days = days;
            Start = start;
            End = end;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} to {2}",
                                 Days.Convert(),
                                 Start.ToShortTimeString(),
                                 End.ToShortTimeString());
        }

        public override bool Equals(object obj)
        {
            var other = (TimePattern) obj;
            return Days == other.Days &&
                   Start == other.Start &&
                   End == other.End;
        }

    }

    [Flags]
    public enum DaysOfWeekBitMask
    {
        Sunday = 1,
        Monday = 2,
        Tuesday = 4,
        Wednesday = 8,
        Thursday = 16,
        Friday = 32,
        Saturday = 64
    }

    public static class DaysOfWeekBitMaskExtensions
    {
        public static DaysOfWeekBitMask Convert(IEnumerable<string> days)
        {
            return Convert(days.Select(Convert));
        }

        public static DaysOfWeekBitMask Convert(IEnumerable<DaysOfWeekBitMask> days)
        {
            return days.Aggregate<DaysOfWeekBitMask, DaysOfWeekBitMask>
                (0, (current, day) => current | day);
        }

        public static DaysOfWeekBitMask Convert(string day)
        {
            switch (day.ToUpperInvariant())
            {
                case "SU":
                    return DaysOfWeekBitMask.Sunday;
                case "M":
                    return DaysOfWeekBitMask.Monday;
                case "T":
                case "TU":
                    return DaysOfWeekBitMask.Tuesday;
                case "W":
                    return DaysOfWeekBitMask.Wednesday;
                case "TH":
                    return DaysOfWeekBitMask.Thursday;
                case "F":
                    return DaysOfWeekBitMask.Friday;
                case "S":
                case "SA":
                    return DaysOfWeekBitMask.Saturday;
                default:
                    throw new NotSupportedException();
            }
        }

        public static string Convert(this DaysOfWeekBitMask days)
        {
            var result = new StringBuilder();
            if (days.Contains(DaysOfWeekBitMask.Sunday))
                result.Append("Su");
            if (days.Contains(DaysOfWeekBitMask.Monday))
                result.Append("M");
            if (days.Contains(DaysOfWeekBitMask.Tuesday))
                result.Append("Tu");
            if (days.Contains(DaysOfWeekBitMask.Wednesday))
                result.Append("W");
            if (days.Contains(DaysOfWeekBitMask.Thursday))
                result.Append("Th");
            if (days.Contains(DaysOfWeekBitMask.Friday))
                result.Append("F");
            if (days.Contains(DaysOfWeekBitMask.Saturday))
                result.Append("Sa");
            return result.ToString();
        }

        public static bool Contains(this DaysOfWeekBitMask days, DaysOfWeekBitMask day)
        {
            return (days & day) == day;
        }

    }


}
