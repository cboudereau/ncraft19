using System;
using System.Collections.Generic;
using MartyMeetsHimSelf.TimeTravelling;
using MartyMeetsHimSelf.BackToDomainInTheFuture;

namespace MartyMeetsHimSelf.Time
{
    public class OpenTimePeriod : IPeriodState
    {
        private readonly List<MartyVersion> _martys;
        private readonly Action<UbiquitiousMartys> _adder;

        public DateTime Start { get; }

        public OpenTimePeriod(DateTime start, List<MartyVersion> martys, Action<UbiquitiousMartys> adder)
        {
            _martys = martys;
            Start   = start;
            _adder  = adder;
        }

        public IPeriodState Next(DateTime date, List<MartyVersion> martys)
        {
            _adder(new UbiquitiousMartys(_martys, this, date));
            if(martys.Count <= 1)
            {
                return new EmptyPeriod(_adder);
            }
            return new OpenTimePeriod(date, martys, _adder);
        }
    }
}
