using Microsoft.Extensions.Configuration;

namespace OnlineStore.Configuration
{
    public class OnlineStoreConfiguration
    {
        public const string SqlConnectionStringSectionName = "ConnectionStrings:OnlineStoreDBPostgres";
        private const string StripeSecretKeyName = "Stripe:SecretKey";

        public string SqlConnectionString { get; private set; }
        public string StripeSecretKey { get; private set; }

        public OnlineStoreConfiguration(string sqlConnectionString, string stripeSecretKey)
        {
            if (string.IsNullOrWhiteSpace(nameof(sqlConnectionString)))
            {
                throw new ArgumentException(nameof(sqlConnectionString));
            }
            if (string.IsNullOrWhiteSpace(nameof(stripeSecretKey)))
            {
                throw new ArgumentException(nameof(stripeSecretKey));
            }

            SqlConnectionString = sqlConnectionString;
            StripeSecretKey = stripeSecretKey;
        }

        public static OnlineStoreConfiguration CreateFromConfigurations()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);

            IConfigurationRoot root = configurationBuilder.Build();

            return new OnlineStoreConfiguration(
                sqlConnectionString: root.GetSection(SqlConnectionStringSectionName).Value,
                stripeSecretKey: root.GetSection(StripeSecretKeyName).Value);
        }
    }
}
