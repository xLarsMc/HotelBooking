using Domain.Guest.Enums;

namespace Application.Booking.DTO
{
    public class BookingDto
    {
        public BookingDto()
        {
            this.PlacedAt = DateTime.UtcNow;
        }

        public int Id { get; set; }
        public DateTime PlacedAt { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int RoomId { get; set; }
        public int GuestId { get; set; }
        private Status Status { get; set; }

        public static Domain.Booking.Entities.Booking MapToEntity(BookingDto bookingDto)
        {
            return new Domain.Booking.Entities.Booking
            {
                Id = bookingDto.Id,
                Start = bookingDto.Start,
                Guest = new Domain.Guest.Entites.Guest { Id = bookingDto.GuestId },
                Room = new Domain.Room.Entities.Room { Id = bookingDto.RoomId },
                End = bookingDto.End,
                PlacedAt = bookingDto.PlacedAt
            };
        }

        public static BookingDto MapToDto(Domain.Booking.Entities.Booking booking)
        {
            return new BookingDto
            {
                Id = booking.Id,
                End = booking.End,
                GuestId = booking.Guest.Id,
                PlacedAt = booking.PlacedAt,
                RoomId = booking.Room.Id,
                Status = booking.Status,
                Start = booking.Start,
            };
        }
    }
}
