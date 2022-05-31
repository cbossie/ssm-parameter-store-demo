using Amazon.SecretsManager;
using Amazon.SecretsManager.Extensions.Caching;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace param_web_app.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public SsmModel Ssm { get; init; }
        public IAmazonSecretsManager Sm { get; init; }
        public string SecretValue { get; set; }

        public IndexModel(SsmModel ssm, ILogger<IndexModel> logger, IAmazonSecretsManager sm)
        {
            _logger = logger;
            Ssm = ssm;
            Sm = sm;
        }



        public async Task OnGet()
        {
            var val = await Sm.GetSecretValueAsync(new Amazon.SecretsManager.Model.GetSecretValueRequest
            {
                SecretId = "demo-test-secret"
            });

            SecretValue = val.SecretString;

        }
    }
}