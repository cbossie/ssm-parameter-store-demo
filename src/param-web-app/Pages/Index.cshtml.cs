using Amazon.SecretsManager;
using Amazon.SecretsManager.Extensions.Caching;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace param_web_app.Pages
{
    public class IndexModel : PageModel
    {
        public SsmModel SsmData{ get; set; } = new SsmModel();

        private SecretsManagerCache _smCache;
        public string? SecretValue { get; set; }

        public IndexModel(IConfiguration config, SecretsManagerCache smc)
        {
            // Load up the configuration from Systems Manager
            config.Bind(SsmData);
            _smCache = smc;
        }



        public async Task OnGet()
        {
            SecretValue = (await _smCache.GetCachedSecret("demo-test-secret").GetSecretValue()).SecretString;
        }
    }
}