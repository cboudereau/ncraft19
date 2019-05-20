using System.Linq;
using System.Collections.Generic;
using MartyMeetsHimSelf.Time;
using MartyMeetsHimSelf.BackToDomainInTheFuture;

namespace MartyMeetsHimSelf.TimeTravelling
{
    public class TimeMachine
    {
        public MartysMeetEachOther PlayWithTime(IGroupOfMarty gom, IEnumerable<TimeEvent> evts)
        {
            var toBeReturned = new MartysMeetEachOther();
            foreach(var evt in evts)
            {
                gom.OnTimeEvent(evt);
                var martys = gom.GetActivatedMarty().ToList();
                toBeReturned.AddMartys(martys, evt.SignalDate);
            }
            return toBeReturned;
        }
    }
}
