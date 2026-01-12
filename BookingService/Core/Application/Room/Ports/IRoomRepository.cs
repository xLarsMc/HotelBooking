using Application.Room.Request;
using Application.Room.Responses;

namespace Application.Room.Ports
{
    public interface IRoomRepository
    {
        Task<RoomResponse> CreateRoom(CreateRoomRequest request);
        Task<RoomResponse> GetRoom(int roomId);
    }
}
