using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GigsterTP1.Data;
using GigsterTP1.Modeles;
using System.Net.Http;
using System.Text.Json;
using System;

namespace GigsterTP1.Pages
{
    public class IndexModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<IndexModel> _logger;
        private readonly HttpClient _httpClient;

        public string Message { get; set; }
        public string MessageAPI { get; set; }
        public List<Modeles.Service> Services { get; set; }
        public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
        [BindProperty]
        public string lescategories { get; set; }
        [BindProperty]
        public string codezip { get; set; }
        // utilisation d'une tuple et qui contient la distance en première position et le service en deuxième
        [BindProperty]
        public List<(double, Modeles.Service)> apiDonnees { get; set; }

        public IndexModel(ILogger<IndexModel> logger, RoleManager<IdentityRole> roleManager, ApplicationDbContext context, HttpClient httpClient)
        {
            _logger = logger;
            _roleManager = roleManager;
            _context = context;
            _httpClient = httpClient;
        }

        public async Task OnGet()
        {

            Message = "Votre talent, votre liberté, vos opportunités";

            var categorieNoms = _context.Categories.Select(c => c.Nom).ToList();
            Categories = categorieNoms
                .Select(categorie => new SelectListItem { Value = categorie, Text = categorie })
                .ToList();

        }

        public async Task OnPost()
        {
            Message = "Votre talent, votre liberté, vos opportunités";

            var categorieIds = _context.Categories
                .Where(c => c.Nom == lescategories)
                .Select(c => c.Id)
                .ToList();

            if (categorieIds.Any())
            {
                Services = _context.Services
                    .Where(s => s.CategorieId == categorieIds.First())
                    .Include(s => s.Utilisateur)
                    .ToList();
            }

            if (Services.Any())
            {
                foreach (var service in Services)
                {
                    string apiUrl = $"https://www.zipcodeapi.com/rest/v2/CA/DemoOnly00x4jy9av3hHEeW16zbE2PocYMnRGpREt9k9VCSc1MaujIMBc2U3b3lY/distance.json/{codezip}/{service.Utilisateur.CodePostal}/km";

                    var jsonString = await _httpClient.GetStringAsync(apiUrl);
                    using JsonDocument doc = JsonDocument.Parse(jsonString);
                    double distance = doc.RootElement.GetProperty("distance").GetDouble();

                    apiDonnees.Add((distance, service));
                }
            }
            else
            {
                MessageAPI = "Aucun service";
            }

            MessageAPI = $"Nombre de résultats : {apiDonnees.Count}";

            var categorieNoms = _context.Categories.Select(c => c.Nom).ToList();
            Categories = categorieNoms
                .Select(categorie => new SelectListItem { Value = categorie, Text = categorie })
                .ToList();
        }

    }
}
