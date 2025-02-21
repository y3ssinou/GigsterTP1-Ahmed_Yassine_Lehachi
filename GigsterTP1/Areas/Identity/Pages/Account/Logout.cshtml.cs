using GigsterTP1.Modeles;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GigsterTP1.Pages.Account
{
    public class LogoutModel : PageModel
    {

        private readonly SignInManager<Utilisateur> _signInManager;

        public LogoutModel(SignInManager<Utilisateur> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _signInManager.SignOutAsync();
            return RedirectToPage("/Index");
        }
    }
}
