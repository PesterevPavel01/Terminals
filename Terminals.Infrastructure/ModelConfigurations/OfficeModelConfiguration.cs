using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Terminals.Contracts.Entities;

namespace Terminals.Infrastructure.ModelConfigurations
{
    internal class OfficeModelConfiguration : IEntityTypeConfiguration<Office>
    {
        public void Configure(EntityTypeBuilder<Office> builder)
        {
            builder.ToTable(typeof(Office).Name);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code)
                .HasMaxLength(Office.CodesMaxLength);

            builder.Property(x => x.Uuid)
                .HasMaxLength(36);

            builder.Property(x => x.Type);

            builder.Property(x => x.CountryCode)
                .HasMaxLength(Office.CodesMaxLength);

            builder.Property(x => x.WorkTime)
                .HasMaxLength(Office.WorkTimeLength);

            builder.HasIndex(x => x.Uuid)
                .IsUnique(false);

            builder.HasIndex(x => x.AddressCity)
                .IsUnique(false);

            builder.HasIndex(x => new { x.AddressCity, x.AddressRegion })
                .IsUnique(false);

            builder.OwnsOne(x => x.Coordinates, coordinatesBuilder =>
            {
                coordinatesBuilder.ToJson();
            });

            builder.OwnsOne(x => x.Phones, phonesBuilder =>
            {
                phonesBuilder.ToJson();

                phonesBuilder.Property(x => x.Id);

                phonesBuilder
                    .Property(x => x.Additional)
                    .HasMaxLength(Phone.PhoneMaxLength);

                phonesBuilder
                    .Property(x => x.PhoneNumber)
                    .HasMaxLength(Phone.PhoneMaxLength);
            });
        }
    }
}
