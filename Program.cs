using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BrickVault.Data;
using BrickVault.Models;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

//using Microsoft.EntityFrameworkCore.Sqlite;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

var myAppConn= builder.Configuration.GetConnectionString("Constr");

// services.AddAuthentication().AddGoogle(googleOptions =>
// {
//     googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
//     googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
// });

var keyVault =
    "@Microsoft.KeyVault(SecretUri=https://intexvault2-15.vault.azure.net/secrets/IntexAzureConnectionString/6d069c01e09e4776916eb1e5ea3850ce)";

builder.Services.AddDbContext<IntexDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration[keyVault]);
    }
);

builder.Services.AddScoped<ILegoRepository, EFLegoRepository>();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<IntexDbContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();


app.MapControllerRoute("paged", "Products/{pageNum}",
    new { Controller = "Home", action = "Products"});
app.MapControllerRoute("pagedwithitems", "Products/{pageNum}/{itemsPerPage}items",
    new { Controller = "Home", action = "Products"});
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.MapRazorPages();

app.Run();