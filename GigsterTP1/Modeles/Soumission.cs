using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GigsterTP1.Pages.Enums;

namespace GigsterTP1.Modeles
{
    public class Soumission
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public DateTime DateCreation { get; set; } = DateTime.Now;
        [Required]
        public DateTime DatePlanification { get; set; }
        public DateTime? DateTerminee { get; set; }
        [Required]
        public string Description { get; set; }
        public Etat Etat { get; set; } = Etat.EnAttente;
        [Required]
        public int ServiceId { get; set; }
        [ForeignKey("ServiceId")]
        public Service Service { get; set; }
        [Required]
        public string UtilisateurId { get; set; }
        [ForeignKey("UtilisateurId")]
        public Utilisateur Utilisateur { get; set; }
        [Required]
        public int? Note { get; set; }
    }

}
