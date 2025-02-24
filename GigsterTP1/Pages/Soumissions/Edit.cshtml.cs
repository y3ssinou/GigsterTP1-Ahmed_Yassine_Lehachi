using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GigsterTP1.Modeles;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GigsterTP1.Data;
using System.ComponentModel.DataAnnotations;

namespace GigsterTP1.Pages.Soumissions
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Soumission Soumission { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "La date de planification est obligatoire.")]
            [Display(Name = "Date de planification")]
            public DateTime DatePlanification { get; set; }

            [Required(ErrorMessage = "La description est obligatoire.")]
            [Display(Name = "Description")]
            public string Description { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Soumission = await _context.Soumissions
                .Include(s => s.Service)
                    .ThenInclude(s => s.Utilisateur)
                .Include(s => s.Utilisateur)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (Soumission == null)
            {
                return NotFound();
            }

            Input = new InputModel
            {
                DatePlanification = Soumission.DatePlanification,
                Description = Soumission.Description
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var soumission = await _context.Soumissions.FindAsync(id);

            if (soumission == null)
            {
                NotFound();
            }

            soumission.DatePlanification = Input.DatePlanification;
            soumission.Description = Input.Description;

            _context.Soumissions.Update(soumission);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Soumissions/Index");
        }
    }
}