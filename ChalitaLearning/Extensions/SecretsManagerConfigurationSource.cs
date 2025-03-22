namespace ChalitaLearning.Extensions
{
    using Microsoft.Extensions.Configuration;
    public class SecretsManagerConfigurationSource : IConfigurationSource
    {
        public IConfigurationProvider Build(IConfigurationBuilder builder) 
        {
            return new SecretsManagerConfigurationProvider();
        }
    }
}
