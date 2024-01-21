using OnlineStore.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace OnlineStore.DAL.Seeder
{
    public static class DatabaseSeeder
    {
        public static void SeedDataBase(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<EnvironmentType>().HasData(
                    new EnvironmentType { Id = 1, Name = "staging" },
                    new EnvironmentType { Id = 2, Name = "development" },
                    new EnvironmentType { Id = 3, Name = "testing" },
                    new EnvironmentType { Id = 4, Name = "production" }
                );

            modelBuilder.Entity<BearerTokenSetting>().HasData(
                    new BearerTokenSetting { Id = 1, EnvironmentTypeId = 1, LifeTime = 30 },
                    new BearerTokenSetting { Id = 2, EnvironmentTypeId = 2, LifeTime = 30 },
                    new BearerTokenSetting { Id = 3, EnvironmentTypeId = 3, LifeTime = 1 },
                    new BearerTokenSetting { Id = 4, EnvironmentTypeId = 4, LifeTime = 7 }
                );

            modelBuilder.Entity<Role>().HasData(
                    new Role() { Id = 1, Name = "admin" },
                    new Role() { Id = 2, Name = "staff" },
                    new Role() { Id = 3, Name = "customer" }
                );

            modelBuilder.Entity<Products>().HasData(
                    new Products() { Id = 1, ProductName = "Wood", Description = "Test product", Price = 123, Quantity = 1},
                    new Products() { Id = 2, ProductName = "Stone", Description = "Test product", Price = 123, Quantity = 123 },
                    new Products() { Id = 3, ProductName = "Wind", Description = "Test product", Price = 123, Quantity = 123 },
                    new Products() { Id = 4, ProductName = "Watter", Description = "Test product", Price = 123, Quantity = 123 },
                    new Products() { Id = 5, ProductName = "Steel", Description = "Test product", Price = 123, Quantity = 123 },
                    new Products() { Id = 6, ProductName = "Сoal", Description = "Test product", Price = 123, Quantity = 123 },
                    new Products() { Id = 7, ProductName = "Paper", Description = "Test product", Price = 123, Quantity = 123 }
                );
        }
    }
}
