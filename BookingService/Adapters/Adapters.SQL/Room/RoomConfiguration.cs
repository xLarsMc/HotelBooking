using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Room
{
    public class RoomConfiguration : IEntityTypeConfiguration<Domain.Room.Entities.Room>
    {
        public void Configure(EntityTypeBuilder<Domain.Room.Entities.Room> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.OwnsOne(x => x.Price, price =>
            {
                price.Property(p => p.Value)
                     .HasPrecision(18, 2)
                     .IsRequired();

                price.Property(p => p.Currency)
                     .IsRequired();

                // opcional: evita tabela separada
                price.WithOwner();
            });
        }

    }
}
