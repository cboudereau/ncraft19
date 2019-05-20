using System;
using System.Linq;
using System.Collections.Generic;
using MartyMeetsHimSelf.BackToDomainInTheFuture;
using MartyMeetsHimSelf.Time;

namespace MartyMeetsHimSelf.TimeTravelling
{
    public class UbiquitiousMartys
    {
        public List<MartyVersion> UbiquitiousMarty { get; }
        public TimePeriod Period { get; }

        public UbiquitiousMartys(List<MartyVersion> martys, TimePeriod period)
        {
            UbiquitiousMarty = martys;
            Period = period;
        }

        public UbiquitiousMartys(List<MartyVersion> martys, OpenTimePeriod period, DateTime stop)
            : this(martys, new TimePeriod(period.Start, stop)) { }

        public override string ToString()
            => $"{string.Join(", ", UbiquitiousMarty.Distinct())} can be founded between {Period.Start.ToUniversalTime()} and {Period.Stop.ToUniversalTime()}.";
    }
}
