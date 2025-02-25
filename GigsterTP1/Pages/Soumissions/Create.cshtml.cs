using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GigsterTP1.Modeles;
using Microsoft.EntityFrameworkCore;
using GigsterTP1.Data;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using GigsterTP1.Enums;
using GigsterTP1.Pages.Enums;
using Microsoft.AspNetCore.Authorization;

namespace GigsterTP1.Pages.Soumissions
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Utilisateur> _userManager;

        public CreateModel(ApplicationDbContext context, UserManager<Utilisateur> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Modeles.Service Service { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        [BindProperty]
        public Soumission Soumission { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "La date de planification est nécessaire")]
            [Display(Name = "Date de planification")]
            public DateTime DatePlanification { get; set; }

            [Required(ErrorMessage = "La description est nécessaire")]
            [Display(Name = "Description")]
            public string Description { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(int serviceId)
        {
            Service = await _context.Services
                .Include(s => s.Utilisateur)
                .FirstOrDefaultAsync(s => s.Id == serviceId);

            if (Service is null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int serviceId)
        {
            if (!ModelState.IsValid)
            {
                Service = await _context.Services
                    .Include(s => s.Utilisateur)
                    .FirstOrDefaultAsync(s => s.Id == serviceId);

                if (Service is null)
                {
                    return NotFound();
                }

                return Page();
            }

            // Récupérez l'utilisateur connecté
            var utilisateurConnecte = await _userManager.GetUserAsync(User);

            if (utilisateurConnecte is null)
            {
                return RedirectToPage("/Account/Login");
            }

            var soumission = new Soumission
            {
                ServiceId = serviceId,
                UtilisateurId = utilisateurConnecte.Id,
                DateCreation = DateTime.Now,
                DatePlanification = Input.DatePlanification,
                Description = Input.Description,
                Etat = Etat.EnAttente
            };

            _context.Soumissions.Add(soumission);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Soumissions/Index");
        }
    }
}