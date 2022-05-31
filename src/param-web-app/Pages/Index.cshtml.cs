using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace param_web_app.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public SsmModel Ssm { get; init; }
        public IndexModel(SsmModel ssm, ILogger<IndexModel> logger)
        {
            _logger = logger;
            Ssm = ssm;
        }



        public void OnGet()
        {

        }
    }
}