using System;

namespace MartyMeetsHimSelf.Time
{
    public class TimePeriod :  IEquatable<TimePeriod>
    {
        public DateTime Start { get; }
        public DateTime Stop { get; }

        public TimePeriod(DateTime start, DateTime stop)
        {
            Start = start;
            Stop = stop;
        }

        public TimePeriod((DateTime, DateTime) startAndStop) : this(startAndStop.Item1, startAndStop.Item2) { }

        public bool IsIn(DateTime date) => date >= Start && date < Stop;

        public bool Equals(TimePeriod other)
            => other != null && other.Start.Equals(Start) && other.Stop.Equals(Stop);

        public override bool Equals(object obj) => Equals(obj as TimePeriod);

        public override int GetHashCode() => Start.GetHashCode() + 123 * Stop.GetHashCode();
    }
}
