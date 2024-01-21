using OnlineStore.DAL.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace OnlineStore.DAL.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasOne(u => u.Role)
                .WithMany(r => r.Customers)
                .HasForeignKey(u => u.RoleId).IsRequired();

            builder.HasMany(r => r.Orders)
                .WithOne(u => u.Customer)
                .HasForeignKey(u => u.CustomerId);
        }
    }
}
