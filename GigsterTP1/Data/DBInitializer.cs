using GigsterTP1.Enums;
using GigsterTP1.Modeles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuGet.LibraryModel;
using SQLitePCL;
using System.Data;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using GigsterTP1.Pages.Enums;

namespace GigsterTP1.Data
{
    public static class DBInitializer
    {
        private static UserManager<IdentityUser> _userManager;

        public static async Task Initialize(ApplicationDbContext context, UserManager<Utilisateur> userManager, RoleManager<IdentityRole> roleManager)
        {
            string[] roles = Enum.GetNames(typeof(Role));
            Random nbrAleatoire = new Random();

            foreach (string role in roles)
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }

            //Création des utilisateurs
            if (!await userManager.Users.AnyAsync())
            {
                // L'administrateur
                var compteadmin = new Utilisateur
                {
                    UserName = "Admin",
                    Email = "admin@factotum.com",
                    Nom = "Admin",
                    Prenom = "Admin",
                    Adresse = "123 Rue Admin",
                    CodePostal = "G1W 3B1",
                    Description = "Administrateur",
                    Avatar = "photopourseed.jpg"
                };

                await userManager.CreateAsync(compteadmin, "Password123!");
                await userManager.AddToRoleAsync(compteadmin, "Administrateur");


                // Les 3 comptes Utilisateurs

                var user1 = new Utilisateur
                {
                    UserName = "utilisateur1",
                    Email = "utilisateur1@gmail.com",
                    Nom = "utilisateur1",
                    Prenom = "utilisateur1",
                    Adresse = "123 Rue utilisateur",
                    CodePostal = "G1A 0A2",
                    Description = "L'utilisateur num 1",
                    Avatar = "photopourseed.jpg"
                };

                await userManager.CreateAsync(user1, "Password123!");
                await userManager.AddToRoleAsync(user1, "Utilisateur");

                var user2 = new Utilisateur
                {
                    UserName = "utilisateur2",
                    Email = "utilisateur2@gmail.com",
                    Nom = "utilisateur2",
                    Prenom = "utilisateur2",
                    Adresse = "123 Rue utilisateur",
                    CodePostal = "G1C 3K5",
                    Description = "L'utilisateur num 2",
                    Avatar = "photopourseed.jpg"
                };

                await userManager.CreateAsync(user2, "Password123!");
                await userManager.AddToRoleAsync(user2, "Utilisateur");

                var user3 = new Utilisateur
                {
                    UserName = "utilisateur3",
                    Email = "utilisateur3@gmail.com",
                    Nom = "utilisateur3",
                    Prenom = "utilisateur3",
                    Adresse = "123 Rue utilisateur",
                    CodePostal = "G1W 3B1",
                    Description = "L'utilisateur num 3",
                    Avatar = "photopourseed.jpg"
                };

                await userManager.CreateAsync(user3, "Password123!");
                await userManager.AddToRoleAsync(user3, "Utilisateur");


                // Les 3 comptes Professionels

                var prof1 = new Utilisateur
                {
                    UserName = "professionnel1",
                    Email = "professionel1@gmail.com",
                    Nom = "professionel1",
                    Prenom = "professionel1",
                    Adresse = "123 Rue utilisateur",
                    CodePostal = "G1G 4Y5",
                    Description = "Le professionel num 1",
                    Avatar = "photopourseed.jpg"
                };

                await userManager.CreateAsync(prof1, "Password123!");
                await userManager.AddToRoleAsync(prof1, "Professionnel");

                var prof2 = new Utilisateur
                {
                    UserName = "professionnel2",
                    Email = "professionel2@gmail.com",
                    Nom = "professionel2",
                    Prenom = "professionel2",
                    Adresse = "123 Rue professionel",
                    CodePostal = "G1H 2R3",
                    Description = "Le professionel num 2",
                    Avatar = "photopourseed.jpg"
                };

                await userManager.CreateAsync(prof2, "Password123!");
                await userManager.AddToRoleAsync(prof2, "Professionnel");

                var prof3 = new Utilisateur
                {
                    UserName = "professionnel3",
                    Email = "professionel3@gmail.com",
                    Nom = "professionel3",
                    Prenom = "professionel3",
                    Adresse = "123 Rue professionel",
                    CodePostal = "G1J 3S8",
                    Description = "Le professionel num 3",
                    Avatar = "photopourseed.jpg"
                };

                await userManager.CreateAsync(prof3, "Password123!");
                await userManager.AddToRoleAsync(prof3, "Professionnel");

            }


            //Création des catégories
            if (!context.Categories.Any())
            {

                context.Categories.AddRange(
                    new Categorie { Nom = "Informatique", EstSupprime = false, },
                    new Categorie { Nom = "Jeux vidéos", EstSupprime = false, },
                    new Categorie { Nom = "Basket-ball", EstSupprime = false, },
                    new Categorie { Nom = "Entretien ménager", EstSupprime = false, },
                    new Categorie { Nom = "Restauration", EstSupprime = false, },
                    new Categorie { Nom = "Santé", EstSupprime = false, }
                );

                await context.SaveChangesAsync();

            }

            var professionel = context.Users
                // Donc professionel 1 2 et 3
                .Where(p => p.UserName.Contains("professionnel"))
                .Select(p => p.Id)
                .ToList();

            var lescategories = context.Categories.Select(c => c.Id).ToList();


            if (!context.Services.Any())
            {
                var services = new List<Service>();
                for (int i = 0; i < 6; i++)
                {
                    services.Add(new Service
                    {
                        Nom = "Lorem ipsum dolor sit amet.",
                        Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. In non nunc urna. Phasellus dignissim tortor.",
                        Tarif = nbrAleatoire.Next(75),
                        // prend une catégorie au hasard
                        CategorieId = lescategories[nbrAleatoire.Next(lescategories.Count)],
                        UtilisateurId = professionel[0],
                        NoteMoyenne = nbrAleatoire.Next(5),
                        NbrVotes = nbrAleatoire.Next(200),
                        EstSupprime = false
                    });
                }
                context.Services.AddRange(services);
                await context.SaveChangesAsync();
            }

            var utilisateur = context.Users
                .Where(u => u.UserName.Contains("utilisateur"))
                .Select(u => u.Id)
                .ToList();

            var lesservices = context.Services.Select(s => s.Id).ToList();

            if (!context.Soumissions.Any())
            {
                var soumissions = new List<Soumission>();
                for (int i = 0; i < 10; i++)
                {
                    soumissions.Add(new Soumission
                    {
                        DateCreation = DateTime.Now,
                        DatePlanification = DateTime.Now,
                        Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. In non nunc urna. Phasellus dignissim tortor.",
                        DateTerminee = null,
                        Etat = Etat.EnAttente,
                        Note = nbrAleatoire.Next(100), // une note sur 100
                        // Prend un service au hasard et un utilisateur aussi
                        ServiceId = lesservices[nbrAleatoire.Next(lesservices.Count)],
                        UtilisateurId = utilisateur[nbrAleatoire.Next(utilisateur.Count)]
                    });
                }
                context.Soumissions.AddRange(soumissions);
                await context.SaveChangesAsync();
            }

        }
    }

}
