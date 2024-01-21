using Autofac;
using OnlineStore.Configuration;
using OnlineStore.DAL.Context;
using OnlineStore.DAL.Interfaces;
using OnlineStore.DAL.Repository;

namespace OnlineStore.Autofac.Modules
{
    public class DALModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var connectionString = OnlineStoreConfiguration.CreateFromConfigurations().SqlConnectionString;

            builder.Register(ctx => new OnlineStoreContext(connectionString)).AsSelf();
            builder.RegisterType<GenericRepository>().As<IGenericRepository>();

            base.Load(builder);
        }
    }
}
