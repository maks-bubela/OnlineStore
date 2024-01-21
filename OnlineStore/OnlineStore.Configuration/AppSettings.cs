using Microsoft.Extensions.Configuration;

namespace OnlineStore.Configuration
{
    public class AppSettings
    {
        private const string AppSettingsPrefix = "AppSettings:";
        public string SettingsValue { get; private set; }

        public AppSettings(string value)
        {
            if (string.IsNullOrWhiteSpace(nameof(value)))
            {
                throw new ArgumentException(nameof(value));
            }
            SettingsValue = value;
        }
        public static AppSettings CreateFromConfigurations(string sectionName)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var config = builder.Build();

            return new AppSettings(
                value: config[AppSettingsPrefix + sectionName]);
        }
    }
}
