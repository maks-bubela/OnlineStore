using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace OnlineStore.ExtensionMethods
{
    public static class RedisCacheExtension
    {
        private const string InstanceNameRedis = "local";
        public static void AddCustomRedisCache(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("Redis"); //
                options.InstanceName = InstanceNameRedis; 
            });
        }

        public static async Task SetStringWithExpirationAsync(this IDistributedCache cache, string key, object value, TimeSpan expiration)
        {
            var serializedValue = JsonSerializer.Serialize(value);
            await cache.SetStringAsync(key, serializedValue, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            });
        }
    }
}
