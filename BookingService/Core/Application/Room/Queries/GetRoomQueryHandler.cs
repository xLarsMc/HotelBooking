using Application.Room.Dtos;
using Domain.Room.Ports;
using Application.Room.Responses;
using MediatR;

namespace Application.Room.Queries
{
    public class GetRoomQueryHandler : IRequestHandler<GetRoomQuery, RoomResponse>
    {
        private readonly IRoomRepository _roomRepository;

        public GetRoomQueryHandler(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<RoomResponse> Handle(GetRoomQuery request, CancellationToken cancellationToken)
        {
            var room = await _roomRepository.GetRoom(request.Id);

            if (room == null)
                return new RoomResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.ROOM_NOT_FOUND,
                    Message = "Could not find a Room with the given id"
                };

            return new RoomResponse
            {
                Data = RoomDto.MapToDto(room),
                Success = true
            };
        }
    }
}
