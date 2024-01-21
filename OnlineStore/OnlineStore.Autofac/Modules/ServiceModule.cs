using Autofac;
using OnlineStore.BLL.Cryptography;
using OnlineStore.BLL.Interfaces;
using OnlineStore.BLL.Services;
using OnlineStore.Services;


namespace OnlineStore.Autofac.Modules
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterType<AccountService>().As<IAccountService>();
            builder.RegisterType<ProductsService>().As<IProductsService>();
            builder.RegisterType<StripeService>().As<IStripeService>();

            builder.RegisterType<CustomerService>().As<ICustomerService>();
            builder.RegisterType<OrderService>().As<IOrderService>();
            builder.RegisterType<TokenService>().As<ITokenService>();
            builder.RegisterType<PasswordProcessing>().As<IPasswordProcessing>();

            base.Load(builder);
        }
    }
}
