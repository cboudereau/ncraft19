using System.Linq;
using System.Collections.Generic;
using MartyMeetsHimSelf.Time;

namespace MartyMeetsHimSelf.BackToDomainInTheFuture
{
    public class GroupOfMarty : IGroupOfMarty
    {
        private readonly List<Marty> _registeredMartys = new List<Marty>(5);
        public void AddMarty(Marty marty) => _registeredMartys.Add(marty);
        public void AddRangeOfMarty(List<Marty> martys) => martys.ForEach(marty => _registeredMartys.Add(marty));

        public Marty GetMarty(string lookForOneMarty)
            => _registeredMartys.FirstOrDefault(mart => mart.Name.Value == lookForOneMarty);

        public void OnTimeEvent(TimeEvent te)
            => _registeredMartys.ForEach(mart => mart.OnTimeEvent(te));

        public IEnumerable<Marty> GetActivatedMarty()
            => _registeredMartys.Where(mart => mart.IsActivated);
    }
}
