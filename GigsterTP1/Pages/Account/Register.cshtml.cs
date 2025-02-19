using GigsterTP1.Identity;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace GigsterTP1.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<Utilisateur> _userManager;
        private readonly SignInManager<Utilisateur> _signInManager;


        public RegisterModel(UserManager<Utilisateur> userManager, SignInManager<Utilisateur> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public class InputModel
        {
            [Required(ErrorMessage = "Le nom d'utilisateur est obligatoire.")]
            [StringLength(50, ErrorMessage = "Le nom d'utilisateur ne peut pas dépasser 50 caractères.")]
            [Display(Name = "Nom d'utilisateur")]
            public string UserName { get; set; }

            [Required(ErrorMessage = "Le prénom est obligatoire.")]
            public string Prenom { get; set; }

            [Required(ErrorMessage = "Le nom est obligatoire.")]
            public string Nom { get; set; }

            [Required(ErrorMessage = "Le courriel est obligatoire.")]
            [EmailAddress(ErrorMessage = "Veuillez saisir un courriel valide")]
            [Display(Name = "Courriel")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Le mot de passe est obligatoire.")]
            [DataType(DataType.Password)]
            [StringLength(100, MinimumLength = 6, ErrorMessage = "Le mot de passe doit contenir au moins 6 caractères.")]
            [Display(Name = "Mot de passe")]
            public string Password { get; set; }

            [Required(ErrorMessage = "Veuillez confirmer votre mot de passe.")]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Les mots de passe ne correspondent pas.")]
            [Display(Name = "Confirmer le mot de passe")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet() 
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Création d'un nouvel utilisateur avec le nom d'utilisateur saisi
            //var user = new IdentityUser { UserName = Input.UserName, Email = Input.Email };

            var user = new Utilisateur { UserName = Input.UserName, Email = Input.Email, Nom = Input.Prenom, Prenom = Input.Prenom };

            // Création de l'utilisateur avec Identity
            var result = await _userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Utilisateur");
                // Connexion automatique après l'inscription
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToPage("/Index");
            }

            // Ajout des erreurs d'inscription à ModelState
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }
    }
}
