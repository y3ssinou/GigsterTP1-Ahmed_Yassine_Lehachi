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

        public DateTime Date { get; set; }

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _roleManager = roleManager;
        }


        public async Task OnGet()
        {

            Message = "Votre talent, votre liberté, vos opportunités.";


            if (!await _roleManager.Roles.AnyAsync())
            {
                await _roleManager.CreateAsync(new IdentityRole("Administrateur"));

                await _roleManager.CreateAsync(new IdentityRole("Professionel"));

                await _roleManager.CreateAsync(new IdentityRole("Utilisateur"));
            }
                
        }
    }
}
