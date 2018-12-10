using Microsoft.Extensions.Configuration;

namespace TheVilleSkill.Tests
{
    public class ConfigurationInitializer
    {
        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json", optional: false)
                .AddUserSecrets("ac961250-2711-4994-a443-6fe1b7bde4aa")
                .Build();

            return config;
        }
    }
}
