using Amazon.SecretsManager;
using Amazon.SecretsManager.Extensions.Caching;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using param_web_app.Configuration;

namespace param_web_app.Pages
{
    public class SecretsManagerModel : PageModel
    {
        private SecretsManagerCache SmCache { get; }

        public DemoConfiguration DemoConfig { get; set; }
        public string SecretPlanLocation { get; set; }
        public string LaunchCode { get; set; }


        public SecretsManagerModel(DemoConfiguration demoConfig, SecretsManagerCache smc)
        {
            SmCache = smc;
            DemoConfig = demoConfig;
        }

        public async Task OnGet()
        {
            // Retrieve the two secret strings. If they are not there, then they will be retrieved from the
            // Secrets manager Service. Otherwise they will come from the cache.
            LaunchCode = await SmCache.GetSecretString($"launchcode-secret-{DemoConfig.Environment}");
            SecretPlanLocation = await SmCache.GetSecretString($"secret-plan-location-secret-{DemoConfig.Environment}");
        }
    }
}