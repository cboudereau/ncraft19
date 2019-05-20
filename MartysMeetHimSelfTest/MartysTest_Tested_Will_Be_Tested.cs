using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using MartyMeetsHimSelf.Time;
using MartyMeetsHimSelf.BackToDomainInTheFuture;
using MartyMeetsHimSelf.TimeTravelling;

namespace MartysMeetHimSelfTest
{
    public class MartysMeetsTests
    {
        [Fact]
        public void Marty_Can_Be_Activated_By_TimeEvent()
        {
            // Arrange
            bool expectedValue = true;
            Marty mart = new Marty("Marty1985Timeline", new List<(DateTime, DateTime)> { (DateTime.Parse("1985-10-21T10:05:00Z"), DateTime.Parse("1985-10-21T10:10:00Z")) });
            TimeEvent te = new TimeEvent(DateTime.Parse("1985-10-21T10:06:00Z"));
            mart.OnTimeEvent(te);
            //Act
            bool mustBetrue = mart.IsActivated;

            // Assert
            Assert.Equal(expectedValue, mustBetrue);
        }

        [Fact]
        public void Marty_Can_Be_DesActivated_By_TimeEvent()
        {
            // Arrange
            bool expectedValue = false;
            Marty mart = new Marty("Marty1985Timeline", new List<(DateTime, DateTime)> { (DateTime.Parse("1985-10-21T10:05:00Z"), DateTime.Parse("1985-10-21T10:10:00Z")) });
            TimeEvent te1 = new TimeEvent(DateTime.Parse("1985-10-21T10:06:00Z"));
            TimeEvent te2 = new TimeEvent(DateTime.Parse("1985-10-21T10:10:00Z"));
            mart.OnTimeEvent(te1);
            mart.OnTimeEvent(te2);
            //Act
            bool mustBetrue = mart.IsActivated;

            // Assert
            Assert.Equal(expectedValue, mustBetrue);
        }

        [Fact]
        public void Marty_Should_Be_Activated()
        {
            // Arrange
            bool expectedValue = true;
            GroupOfMarty gom = new GroupOfMarty();
            gom.AddMarty(new Marty("Marty1985Timeline", new List<(DateTime, DateTime)> { (DateTime.Parse("1985-10-21T10:05:00Z"), DateTime.Parse("1985-10-21T10:10:00Z")) }));
            //gom.AddMarty(new Marty("Marty2015Timeline", new List<(DateTime, DateTime)> { (DateTime.Parse("1985-10-21T10:09:00Z"), DateTime.Parse("1985-10-21T10:17:00Z")) }));
            TimeEvent te = new TimeEvent(DateTime.Parse("1985-10-21T10:06:00Z"));
            gom.OnTimeEvent(te);
            //Act
            bool mustBetrue = gom.GetMarty("Marty1985Timeline").IsActivated;

            // Assert
            Assert.Equal(expectedValue, mustBetrue);
        }

        [Fact]
        public void Can_Find_ActivatedMart_From_Group_Of_Marty()
        {
            // Arrange
            GroupOfMarty gom = new GroupOfMarty();
            var expectedMarty    = new Marty("Marty1985Timeline", new List<(DateTime, DateTime)> { (DateTime.Parse("1985-10-21T10:05:00Z"), DateTime.Parse("1985-10-21T10:10:00Z")) });
            var notExpectedMarty = new Marty("Marty2015Timeline", new List<(DateTime, DateTime)> { (DateTime.Parse("1985-10-23T10:09:00Z"), DateTime.Parse("1985-10-23T10:17:00Z")) });
            gom.AddMarty(expectedMarty);
            gom.AddMarty(notExpectedMarty);
            TimeEvent te = new TimeEvent(DateTime.Parse("1985-10-21T10:06:00Z"));
            gom.OnTimeEvent(te);
            //Act
            var maybeGoodMart = gom.GetActivatedMarty();

            // Assert
            Assert.Single(maybeGoodMart);
            Assert.Equal(expectedMarty, maybeGoodMart.First());
        }

        [Fact]
        public void Can_Find_Time_Interval_Where_Martys_Meet_Each_Other()
        {
            // Arrange
            var time1 = DateTime.Parse("1985-10-21T10:05:00Z");
            var time2 = DateTime.Parse("1985-10-21T10:10:00Z");
            var time3 = DateTime.Parse("1985-10-21T10:09:00Z");
            var time4 = DateTime.Parse("1985-10-21T10:17:00Z");
            var expectedInterval = new TimePeriod(time3, time2);
            var events = (new List<DateTime>(4) { time1, time3, time2, time4 })
                         //.OrderBy(dateTime => dateTime)
                         .Select(dateTime => new TimeEvent(dateTime));
            GroupOfMarty gom = new GroupOfMarty();
            var marty1 = new Marty("MartyTimeline1", new List<(DateTime, DateTime)> { (time1, time2) });
            var marty2 = new Marty("MartyTimeline2", new List<(DateTime, DateTime)> { (time3, time4) });
            gom.AddMarty(marty1);
            gom.AddMarty(marty2);
            var deloreane = new TimeMachine();

            //Act
            MartysMeetEachOther mmeo = deloreane.PlayWithTime(gom, events);

            // Assert
            Assert.Single(mmeo.TimePeriods);
            Assert.Equal(mmeo.TimePeriods[0], expectedInterval);
        }
    }
}
