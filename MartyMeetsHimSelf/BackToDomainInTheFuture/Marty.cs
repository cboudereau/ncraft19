using System;
using System.Linq;
using System.Collections.Generic;
using MartyMeetsHimSelf.Time;

namespace MartyMeetsHimSelf.BackToDomainInTheFuture
{
    public class Marty
    {
        private readonly List<TimePeriod> _periods;

        public MartyVersion Name { get; }

        public Marty(string name, List<(DateTime, DateTime)> list)
        {
            Name = new MartyVersion(name);
            _periods = list.Select(tuple => new TimePeriod(tuple)).ToList();
        }

        public bool IsActivated { get; internal set; }

        public void OnTimeEvent(TimeEvent te)
        {
            var isIn = false;
            foreach (var period in _periods)
            {
                isIn |= period.IsIn(te.SignalDate);
            }
            IsActivated = isIn;
        }
    }
}
