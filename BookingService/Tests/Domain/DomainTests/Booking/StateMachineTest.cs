using Domain.Booking.Entities;
using Domain.Guest.Enums;
using Action = Domain.Guest.Enums.Action;

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
            Assert.AreEqual(booking.Status, Status.Created);
        }

        [Test]
        public void ShouldSetStatusToPaidWhenPayingForABookingWithCreatedStatus()
        {
            var booking = new Booking();

            booking.ChangeState(Action.Paying);

            Assert.AreEqual(booking.Status, Status.Paid);
        }

        [Test]
        public void ShouldSetStatusToCancelWhenCancellingForABookingWithCreatedStatus()
        {
            var booking = new Booking();

            booking.ChangeState(Action.Cancelling);

            Assert.AreEqual(booking.Status, Status.Canceled);
        }

        [Test]
        public void ShouldSetStatusToFinishedWhenFinishingForAPaidBook()
        {
            var booking = new Booking();
            booking.ChangeState(Action.Paying);
            booking.ChangeState(Action.Finishing);

            Assert.AreEqual(booking.Status, Status.Finished);
        }

        [Test]
        public void ShouldSetStatusToRefoundedWhenRefoundingForAPaidBook()
        {
            var booking = new Booking();
            booking.ChangeState(Action.Paying);
            booking.ChangeState(Action.Refounding);

            Assert.AreEqual(booking.Status, Status.Refounded);
        }

        [Test]
        public void ShouldSetStatusToCreatedWhenReopeningACanceledBooking()
        {
            var booking = new Booking();

            booking.ChangeState(Action.Cancelling);
            booking.ChangeState(Action.Reopenning);

            Assert.AreEqual(booking.Status, Status.Created);
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

            Assert.AreEqual(booking.Status, Status.Finished);
        }
    }
}