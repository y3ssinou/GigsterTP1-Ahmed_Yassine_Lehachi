using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace GigsterTP1.Pages
{
    public class IndexModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public string Message { get; set; }

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _roleManager = roleManager;
        }


        public async Task OnGet()
        {

            Message = "Votre talent, votre liberté, vos opportunités.";
                
        }
    }
}
