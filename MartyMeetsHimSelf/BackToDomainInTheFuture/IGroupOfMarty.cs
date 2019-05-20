using System.Collections.Generic;
using MartyMeetsHimSelf.Time;

namespace MartyMeetsHimSelf.BackToDomainInTheFuture
{
    public interface IGroupOfMarty
    {
        void AddMarty(Marty marty);
        IEnumerable<Marty> GetActivatedMarty();
        Marty GetMarty(string lookForOneMarty);
        void OnTimeEvent(TimeEvent te);
    }
}