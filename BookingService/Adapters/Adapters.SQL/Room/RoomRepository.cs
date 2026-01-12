using Domain.Room.Ports;
using Microsoft.EntityFrameworkCore;

namespace Data.Room
{
    public class RoomRepository : IRoomRepository
    {
        private readonly HotelDbContext _hotelDbContext;

        public RoomRepository(HotelDbContext hotelDbContext)
        {
            _hotelDbContext = hotelDbContext;
        }

        public async Task<int> CreateRoom(Domain.Room.Entities.Room room)
        {
            _hotelDbContext.Rooms.Add(room);
            await _hotelDbContext.SaveChangesAsync();
            return room.Id;
        }

        public async Task<Domain.Room.Entities.Room> GetRoom(int id)
        {
            return await _hotelDbContext.Rooms
                .Where(r => r.Id == id).FirstAsync();
        }

        public async Task<Domain.Room.Entities.Room> GetRoomWithAggregate(int id)
        {
            return await _hotelDbContext.Rooms
                .Include(r => r.Bookings)
                .Where(r => r.Id == id).FirstAsync();
        }
    }
}
