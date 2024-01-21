using Microsoft.OpenApi.Models;
using OnlineStore.DAL.Context;
using OnlineStore.Filters;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;

namespace OnlineStore.ExtensionMethods
{
    public static class SwaggerExtension
    {
        private const string TokenDescription = "Please insert JWT with Bearer into field";
        private const string OperationName = "Authorization";
        private const string TokenApiKey = "Bearer";
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = nameof(OnlineStoreContext), Version = "v1", });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.AddSecurityDefinition(TokenApiKey, new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = TokenDescription,
                    Name = OperationName,
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = TokenApiKey
                            }
                        },
                        new string[] { }
                    }
                });
            });
        }

        public static void UseCustomSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.DocExpansion(DocExpansion.List);
            });
        }
    }
}
