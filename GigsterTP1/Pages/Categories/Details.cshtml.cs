using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GigsterTP1.Modeles;
using System.Linq;
using System.Threading.Tasks;
using GigsterTP1.Data;
using Microsoft.AspNetCore.Mvc;

namespace GigsterTP1.Pages.Categories
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Categorie Categorie { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            Categorie = await _context.Categories
                .Include(c => c.LesServices)
                .ThenInclude(s => s.Utilisateur)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (Categorie is null)
                return NotFound();

            return Page();
        }
    }
}