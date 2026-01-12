using Domain.Guest.Entites;

namespace Domain.Guest.Ports
{
    public interface IGuestRepository
    {
        Task<Domain.Guest.Entites.Guest> GetGuest(int id);
        Task<int> CreateGuest(Domain.Guest.Entites.Guest guest);
    }
}
