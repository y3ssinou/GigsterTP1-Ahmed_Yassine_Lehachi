using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GigsterTP1.Identity
{
    public class Utilisateur : IdentityUser
    {

        [Required(ErrorMessage = "Veuillez saisir un nom")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Veuillez saisir un prénom")]
        public string Prenom { get; set; }
    }
}
