using Application.Guest.Requests;
using Application.Guest.Responses;

namespace Application.Guest.Ports
{
    public interface IGuestRepository
    {
        Task<GuestResponse> CreateGuest(CreateGuestRequest request);
        Task<GuestResponse> GetGuest(int request);
    }
}
