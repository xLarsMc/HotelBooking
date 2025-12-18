using Domain.Entites;
using Domain.Enums;
using NUnit.Framework;
using Action = Domain.Enums.Action;

namespace DomainTest.Bookings
{
    public class StateMachineTest
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void ShouldAlwaysStartsWithCreatedStatus()
        {
            var booking = new Booking();
            Assert.Equals(booking.CurrentStatus, Status.Created);
        }

        [Test]
        public void ShouldSetStatusToPaidWhenPayingForABookingWithCreatedStatus()
        {
            var booking = new Booking();

            booking.ChangeState(Action.Paying);

            Assert.Equals(booking.CurrentStatus, Status.Paid);
        }
    }
}