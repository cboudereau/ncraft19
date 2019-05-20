using System;

namespace MartyMeetsHimSelf.Time
{
    public class TimeEvent
    {
        public DateTime SignalDate { get; }

        public TimeEvent(DateTime dateTime) => SignalDate = dateTime;
    }
}
