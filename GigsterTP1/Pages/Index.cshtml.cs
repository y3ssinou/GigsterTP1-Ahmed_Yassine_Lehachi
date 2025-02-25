using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GigsterTP1.Data;
using GigsterTP1.Modeles;

namespace GigsterTP1.Pages
{
    public class IndexModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly ApplicationDbContext _context;

        public string Message { get; set; }

        private readonly ILogger<IndexModel> _logger;

        [BindProperty]
        public string Categoriee { get; set; }

        [BindProperty]
        public List<SelectListItem> Categorie { get; set; }

        // API :
                


        public IndexModel(ILogger<IndexModel> logger, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _logger = logger;
            _roleManager = roleManager;
            _context = context;
        }


        public async Task OnGet()
        {
            var categories = await _context.Categories.Select(c => c.Nom).ToListAsync();

            Categorie = categories.Select(c => new SelectListItem { Value = c, Text = c }).ToList();

            Message = "Votre talent, votre liberté, vos opportunités.";
                
        }
    }
}
