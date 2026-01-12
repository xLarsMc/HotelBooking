using Domain.Guest.DomainExceptions;
using Domain.Guest.Ports;
using Domain.Guest.ValueObjects;

namespace Domain.Guest.Entites
{
    public class Guest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public PersonId DocumentId { get; set; }
        private void ValidateState()
        {
            if (DocumentId == null || string.IsNullOrEmpty(DocumentId.IdNumber) || DocumentId.IdNumber.Length <= 3 || DocumentId.DocumentType == 0)
            {
                throw new InvalidPersonDocumentIdException();
            }

            if (string.IsNullOrEmpty(Name)    || 
                string.IsNullOrEmpty(Surname) || 
                string.IsNullOrEmpty(Email))
            {
                throw new MissingRequiringInformationException();
            }

            if (!Utils.Utils.ValidateEmail(Email))
            {
                throw new InvalidEmailException();
            }
        }

        public bool isValid()
        {
            this.ValidateState();
            return true;
        }
        public async Task Save(IGuestRepository guestRepository)
        {
            ValidateState();
            if(Id == 0)
            {
                Id = await guestRepository.CreateGuest(this);
            }
            else
            {
                //await guestRepository.updateGuest(this);
            }
        }
    }
}
