using Application.Booking.DTO;
using Application.Booking.Responses;
using MediatR;

namespace Application.Booking.Commands
{
    public class CreateBookingCommand : IRequest<BookingResponse>
    {
        public BookingDto BookingDto { get; set; }
    }
}
