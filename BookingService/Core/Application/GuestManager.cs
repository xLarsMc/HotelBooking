using Application.Guest.DTO;
using Application.Guest.Ports;
using Application.Guest.Requests;
using Application.Guest.Responses;
using Domain.DomainExceptions;
using Domain.Ports;

namespace Application
{
    public class GuestManager : IGuestManager
    {
        private readonly IGuestRepository _guestRepository;
        public GuestManager(IGuestRepository guestRepository)
        {
            _guestRepository = guestRepository;
        }
        public async Task<GuestResponse> CreateGuest(CreateGuestRequest request)
        {
            try
            {
                var guest = GuestDTO.MapToEntity(request.Data);

                await guest.Save(_guestRepository);

                request.Data.Id = guest.Id;

                return new GuestResponse
                {
                    Data = request.Data,
                    Sucess = true,
                };

            }
            catch (InvalidPersonDocumentIdException)
            {
                return new GuestResponse
                {
                    Sucess = false,
                    ErrorCode = ErrorCodes.INVALID_PERSON_ID,
                    Message = "The ID passed is not valid"
                };
            }
            catch (MissingRequiringInformationException)
            {
                return new GuestResponse
                {
                    Sucess = false,
                    ErrorCode = ErrorCodes.MISSING_REQUIRED_INFORMATION,
                    Message = "Missing required information passed"
                };
            }
            catch (InvalidEmailException)
            {
                return new GuestResponse
                {
                    Sucess = false,
                    ErrorCode = ErrorCodes.INVALID_EMAIL,
                    Message = "The given email is not valid"
                };
            }
            catch (Exception)
            {
                return new GuestResponse
                {
                    Sucess = false,
                    ErrorCode = ErrorCodes.COULD_NOT_STORE_DATA,
                    Message = "There was an error when saving to DB"
                };
            }
        }

        public async Task<GuestResponse> GetGuest(int request)
        {
            var guest = await _guestRepository.GetGuest(request);

            if(guest == null)
            {
                return new GuestResponse
                {
                    Sucess = false,
                    ErrorCode = ErrorCodes.GUEST_NOT_FOUND,
                    Message = "No Guest record has been found with the given id"
                };
            }

            return new GuestResponse
            {
                Sucess = true,
                Data = GuestDTO.MapToDTO(guest),
            };
        }
    }
}
