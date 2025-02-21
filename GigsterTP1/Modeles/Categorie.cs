using System.ComponentModel.DataAnnotations;

namespace GigsterTP1.Modeles
{
    public class Categorie
    {
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage = "Le nom est obligatoire")]
        public string Nom { get; set; }
        public bool EstSupprime { get; set; } = false;
        // J'utilise une interface générique pour les services
        public List<Service> LesServices { get; set; }
    }
}
