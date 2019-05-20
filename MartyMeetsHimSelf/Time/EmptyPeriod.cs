using MartyMeetsHimSelf.BackToDomainInTheFuture;
using MartyMeetsHimSelf.TimeTravelling;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MartyMeetsHimSelf.Time
{

    public class EmptyPeriod : IPeriodState
    {
        readonly Action<UbiquitiousMartys> _adder;

        public EmptyPeriod(Action<UbiquitiousMartys> adder) => _adder = adder;

        public IPeriodState Next(DateTime date, List<MartyVersion> martys)
            => (martys.Count > 1 ? new OpenTimePeriod(date, martys.Select(marts => new MartyVersion(marts.Value)).ToList(), _adder) as IPeriodState : this as IPeriodState);
    }
}
