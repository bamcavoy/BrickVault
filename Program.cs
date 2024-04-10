using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BrickVault.Data; // Ensure this namespace correctly points to where your ApplicationDbContext and IntexDbContext are defined.
using BrickVault.Models; // If you have specific models here being used, ensure they are correctly defined.
// Note: No need to import Microsoft.EntityFrameworkCore.Sqlite or Azure specific namespaces unless specifically used in this file.

var builder = WebApplication.CreateBuilder(args);

// Setup connection strings
var defaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var azureConnectionString = builder.Configuration.GetConnectionString("AzureConnection");

// Register DbContexts with the respective connection strings
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(defaultConnectionString));

builder.Services.AddDbContext<IntexDbContext>(options =>
    options.UseSqlServer(azureConnectionString));

// Setup Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>(); // Make sure you are pointing to the correct DbContext that contains the Identity tables

// If you are using Google Authentication, ensure your Google ClientId and ClientSecret are correctly set up in your configuration (appsettings.json or user secrets)
builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
});

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ILegoRepository, EFLegoRepository>(); // Registering ILegoRepository

var app = builder.Build();

// Middleware configuration
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication(); // Ensure authentication is called before authorization
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
