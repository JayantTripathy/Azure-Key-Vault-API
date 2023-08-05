using Azure_Key_Vault_API.Models;

namespace Azure_Key_Vault_API.Interfaces
{
    public interface IKeyVaultSecretService
    {
        List<VaultSecret> GetSecrets();
    }
}
