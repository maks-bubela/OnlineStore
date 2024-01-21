using Autofac;
using OnlineStore.Configuration;
using OnlineStore.DAL.Context;
using OnlineStore.ExtensionMethods;
using Stripe;
using OnlineStore.Filters;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace OnlineStore.AppStart
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; set; }
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwagger();
            services.AddControllers().AddJsonOptions(options =>
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
            services.AddJwtToken(Configuration);
            services.AddOptions();
            services.AddMvc(options => options.Filters.Add(new ExceptionFilter()));
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            DIConfig.Configure(builder);

        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<OnlineStoreContext>();
                context.Database.EnsureCreated();
            }
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCustomSwagger();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();

                endpoints.MapControllerRoute(
                    name: "swagger",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );
            });
        }
    }
}
