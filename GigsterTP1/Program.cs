using GigsterTP1.Data;
using GigsterTP1.Modeles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("La BD est non fonctionnel");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));


builder.Services.AddIdentity<Utilisateur, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireUppercase = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Administrateur")); // Seuls les utilisateurs avec le r�le "Administrateur" peuvent acc�der
});

// Add services to the container.
builder.Services.AddRazorPages(options =>
{
    //options.Conventions.AuthorizeFolder("/Articles"); // Prot�ge toutes les pages dans /Pages/Articles
    options.Conventions.AuthorizeAreaFolder("Admin", "/", "AdminOnly"); //Prot�ge toutes les pages de /Areas/Admin
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login"; // Redirige vers /Utilisateur/Connexion au lieu de /Account/Login
    options.AccessDeniedPath = "/Identity/Account/AccessDenied"; // Page d'erreur si acc�s refus�
});


builder.Services.AddHttpClient();

builder.Services.AddRazorPages();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseStatusCodePagesWithReExecute("/Error/{0}");
}

app.UseExceptionHandler("/Error");
app.UseStatusCodePagesWithReExecute("/Error/{0}");

//Cr�ation des seeds au d�marrage de l'application 
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    //On obtient les services n�cessaires  
    var context = services.GetRequiredService<ApplicationDbContext>();
    var userManager = services.GetRequiredService<UserManager<Utilisateur>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    await DBInitializer.Initialize(context, userManager, roleManager);

}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();