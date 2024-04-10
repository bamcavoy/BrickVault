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

builder.Services.AddDbContext<IntexDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration["ConnectionStrings:AzureConnection"]);
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

app.MapControllerRoute(
    name: "paged",
    pattern: "{controller=Home}/{action=Index}/page{pageNum:int}"
);
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.MapRazorPages();

app.Run();