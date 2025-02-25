using Microsoft.AspNetCore.Mvc.RazorPages;
using GigsterTP1.Modeles;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using GigsterTP1.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace GigsterTP1.Pages.Service
{
    [Authorize(Roles = "Professionnel")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Utilisateur> _userManager;

        public IndexModel(ApplicationDbContext context, UserManager<Utilisateur> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<Modeles.Service> Services { get; set; }

        public async Task OnGetAsync()
        {
            var utilisateurConnecte = await _userManager.GetUserAsync(User);

            Services = await _context.Services
                .Include(s => s.Categorie)
                .Where(s => s.UtilisateurId == utilisateurConnecte.Id && !s.EstSupprime)
                .ToListAsync();
        }
    }
}