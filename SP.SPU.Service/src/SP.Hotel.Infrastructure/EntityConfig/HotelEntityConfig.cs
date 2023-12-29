using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SP.SPU.Domain.AggregatesModel.HotelAggregate;

namespace SP.SPU.Infrastructure.EntityConfig;

public class HotelEntityConfig : IEntityTypeConfiguration<HotelEntity>
{
    public void Configure(EntityTypeBuilder<HotelEntity> config)
    {
        config.ToTable("hotels", DataContext.DEFAULT_SCHEMA);
        config.HasKey(i => i.Id);
        config.Ignore(i => i.DomainEvents);

        config.Property(i => i.Name).HasColumnName("name");
        config.Property(i => i.City).HasColumnName("city");
        config.Property(i => i.Area).HasColumnName("area");
        config.Property(i => i.Address).HasColumnName("address");
        config.Property(i => i.Description).HasColumnName("description");
        config.Property(i => i.CreateTime).HasColumnName("create_time");

        config.Metadata.FindNavigation(nameof(HotelEntity.Pictures))?.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
