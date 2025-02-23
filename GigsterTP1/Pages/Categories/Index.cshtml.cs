using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GigsterTP1.Modeles;
using System.Collections.Generic;
using System.Threading.Tasks;
using GigsterTP1.Data;

namespace GigsterTP1.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Categorie> Categories { get; set; }

        public async Task OnGetAsync()
        {
            Categories = await _context.Categories.ToListAsync();
        }
    }
}