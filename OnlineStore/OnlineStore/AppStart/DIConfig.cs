using Autofac;
using AutoMapper;
using OnlineStore.Autofac.Modules;
using OnlineStore.BLL.MappingProfiles;
using OnlineStore.Interfaces;
using OnlineStore.JwtConfig.Provider;
using OnlineStore.MappingProfiles;

namespace OnlineStore.AppStart
{
    public class DIConfig
    {
        public static ContainerBuilder Configure(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule<DALModule>();
            containerBuilder.RegisterModule<ServiceModule>();
            containerBuilder.RegisterType<AuthOptions>().As<IAuthOptions>();
            containerBuilder.Register(ctx => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new CustomerProfileBLL());
                cfg.AddProfile(new CustomerProfile());
                cfg.AddProfile(new ProductsProfileBLL());
                cfg.AddProfile(new ProductsProfile());
                cfg.AddProfile(new OrderProfile());
                cfg.AddProfile(new OrderProfileBLL());
            }).CreateMapper()).As<IMapper>().InstancePerLifetimeScope();
            return containerBuilder;
        }

    }
}
