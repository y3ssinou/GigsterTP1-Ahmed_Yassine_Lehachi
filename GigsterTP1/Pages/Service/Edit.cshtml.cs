using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GigsterTP1.Modeles;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GigsterTP1.Data;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace GigsterTP1.Pages.Service
{
    [Authorize(Roles = "Professionnel")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public List<SelectListItem> Categories { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Le nom est nécessaire")]
            [Display(Name = "Nom")]
            public string Nom { get; set; }

            [Required(ErrorMessage = "Le nom est nécessaire")]
            [Display(Name = "Catégorie")]
            public int CategorieId { get; set; }

            [Required(ErrorMessage = "Le nom est nécessaire")]
            [Display(Name = "Tarif")]
            public decimal Tarif { get; set; }

            [Required(ErrorMessage = "Le nom est nécessaire")]
            [Display(Name = "Description")]
            public string Description { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service is null)
            {
                return NotFound();
            }

            Categories = await _context.Categories
                .Select(c => new SelectListItem
                {
                    Text = c.Nom,
                    Value = c.Id.ToString()
                })
                .ToListAsync();

            Input = new InputModel
            {
                Nom = service.Nom,
                CategorieId = service.CategorieId,
                Tarif = service.Tarif,
                Description = service.Description
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                var service = await _context.Services.FindAsync(id);
                if (service == null)
                {
                    return NotFound();
                }

                service.Nom = Input.Nom;
                service.CategorieId = Input.CategorieId;
                service.Tarif = (int)Input.Tarif;
                service.Description = Input.Description;

                _context.Services.Update(service);
                await _context.SaveChangesAsync();

                return RedirectToPage("Index");
            }

            Categories = await _context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Nom
                })
                .ToListAsync();

            return Page();

        }
    }
}