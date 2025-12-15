using Anirut.Evacuation.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Anirut.Evacuation.Api.Data.Configurations;

public class VehicleConfiguration : IEntityTypeConfiguration<VehicleEntity>
{
    public void Configure(EntityTypeBuilder<VehicleEntity> builder)
    {
        builder.ToTable("Vehicle");
        builder.HasKey(v => v.VehicleId);
        builder.OwnsOne(v => v.Location);
    }
}
