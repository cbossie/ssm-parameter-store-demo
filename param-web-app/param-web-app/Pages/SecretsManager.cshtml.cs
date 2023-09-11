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

        private ILogger Logger { get; }


        public SecretsManagerModel(DemoConfiguration demoConfig, SecretsManagerCache smc, ILogger<SecretsManagerModel> logger)
        {
            SmCache = smc;
            DemoConfig = demoConfig;
            Logger = logger;
        }

        public async Task OnGet()
        {
            Logger.LogInformation("Opening Parameter Store Page");

            // Retrieve the two secret strings. If they are not there, then they will be retrieved from the
            // Secrets manager Service. Otherwise they will come from the cache.
            LaunchCode = await SmCache.GetSecretString($"launchcode-secret-{DemoConfig.Environment}");
            SecretPlanLocation = await SmCache.GetSecretString($"secret-plan-location-secret-{DemoConfig.Environment}");
        }
    }
}