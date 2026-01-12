using Domain.Room.Ports;
using Domain.Guest.ValueObjects;
using Domain.Room.Exceptions;
using Domain.Guest.Enums;
using Domain.Booking.Entities;

namespace Domain.Room.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public bool InMaintenance { get; set; }
        public Price Price { get; set; }
        public ICollection<Domain.Booking.Entities.Booking> Bookings { get; set; }
        public bool isAvailable
        {
            get
            {
                if (InMaintenance || HasGuest) return false;
                
                return true;
            }
        }

        public bool HasGuest 
        {
            get
            {
                var notAvaliableStatus = new List<Status>()
                {
                    Status.Paid,
                };

                //Sabendo que na room agora, estou pegando os bookings, quando for carregar do banco, tenho que incluir na busca de room, os aggregate de bookings junto. Caso contrário,
                //ele não trará essas informaçoes e jogará uma exception aqui. Nesse caso, preciso carregar pra ver se os bookings que possuem essa room estão disponíveis, ou seja, não estão
                //como criado nem como pago. Se sim, significa true, que significa que há guest já nessa room com booking ativo, travando a criação de outro booking para essa room.
                return this.Bookings.Where(b => b.Room.Id == this.Id && 
                                           notAvaliableStatus.Contains(b.Status)).Count() > 0;
            }
        }

        private void ValidateState()
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                throw new InvalidRoomDataException();
            }

            if (this.Price == null || this.Price.Value < 10)
            {
                throw new InvalidRoomPriceException();
            }
        }

        public bool CanBeBooked()
        {
            try
            {
                this.ValidateState();
            }
            catch (Exception)
            {
                return false;
            }

            if (!this.isAvailable) return false;

            return true;
        }

        public async Task Save(Domain.Room.Ports.IRoomRepository roomRepository)
        {
            this.ValidateState();

            if (this.Id == 0) this.Id = await roomRepository.CreateRoom(this);
        }
    }
}
