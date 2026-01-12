using Domain.Guest.Enums;

namespace Application.Room.Dtos
{
    public class RoomDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int LeveL { get; set; }
        public bool InMaintence { get; set; }
        public decimal Price { get; set; }
        public AcceptedCurrencies Currency { get; set; }

        public static Domain.Room.Entities.Room MapToEntity(RoomDto dto)
        {
            return new Domain.Room.Entities.Room
            {
                Id = dto.Id,
                Name = dto.Name,
                Level = dto.LeveL,
                InMaintenance = dto.InMaintence,
                Price = new Domain.Guest.ValueObjects.Price { Currency = dto.Currency, Value = dto.Price }
            };
        }

        public static RoomDto MapToDto(Domain.Room.Entities.Room room)
        {
            return new RoomDto
            {
                Id = room.Id,
                Name = room.Name,
                LeveL = room.Level,
                InMaintence = room.InMaintenance,
            };
        }
    }
}
