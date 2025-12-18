using Domain.Entites;

namespace Domain.Ports
{
    public interface IGuestRepository
    {
        Task<Guest?> GetGuest(int id);
        Task<int> CreateGuest(Guest guest);
    }
}
