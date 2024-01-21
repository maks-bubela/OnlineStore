using System.Reflection;
using Microsoft.EntityFrameworkCore;
using OnlineStore.DAL.Entities;
using OnlineStore.DAL.Seeder;

namespace OnlineStore.DAL.Context
{
    public class OnlineStoreContext : DbContext
    {
        public OnlineStoreContext(string connectionString) : base(GetOptions(connectionString)) { }
        public OnlineStoreContext(DbContextOptions<OnlineStoreContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<BearerTokenSetting> BearerTokenSettings { get; set; }
        public DbSet<EnvironmentType> EnvironmentTypes { get; set; }


        private static DbContextOptions<OnlineStoreContext> GetOptions(string connectionString)
        {
            return new DbContextOptionsBuilder<OnlineStoreContext>()
                .UseNpgsql(connectionString)  
                .Options;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            DatabaseSeeder.SeedDataBase(modelBuilder);
            base.OnModelCreating(modelBuilder);

        }
    }
}
