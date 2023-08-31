using Amazon.SecretsManager;
using Amazon.SecretsManager.Extensions.Caching;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using param_web_app.Configuration;

namespace param_web_app.Pages
{
    public class IndexModel : PageModel
    {
        public IndexModel()
        {
        }

        public async Task OnGet()
        {         
        }
    }
}