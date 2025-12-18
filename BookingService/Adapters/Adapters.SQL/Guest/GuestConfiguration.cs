using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Guest
{
    public class GuestConfiguration : IEntityTypeConfiguration<Domain.Entites.Guest>
    {
        public void Configure(EntityTypeBuilder<Domain.Entites.Guest> builder)
        {
            builder.HasKey(x => x.Id);
            builder.OwnsOne(x => x.DocumentId).Property(x => x.IdNumber);
            builder.OwnsOne(x => x.DocumentId).Property(x => x.DocumentType);
        }
    }
}
