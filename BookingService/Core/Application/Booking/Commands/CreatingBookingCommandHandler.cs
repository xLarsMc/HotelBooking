using Application.Booking.DTO;
using Application.Booking.Responses;
using Domain.Guest.Ports;
using Domain.Booking.Exceptions;
using Domain.Booking.Ports;
using Domain.Room.Exceptions;
using Domain.Room.Ports;
using MediatR;

namespace Application.Booking.Commands
{
    public class CreatingBookingCommandHandler : IRequestHandler<CreateBookingCommand, BookingResponse>
    {
        //Se fosse usar CQRS desde o começo, o que foi feito em bookingManager seria feito diretamente aqui. Agora, por ser uma adição, só vou puxar ele de lá pra não duplicar código nem apagar nada.
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IGuestRepository _guestRepository;
        public CreatingBookingCommandHandler(IBookingRepository bookingRepository, IRoomRepository roomRepository, IGuestRepository guestRepository)
        {
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
            _guestRepository = guestRepository;
        }

        public async Task<BookingResponse> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var bookingDto = request.BookingDto;
                var booking = BookingDto.MapToEntity(bookingDto);
                booking.Guest = await _guestRepository.GetGuest(bookingDto.GuestId);
                booking.Room = await _roomRepository.GetRoom(bookingDto.RoomId);

                await booking.Save(_bookingRepository);
                bookingDto.Id = booking.Id;

                return new BookingResponse
                {
                    Data = bookingDto,
                    Success = true,

                };
            }
            catch (PlacedAtIsARequiredInformationException)
            {
                return new BookingResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.BOOKING_MISSING_REQUIRED_INFORMATION,
                    Message = "PlacedAt is a required information"
                };
            }
            catch (StartDateTimeIsRequiredException)
            {
                return new BookingResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.BOOKING_MISSING_REQUIRED_INFORMATION,
                    Message = "Start is a required information"
                };
            }
            catch (EndDateTimeIsRequiredException)
            {
                return new BookingResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.BOOKING_MISSING_REQUIRED_INFORMATION,
                    Message = "End is a required information"
                };
            }
            catch (RoomIsRequiredException)
            {
                return new BookingResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.BOOKING_MISSING_REQUIRED_INFORMATION,
                    Message = "Room is a required information"
                };
            }
            catch (GuestIsRequiredException)
            {
                return new BookingResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.BOOKING_MISSING_REQUIRED_INFORMATION,
                    Message = "Guest is a required information"
                };
            }
            catch (RoomCannotBeBookedException)
            {
                return new BookingResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.BOOKING_ROOM_CANNOT_BE_BOOKED,
                    Message = "The selected Room is not avaliable"
                };
            }
            catch (Exception)
            {
                return new BookingResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.BOOKING_NOT_FOUND,
                    Message = "There was an error when saving to DB"
                };
            }
        }
    }
}
