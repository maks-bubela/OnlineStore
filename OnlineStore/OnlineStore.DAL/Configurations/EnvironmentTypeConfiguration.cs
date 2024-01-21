using OnlineStore.DAL.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace OnlineStore.DAL.Configurations
{
    public class EnvironmentTypeConfiguration : IEntityTypeConfiguration<EnvironmentType>
    {
        public void Configure(EntityTypeBuilder<EnvironmentType> builder)
        {
            builder.HasMany(e => e.BearerTokenSettings)
                .WithOne(b => b.EnvironmentType)
                .HasForeignKey(e => e.EnvironmentTypeId);
        }
    }
}
