using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OnlineStore.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.DAL.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasOne(u => u.Customer)
                .WithMany(r => r.Orders)
                .HasForeignKey(u => u.CustomerId).IsRequired();

            builder.HasOne(u => u.Products)
                .WithMany(r => r.Orders).
                HasForeignKey(u => u.ProductsId).IsRequired();
        }
    }
}
