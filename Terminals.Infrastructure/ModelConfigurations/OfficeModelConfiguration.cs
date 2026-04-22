using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Terminals.Contracts.Entities;

namespace Terminals.Infrastructure.ModelConfigurations
{
    internal class OfficeModelConfiguration : IEntityTypeConfiguration<Office>
    {
        public void Configure(EntityTypeBuilder<Office> builder)
        {
            builder.ToTable("offices");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code)
                .HasMaxLength(Office.CodesMaxLength);

            builder.Property(x => x.Uuid)
                .HasMaxLength(36);

            builder.Property(x => x.AddressCity)
                .HasMaxLength(Office.AddressCityMaxLength);

            builder.Property(x => x.AddressStreet)
                .HasMaxLength(Office.AddressStreetMaxLength);

            builder.Property(x => x.AddressRegion)
                .HasMaxLength(Office.AddressRegionMaxLength);

            builder.Property(x => x.AddressHouseNumber)
                .HasMaxLength(Office.AddressHouseNumberMaxLength);

            builder.Property(x => x.Type);

            builder.Property(x => x.CountryCode)
                .HasMaxLength(Office.CodesMaxLength);

            builder.Property(x => x.WorkTime)
                .HasMaxLength(Office.WorkTimeMaxLength);

            builder.HasIndex(x => x.Uuid);

            builder.HasIndex(x => x.AddressCity);

            builder.HasIndex(x => new { x.AddressCity, x.AddressRegion });

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
