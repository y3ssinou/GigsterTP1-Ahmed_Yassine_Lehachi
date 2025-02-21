using GigsterTP1.Modeles;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GigsterTP1.Pages.Account
{
    public class LoginModel : PageModel
    {

        private readonly SignInManager<Utilisateur> _signInManager;

        public LoginModel(SignInManager<Utilisateur> signInManager)
        {
            _signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

       

        public class InputModel
        {
            [Required(ErrorMessage = "Le nom d'utilisateur est requis.")]
            [DisplayName("Nom d'utilisateur")]
            public string UserName { get; set; }

            [Required(ErrorMessage = "Le mot de passe est requis.")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            public bool RememberMe { get; set; }
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _signInManager.PasswordSignInAsync(
                Input.UserName, Input.Password, Input.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                if(returnUrl is not null)
                    return LocalRedirect(returnUrl); 
                else
                    return RedirectToPage("/Index"); // Redirige vers la page d'accueil
            }
            else
            {
                ModelState.AddModelError("","Courriel ou mot de passe incorrect.");
            }

            return Page();
        }
    }
}
