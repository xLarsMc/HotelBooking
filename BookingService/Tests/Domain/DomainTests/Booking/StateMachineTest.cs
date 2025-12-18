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


        //Todos esses são positive paths, ou seja, caminhos perfeitos e ideias.
        [Test]
        public void ShouldAlwaysStartsWithCreatedStatus()
        {
            var booking = new Booking();
            Assert.AreEqual(booking.CurrentStatus, Status.Created);
        }

        [Test]
        public void ShouldSetStatusToPaidWhenPayingForABookingWithCreatedStatus()
        {
            var booking = new Booking();

            booking.ChangeState(Action.Paying);

            Assert.AreEqual(booking.CurrentStatus, Status.Paid);
        }

        [Test]
        public void ShouldSetStatusToCancelWhenCancellingForABookingWithCreatedStatus()
        {
            var booking = new Booking();

            booking.ChangeState(Action.Cancelling);

            Assert.AreEqual(booking.CurrentStatus, Status.Canceled);
        }

        [Test]
        public void ShouldSetStatusToFinishedWhenFinishingForAPaidBook()
        {
            var booking = new Booking();
            booking.ChangeState(Action.Paying);
            booking.ChangeState(Action.Finishing);

            Assert.AreEqual(booking.CurrentStatus, Status.Finished);
        }

        [Test]
        public void ShouldSetStatusToRefoundedWhenRefoundingForAPaidBook()
        {
            var booking = new Booking();
            booking.ChangeState(Action.Paying);
            booking.ChangeState(Action.Refounding);

            Assert.AreEqual(booking.CurrentStatus, Status.Refounded);
        }

        [Test]
        public void ShouldSetStatusToCreatedWhenReopeningACanceledBooking()
        {
            var booking = new Booking();

            booking.ChangeState(Action.Cancelling);
            booking.ChangeState(Action.Reopenning);

            Assert.AreEqual(booking.CurrentStatus, Status.Created);
        }

        //Esses são negative paths, ou seja, onde não deve haver a mudança. Como por exemplo, mando um 
        //caso impossível de haver mudança, como pedir reebolso quando já finalizei a reserva.

        [Test]
        public void ShouldNotChanceStatusWhenRefoundingABookingwithCreatedStatus()
        {
            var booking = new Booking();

            booking.ChangeState(Action.Paying);
            booking.ChangeState(Action.Finishing);
            booking.ChangeState(Action.Refounding);

            Assert.AreEqual(booking.CurrentStatus, Status.Finished);
        }
    }
}