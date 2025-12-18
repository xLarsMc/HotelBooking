using Data.Guest;
using Data.Room;
using Microsoft.EntityFrameworkCore;
using Entites = Domain.Entites;

namespace Data
{
    public class HotelDbContext : DbContext
    {
        public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options) { }
        public virtual DbSet<Entites.Guest> Guests { get; set; }
        public virtual DbSet<Entites.Room> Rooms { get; set; }
        public virtual DbSet<Entites.Booking> Bookings { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new GuestConfiguration());
            modelBuilder.ApplyConfiguration(new RoomConfiguration());
        }
    }
}
