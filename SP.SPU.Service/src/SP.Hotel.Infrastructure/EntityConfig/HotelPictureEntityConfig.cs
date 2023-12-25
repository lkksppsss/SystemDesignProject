using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SP.SPU.Domian.AggregatesModel.HotelAggregate;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace SP.SPU.Infrastructure.EntityConfig;

public class HotelPictureEntityConfig : IEntityTypeConfiguration<HotelPictureEntity>
{
    public void Configure(EntityTypeBuilder<HotelPictureEntity> config)
    {
        config.ToTable("hotel_pictures", DataContext.DEFAULT_SCHEMA);
        config.HasKey(i => i.Id);
        config.Ignore(i => i.DomainEvents);

        config.Property(i => i.Type).HasColumnName("type")
            .HasConversion(new ValueConverter<HotelPicType, int>(from => from.Id, to => HotelPicType.From(to)));
        config.Property(i => i.Url).HasColumnName("url");
        config.Property(i => i.CreateTime).HasColumnName("create_time");

        config.Property<int>("HotelEntityId").HasColumnName("hotel_id").IsRequired(true);
    }
}
