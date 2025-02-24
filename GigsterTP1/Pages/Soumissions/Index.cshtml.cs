using Microsoft.AspNetCore.Mvc.RazorPages;
using GigsterTP1.Modeles;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using GigsterTP1.Data;
using Microsoft.AspNetCore.Identity;

namespace GigsterTP1.Pages.Soumissions
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Utilisateur> _userManager;

        public IndexModel(ApplicationDbContext context, UserManager<Utilisateur> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Soumission> SoumissionsEnvoyees { get; set; }
        public IList<Soumission> SoumissionsRecues { get; set; }

        public async Task OnGetAsync()
        {
            var idutilisateurConnecte = await _userManager.GetUserAsync(User);

            SoumissionsEnvoyees = new List<Soumission>();
            SoumissionsRecues = new List<Soumission>();

            if (idutilisateurConnecte is null)
            {
                RedirectToPage("/Account/Login");
                return;
            }

            SoumissionsRecues = await _context.Soumissions.Include(s => s.Service).ThenInclude(s => s.Utilisateur)
                .Include(s => s.Utilisateur).Where(s => s.Service.UtilisateurId == idutilisateurConnecte.Id)
                .OrderByDescending(s => s.DateCreation).ToListAsync();

            SoumissionsEnvoyees = await _context.Soumissions.Include(s => s.Service).ThenInclude(s => s.Utilisateur)
                .Where(s => s.UtilisateurId == idutilisateurConnecte.Id).OrderByDescending(s => s.DateCreation)
                .ToListAsync();
        }
    }
}