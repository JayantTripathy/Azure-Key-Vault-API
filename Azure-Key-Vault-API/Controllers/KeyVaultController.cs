using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure_Key_Vault_API.Interfaces;
using Azure_Key_Vault_API.ServiceVault;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Azure_Key_Vault_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KeyVaultController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IKeyVaultSecretService keyVaultSecretService;
        public KeyVaultController(IConfiguration configuration, IKeyVaultSecretService keyVaultSecretService)
        {
            _configuration = configuration;
            this.keyVaultSecretService = keyVaultSecretService;
        }
        [HttpGet]
        [Route("getsecrets")]
        public async Task<IActionResult> GetSecretConfigurations()
        {
            try
            {
                var response = keyVaultSecretService.GetSecrets();
                return Ok(response);
            }
            catch(Exception ex)
            {
                return BadRequest("Error: Unable to read secret");
            }
        }
    }
}
