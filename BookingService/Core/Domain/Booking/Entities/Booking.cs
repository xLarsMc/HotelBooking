using Domain.Booking.Exceptions;
using Domain.Booking.Ports;
using Domain.Guest.Enums;
using Domain.Room.Exceptions;
using Action = Domain.Guest.Enums.Action;

namespace Domain.Booking.Entities
{
    public class Booking
    {
        public Booking () { Status = Status.Created; }
        public int Id { get; set; }
        public DateTime PlacedAt { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public Room.Entities.Room Room { get; set; }
        public Guest.Entites.Guest Guest { get; set; }
        public Status Status { get; set; }
        //StateMachine design pattern - Mudança desse estado definido já na classe, encapsulando e evitando muudanças inesperadas e foras de lógica.
        public void ChangeState(Action action)
        {
            Status = (Status, action) switch
            {
                (Status.Created,  Action.Paying) => Status.Paid,
                (Status.Created,  Action.Cancelling) => Status.Canceled,
                (Status.Paid,     Action.Finishing) => Status.Finished,
                (Status.Paid,     Action.Refounding) => Status.Refounded,
                (Status.Canceled, Action.Reopenning) => Status.Created,
                _ => Status
            };
        }

        private void ValidateState()
        {
            if(this.PlacedAt == default(DateTime))
            {
                throw new PlacedAtIsARequiredInformationException();
            }

            if (this.Start == default(DateTime))
            {
                throw new StartDateTimeIsRequiredException();
            }

            if (this.End == default(DateTime))
            {
                throw new EndDateTimeIsRequiredException();
            }

            if (this.Room == null)
            {
                throw new RoomIsRequiredException();
            }

            if (this.Guest == null)
            {
                throw new GuestIsRequiredException();
            }

            this.Guest.isValid();

            if (!this.Room.CanBeBooked()) throw new RoomCannotBeBookedException();
        }

        public async Task Save(IBookingRepository bookingRepository)
        {
            this.ValidateState();

            this.Guest.isValid();

            if (!this.Room.CanBeBooked()) throw new RoomCannotBeBookedException();

            if(this.Id == 0)
            {
                var response = await bookingRepository.CreateBooking(this);
                this.Id = response.Id;
            }
            else
            {

            }
        }
    }
}
