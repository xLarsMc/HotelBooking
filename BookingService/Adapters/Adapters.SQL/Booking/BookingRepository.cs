using Domain.Booking.Ports;
using Microsoft.EntityFrameworkCore;

namespace Data.Booking
{
    public class BookingRepository : IBookingRepository
    {
        private readonly HotelDbContext _dbContext;

        public BookingRepository(HotelDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Domain.Booking.Entities.Booking> CreateBooking(Domain.Booking.Entities.Booking booking)
        {
            _dbContext.Bookings.Add(booking);
            await _dbContext.SaveChangesAsync();
            return booking;
        }

        public Task<Domain.Booking.Entities.Booking> Get(int id)
        {
            //Uma vez que nem todas informações além de Id de guest e room são necessários, o certo seria criar um QueryModel, a fim de pegar apenas as informações necessárias, e não todo o objeto.
            return _dbContext.Bookings
                .Include(b => b.Guest)
                .Include(b => b.Room)
                .Where(b => b.Id == id).FirstAsync();
        }
    }
}
