using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GigsterTP1.Modeles
{
    public class Utilisateur : IdentityUser
    {
        [Required (ErrorMessage = "Veuillez saisir votre nom")]
        public string Nom { get; set; }
        [Required(ErrorMessage = "Veuillez saisir votre prenom")]
        public string Prenom { get; set; }
        [Required(ErrorMessage = "Veuillez saisir votre email")]
        public string Adresse { get; set; }
        [Required(ErrorMessage = "Veuillez saisir votre code postal")]
        public string CodePostal { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string? Avatar { get; set; }
    }
}
