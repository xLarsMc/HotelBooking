using Domain.Room.Entities;

namespace Domain.Room.Ports
{
    public interface IRoomRepository
    {
        Task<int> CreateRoom(Domain.Room.Entities.Room request);
        Task<Domain.Room.Entities.Room> GetRoom(int roomId);
        Task<Domain.Room.Entities.Room> GetRoomWithAggregate(int id);
    }
}
