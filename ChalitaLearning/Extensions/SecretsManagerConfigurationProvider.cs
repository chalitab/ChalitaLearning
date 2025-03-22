using System;
using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using ChalitaLearning.Configurations.Amazon;
using Newtonsoft.Json.Linq;

namespace ChalitaLearning.Extensions
{
    public class SecretsManagerConfigurationProvider : ConfigurationProvider
    {
        public override void Load()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configName = Environment.GetEnvironmentVariable("SecretManagerName");

            using var client = new AmazonSecretsManagerClient(RegionEndpoint.APSoutheast1);
            var secretId = environment + "/" + configName;
            var response = client.GetSecretValueAsync(new GetSecretValueRequest { SecretId = secretId })
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();

            Data = JsonConfigurationParser.Instance.ParseObject(JObject.Parse(response.SecretString));
        }
    }
}
