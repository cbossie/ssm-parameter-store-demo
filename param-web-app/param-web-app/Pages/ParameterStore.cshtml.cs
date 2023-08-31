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


        public ParameterStoreModel(IConfiguration config, SecretsManagerCache smc)
        {
            // Load up the configuration from Systems Manager
            config.Bind(SsmData);
        }

        public async Task OnGet()
        {
            
        }
    }
}