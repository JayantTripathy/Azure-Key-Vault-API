using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure_Key_Vault_API.Interfaces;
using Azure_Key_Vault_API.Models;

namespace Azure_Key_Vault_API.ServiceVault
{
    public class KeyVaultSecretService : IKeyVaultSecretService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<KeyVaultSecretService> logger;

        public KeyVaultSecretService(IConfiguration configuration, ILogger<KeyVaultSecretService> logger)
        {
            _configuration = configuration;
            this.logger = logger;

        }
        public List<VaultSecret> GetSecrets()
        {
            var keyVaultSecrets = new List<VaultSecret>();
            try
            {

                var keyVaultEndpoint = _configuration["KeyVault:BaseUrl"];
                var clientId = _configuration["AzureAd:ClientId"];
                var clientSecret = _configuration["AzureAd:ClientSecret"];
                var tenantId = _configuration["AzureAd:TenantId"];

                var secretClient = new SecretClient(new Uri(keyVaultEndpoint), new ClientSecretCredential(tenantId, clientId, clientSecret));

                if (!string.IsNullOrEmpty(keyVaultEndpoint))
                {

                    // Get the root configuration section
                    var rootConfiguration = (IConfigurationRoot)_configuration;

                    // Create a dictionary to hold the secrets
                    var secrets = new Dictionary<string, string>();

                    // Get the keys of all the existing secrets
                    var secretProperties = secretClient.GetPropertiesOfSecrets();
                    foreach (var secretProperty in secretProperties)
                    {
                        var secretName = secretProperty.Name;
                        var secretValue = secretClient.GetSecret(secretName).Value.Value;

                        keyVaultSecrets.Add(new VaultSecret() { Name = secretName, Value = secretValue });
                    }
                }
                else
                {
                    logger.LogWarning("The KeyVault:BaseUrl configuration setting is missing or empty. No secrets were refreshed.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while refreshing the secrets from Key Vault.");
            }
            return keyVaultSecrets;
        }
    }
}