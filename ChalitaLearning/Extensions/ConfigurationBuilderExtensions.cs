namespace ChalitaLearning.Extensions
{
    using System.IO;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    public static class StartUpConfiguration
    {
        public static void AddConfigurations(this WebApplicationBuilder host)
        {
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
#if DEBUG
                .SetBasePath(Directory.GetCurrentDirectory())
                .Add(new SecretsManagerConfigurationSource())
                // .AddJsonFile("appsettings.json", optional: true)
#else
            .Add(new SecretsManagerConfigurationSource())
#endif
                .Build();

            host.Configuration.AddConfiguration(configuration);
        }
    }
}
