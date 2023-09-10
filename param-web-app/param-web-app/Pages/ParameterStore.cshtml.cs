using Amazon.SecretsManager;
using Amazon.SecretsManager.Extensions.Caching;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using param_web_app.Configuration;

namespace param_web_app.Pages
{
    public class ParameterStoreModel : PageModel
    {
        public SsmModel SsmData{ get; set; } = new SsmModel();
        public DemoConfiguration DemoConfig { get; set; }
        private ILogger Logger { get; }
        public ParameterStoreModel(IConfiguration config, DemoConfiguration demoConfig, ILogger<ParameterStoreModel> logger)
        {
            // Load up the configuration from Systems Manager
            config.Bind(SsmData);
            DemoConfig = demoConfig;
            Logger = logger;
        }

        public async Task OnGet()
        {
            Logger.LogInformation("Opening Parameter Store Page");
        }
    }
}