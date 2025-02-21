using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GigsterTP1.Modeles
{
    public class Service
    {
        public int Id { get; set; }
        [Required]
        public string Nom { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Tarif { get; set; }
        [Required]
        public int CategorieId { get; set; }
        [ForeignKey("CategorieId")]
        public Categorie Categorie { get; set; }
        [Required]
        public string UtilisateurId { get; set; }
        public Utilisateur Utilisateur { get; set; }
        public double NoteMoyenne { get; set; }
        public int NbrVotes { get; set; }

        public bool EstSupprime { get; set; } = false;
        
    }
}
