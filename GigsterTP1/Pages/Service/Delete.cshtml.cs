using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GigsterTP1.Data;
using Microsoft.EntityFrameworkCore;

namespace GigsterTP1.Pages.Service
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Modeles.Service Service { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Service = await _context.Services
                .Include(s => s.Categorie)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (Service == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var service = await _context.Services.FindAsync(id);

            if (service is null)
            {
                return NotFound();
            }

            service.EstSupprime = true;
            _context.Services.Update(service);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}