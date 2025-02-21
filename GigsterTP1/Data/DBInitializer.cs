using GigsterTP1.Data;
using GigsterTP1.Modeles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuGet.LibraryModel;
using SQLitePCL;
using System.Data;

namespace GigsterTP1.Data
{
    public static class DBInitializer
    {
        private static UserManager<IdentityUser> _userManager;

        public static async Task Initialize(ApplicationDbContext context, UserManager<Utilisateur> userManager, RoleManager<IdentityRole> roleManager)
        {

            //Création des rôles
            if (!await roleManager.Roles.AnyAsync())
            {
                await roleManager.CreateAsync(new IdentityRole("Administrateur"));
                await roleManager.CreateAsync(new IdentityRole("Utilisateur"));
                await roleManager.CreateAsync(new IdentityRole("Professionel"));
            }

            //Création des utilisateurs
            if (!await userManager.Users.AnyAsync())
            {

                //Création de l'administrateur
                var user3 = new Utilisateur { UserName = "Admin", Email = "admin@factotum.com", Prenom = "Admin", Nom = "Admin",  };
                await userManager.CreateAsync(user3, "Password123!");
                await userManager.AddToRoleAsync(user3, "Administrateur");

                //Création d'un compte profesionnel
                var user1 = new Utilisateur { UserName = "yassine1", Email = "yassine1@factotum.com", Prenom = "Yassine1", Nom = "Lehachi1" };
                await userManager.CreateAsync(user1, "Yassine12@");
                await userManager.AddToRoleAsync(user1, "Professionel");
                    
                //Création d'un compte utilisateur
                var user2 = new Utilisateur { UserName = "yassine2", Email = "yassine2@factotum.com", Prenom = "Yassine2", Nom = "Lehachi2" };
                await userManager.CreateAsync(user2, "Yassine13@");
                await userManager.AddToRoleAsync(user2, "Utilisateur");

            }

            // ...



        }

    }

}
