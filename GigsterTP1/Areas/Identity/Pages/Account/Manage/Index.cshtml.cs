// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using GigsterTP1.Modeles;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GigsterTP1.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<Utilisateur> _userManager;
        private readonly SignInManager<Utilisateur> _signInManager;
        private readonly IWebHostEnvironment _environment;

        public IndexModel(
            UserManager<Utilisateur> userManager,
            SignInManager<Utilisateur> signInManager,
            IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _environment = environment;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Nom d'utilisateur")]
            public string Username { get; set; }

            [Required(ErrorMessage = "Le nom est nécessaire")]
            [Display(Name = "Nom")]
            public string Nom { get; set; }

            [Required(ErrorMessage = "Le prénom est nécessaire")]
            [Display(Name = "Prénom")]
            public string Prenom { get; set; }

            [EmailAddress]
            [Display(Name = "Courriel")]
            public string Email { get; set; }

            [Required(ErrorMessage = "L'email est nécessaire")]
            [Display(Name = "Adresse")]
            public string Adresse { get; set; }

            [Required(ErrorMessage = "Le code postal est nécessaire")]
            [Display(Name = "Code postal")]
            public string CodePostal { get; set; }

            [Display(Name = "Description")]
            public string Description { get; set; }

            [Display(Name = "Avatar")]
            public IFormFile Avatar { get; set; }

            [Display(Name = "Offrir des services ?")]
            public bool OffrirService { get; set; }
        }

        private async Task LoadAsync(Utilisateur user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var unprofessionnel = await _userManager.IsInRoleAsync(user, "Professionnel");

            Username = userName;

            Input = new InputModel
            {
                Username = userName,
                Nom = user.Nom,
                Prenom = user.Prenom,
                Email = email,
                Adresse = user.Adresse,
                CodePostal = user.CodePostal,
                Description = user.Description,
                OffrirService = unprofessionnel
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Pas capable de Charger :'{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Pas capable de Charger : '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            // Mise à jour des champs de l'utilisateur
            if (Input.Nom != user.Nom)
            {
                user.Nom = Input.Nom;
            }

            if (Input.Prenom != user.Prenom)
            {
                user.Prenom = Input.Prenom;
            }

            if (Input.Adresse != user.Adresse)
            {
                user.Adresse = Input.Adresse;
            }

            if (Input.CodePostal != user.CodePostal)
            {
                user.CodePostal = Input.CodePostal;
            }

            if (Input.Description != user.Description)
            {
                user.Description = Input.Description;
            }

            // pris de inscription
            if (Input.Avatar is not null)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "avatars");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Input.Avatar.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Input.Avatar.CopyToAsync(fileStream);
                }

                user.Avatar = uniqueFileName;
            }

            var estProf = await _userManager.IsInRoleAsync(user, "Professionnel");

            if (Input.OffrirService && !estProf)
            {
                await _userManager.AddToRoleAsync(user, "Professionnel");
            }
            else if (!Input.OffrirService && estProf)
            {
                await _userManager.RemoveFromRoleAsync(user, "Professionnel");
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                StatusMessage = "Erreur pour la modification";
                return RedirectToPage();
            }

            await _signInManager.RefreshSignInAsync(user);

            StatusMessage = "Votre profil a été mis à jour avec succès !";
            return RedirectToPage();
        }
    }
}