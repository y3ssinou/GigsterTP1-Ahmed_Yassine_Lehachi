using GigsterTP1.Modeles;
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

            [Required(ErrorMessage = "L'adresse est obligatoire.")]
            [Display(Name = "Adresse")]
            public string Adresse { get; set; }

            [Required(ErrorMessage = "Le code postal est obligatoire.")]
            [Display(Name = "Code postal")]
            public string CodePostal { get; set; }

            [Required(ErrorMessage = "Une description est obligatoire.")]
            [Display(Name = "Description")]
            public string Description { get; set; }

            [Required(ErrorMessage = "L'avatar est obligatoire.")]
            [Display(Name = "Avatar")]
            public IFormFile Avatar { get; set; }
        }

        public void OnGet() 
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {

            string avatarPath = null;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Input.Avatar != null)
            {
                // Chemin de base pour les uploads
                // Équivalent au _configuration["Images:UploadPath"]; 
                var cheminUpload = Path.Combine("wwwroot", "uploads", "avatars");

                // Créer le dossier s'il n'existe pas
                if (!Directory.Exists(cheminUpload))
                {
                    Directory.CreateDirectory(cheminUpload);
                }

                // Générer un nom de fichier unique
                // Enregistrer le fichier sur le disque

                var nomImageUnique = Guid.NewGuid().ToString() + "_" + Input.Avatar.FileName;
                var chemin = Path.Combine(cheminUpload, nomImageUnique);

                using (var fileStream = new FileStream(chemin, FileMode.Create))
                {
                    await Input.Avatar.CopyToAsync(fileStream);
                }

                // Enregistrer le chemin relatif dans la base de données
                avatarPath = Path.Combine("uploads", "avatars", nomImageUnique);
            }

            // Créer l'utilisateur
            var user = new Utilisateur
            {
                UserName = Input.UserName,
                Email = Input.Email,
                Nom = Input.Nom,
                Prenom = Input.Prenom,
                Adresse = Input.Adresse,
                CodePostal = Input.CodePostal,
                Description = Input.Description,
                Avatar = avatarPath
            };

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
