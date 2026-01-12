using Application.Room.Request;
using Application.Room.Responses;
using Domain.Room.Entities;

namespace Application.Room.Ports
{
    public interface IRoomRepository
    {
        Task<Domain.Room.Entities.Room> CreateRoom(CreateRoomRequest request);
        Task<Domain.Room.Entities.Room> GetRoom(int roomId);
    }
}
