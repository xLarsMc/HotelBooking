using Domain.Guest.Ports;
using Microsoft.EntityFrameworkCore;

namespace Data.Guest
{
    public class GuestRepository : IGuestRepository
    {
        private readonly HotelDbContext _hotelDbContext;
        public GuestRepository(HotelDbContext hotelDbContext)
        {
            _hotelDbContext = hotelDbContext;
        }
        public async Task<int> CreateGuest(Domain.Guest.Entites.Guest guest)
        {
            _hotelDbContext.Guests.Add(guest);
            await _hotelDbContext.SaveChangesAsync();
            return guest.Id;
        }

        public Task<Domain.Guest.Entites.Guest?> GetGuest(int id)
        {
            return _hotelDbContext.Guests.Where(guest => guest.Id == id).FirstOrDefaultAsync();

        }
    }
}
