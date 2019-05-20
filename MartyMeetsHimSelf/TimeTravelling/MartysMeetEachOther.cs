using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using MartyMeetsHimSelf.Time;
using MartyMeetsHimSelf.BackToDomainInTheFuture;

namespace MartyMeetsHimSelf.TimeTravelling
{
    public class MartysMeetEachOther
    {
        private readonly List<UbiquitiousMartys> _ubiquitiousMartys = new List<UbiquitiousMartys>();
        private IPeriodState _state;

        public MartysMeetEachOther() => _state = new EmptyPeriod(_ubiquitiousMartys.Add);

        public List<TimePeriod> TimePeriods 
            => _ubiquitiousMartys.Select(marts => marts.Period).ToList();

        public void AddMartys(IEnumerable<Marty> martys, DateTime time) => _state = _state.Next(time, martys.Select(elem => elem.Name).ToList());

        public override string ToString()
        {
            var sb = new StringBuilder();
            _ubiquitiousMartys.ForEach(marty => sb.AppendLine(marty.ToString()));
            return sb.ToString();
        }
    }
}
