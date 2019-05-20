using MartyMeetsHimSelf.BackToDomainInTheFuture;
using MartyMeetsHimSelf.Time;
using MartyMeetsHimSelf.TimeTravelling;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AskTimes
{
    // Timeline 1 where 1985's Marty was there
    //  1985-10-21T10:05:00Z -> 1985-10-21T14:05:00Z
    // Timeline 2 where 1985's Marty went in the past then back to the future
    //  1955-11-12T13:50:00Z -> 1955-11-12T23:30:00Z
    //  1985-10-21T15:07:00Z -> 1985-10-21T16:00:00Z
    //  2015-10-21T15:00:00Z -> 2015-10-21T18:00:00Z
    // Timeline 2' where 2015's Marty is alive
    //  2015-01-01T00:00:00Z -> 2016-01-01T00:00:00Z
    // Timeline 3 where 1985's Marty went in the far west!
    //  1955-11-12T17:00:00Z -> 1955-11-12T23:30:00Z
    //  1885-10-21T10:00:00Z -> 1885-10-22T23:00:00Z
    //  2015-10-22T15:05:00Z -> 2015-10-22T20:00:00Z
    class Program
    {
        static void Main(string[] args)
        {
            var events = (new List<DateTime>(16) { DateTime.Parse("1985-10-21T10:05:00Z"), DateTime.Parse("1985-10-21T14:05:00Z"),
                                                   DateTime.Parse("1955-11-12T13:50:00Z"), DateTime.Parse("1955-11-12T23:30:00Z"),
                                                   DateTime.Parse("1985-10-21T15:07:00Z"), DateTime.Parse("1985-10-21T16:00:00Z"),
                                                   DateTime.Parse("2015-10-21T15:00:00Z"), DateTime.Parse("2015-10-21T18:00:00Z"),
                                                   DateTime.Parse("2015-01-01T00:00:00Z"), DateTime.Parse("2016-01-01T00:00:00Z"),
                                                   DateTime.Parse("1955-11-12T17:00:00Z"), DateTime.Parse("1955-11-12T23:30:00Z"),
                                                   DateTime.Parse("1885-10-21T10:00:00Z"), DateTime.Parse("1885-10-22T23:00:00Z"),
                                                   DateTime.Parse("2015-10-22T15:05:00Z"), DateTime.Parse("2015-10-22T20:00:00Z")})
                            .OrderBy(d => d)
                            .Distinct()
                            .Select(date => new TimeEvent(date));
            var marty1      = new Marty("MartyTimeline1", new List<(DateTime, DateTime)> { (DateTime.Parse("1985-10-21T10:05:00Z"), DateTime.Parse("1985-10-21T14:05:00Z")) });
            var marty2      = new Marty("MartyTimeline2", new List<(DateTime, DateTime)> { (DateTime.Parse("1955-11-12T13:50:00Z"), DateTime.Parse("1955-11-12T23:30:00Z")),
                                                                                           (DateTime.Parse("1985-10-21T15:07:00Z"), DateTime.Parse("1985-10-21T16:00:00Z")),
                                                                                           (DateTime.Parse("2015-10-21T15:00:00Z"), DateTime.Parse("2015-10-21T18:00:00Z")) });
            var marty2prime = new Marty("MartyTimeline2'", new List<(DateTime, DateTime)> { (DateTime.Parse("2015-01-01T00:00:00Z"), DateTime.Parse("2016-01-01T00:00:00Z")) });
            var marty3      = new Marty("MartyTimeline3", new List<(DateTime, DateTime)> { (DateTime.Parse("1955-11-12T17:00:00Z"), DateTime.Parse("1955-11-12T23:30:00Z")),
                                                                                           (DateTime.Parse("1885-10-21T10:00:00Z"), DateTime.Parse("1885-10-22T23:00:00Z")),
                                                                                           (DateTime.Parse("2015-10-22T15:05:00Z"), DateTime.Parse("2015-10-22T20:00:00Z")) });
            var gom = new GroupOfMarty();
            gom.AddRangeOfMarty(new List<Marty>(4) { marty1, marty2, marty2prime, marty3 });


            var deloreane = new TimeMachine();

            try
            {
                MartysMeetEachOther mmeo = deloreane.PlayWithTime(gom, events);
                Console.WriteLine(mmeo);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
            Console.ReadKey();
        }
    }
}
