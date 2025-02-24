using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GigsterTP1.Data;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using GigsterTP1.Modeles;

namespace GigsterTP1.Pages.Service
{
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
        public InputModel Input { get; set; }

        public List<SelectListItem> Categories { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Le nom est nécessaire")]
            [Display(Name = "Nom")]
            public string Nom { get; set; }

            [Required(ErrorMessage = "La catégorie est nécessaire")]
            [Display(Name = "Catégorie")]
            public int CategorieId { get; set; }

            [Required(ErrorMessage = "Le tarif est nécessaire")]
            [Display(Name = "Tarif")]
            public decimal Tarif { get; set; }

            [Required(ErrorMessage = "La description est nécessaire")]
            [Display(Name = "Description")]
            public string Description { get; set; }
        }

        public async Task OnGetAsync()
        {
            Categories = await _context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Nom
                })
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Categories = await _context.Categories
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Nom
                    })
                    .ToListAsync();
                return Page();
            }

            var utilisateurConnecte = await _userManager.GetUserAsync(User);

            var service = new Modeles.Service
            {
                Nom = Input.Nom,
                CategorieId = Input.CategorieId,
                Tarif = (int)Input.Tarif,
                Description = Input.Description,
                UtilisateurId = utilisateurConnecte.Id,
                EstSupprime = false
            };

            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}