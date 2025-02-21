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
        policy.RequireRole("Administrateur")); // Seuls les utilisateurs avec le rôle "Administrateur" peuvent accéder
});

// Add services to the container.
builder.Services.AddRazorPages(options =>
{
    //options.Conventions.AuthorizeFolder("/Articles"); // Protège toutes les pages dans /Pages/Articles
    options.Conventions.AuthorizeAreaFolder("Admin", "/", "AdminOnly"); //Protège toutes les pages de /Areas/Admin
});


builder.Services.AddHttpClient();

builder.Services.AddRazorPages();

var app = builder.Build();


    app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();