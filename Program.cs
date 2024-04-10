using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BrickVault.Data;
using BrickVault.Models;
using Azure.Identity;

//using Microsoft.EntityFrameworkCore.Sqlite;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// services.AddAuthentication().AddGoogle(googleOptions =>
// {
//     googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
//     googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
// });

var conString = builder.Configuration["ConnectionString:brickvaultconnection"];

builder.Services.AddDbContext<IntexDbContext>(options =>
{
    options.UseSqlServer(conString);
});

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