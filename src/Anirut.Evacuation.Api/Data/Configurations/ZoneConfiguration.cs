using Anirut.Evacuation.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Anirut.Evacuation.Api.Data.Configurations;

public class ZoneConfiguration : IEntityTypeConfiguration<ZoneEntity>
{
    public void Configure(EntityTypeBuilder<ZoneEntity> builder)
    {
        builder.ToTable("Zone");
        builder.HasKey(z => z.ZoneId);
        builder.OwnsOne(z => z.Location);
    }
}
