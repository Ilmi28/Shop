using Azure.Security.KeyVault.Secrets;
using Azure.Identity;
using Azure.Core;
using Microsoft.Extensions.Azure;

namespace Shop.Services
{
    public class KeyVaultService
    {
        public string GetSecret(string secretName)
        {
            var client = new SecretClient(new Uri("https://testshop1keyvault.vault.azure.net/"), new DefaultAzureCredential());
            KeyVaultSecret secret = client.GetSecret(secretName);
            return secret.Value;
        }   
        
    }
}
