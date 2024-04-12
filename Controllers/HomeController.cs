using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BrickVault.Models;
using BrickVault.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BrickVault.Controllers;

public class HomeController : Controller
{
    private IConfiguration _configuration;
    private ILegoRepository _repo;
    // private readonly SignInManager<AspNetUser> _signInManager;
    public HomeController(IConfiguration configuration, ILegoRepository tmp)
    {
        _configuration = configuration;
        _repo = tmp;
        // _signInManager = signInManager;
    }
    
    public IActionResult Index()
    {
        List<int> topItems = [27, 33, 34, 37, 24];

        if (User.IsInRole("Customer"))
        {
            topItems = [9, 25, 17, 16, 37];
        }

        ViewBag.Products = _repo.Products
            .Where(x => topItems.Contains(x.ProductId))
            .ToList();
        
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    
    [HttpGet]
    public IActionResult Products(int itemsPerPage = 10, int pageNum = 1, List<int> categories = null, List<string> colors = null)
    {
        var primaryColors = _repo.Products.Select(x => x.PrimaryColor);
        var secondaryColors = _repo.Products.Select(x => x.SecondaryColor);
        var allColors = primaryColors.Concat(secondaryColors).Distinct().ToList();
    
        // Filter products based on selected categories and colors
        IQueryable<Product> filteredProducts = _repo.Products;

        if (categories != null && categories.Any())
        {
            filteredProducts = filteredProducts.Where(p => p.ProductCategories.Any(pc => categories.Contains(pc.CategoryId)));
        }

        if (colors != null && colors.Any())
        {
            filteredProducts = filteredProducts.Where(p => colors.Contains(p.PrimaryColor) || colors.Contains(p.SecondaryColor));
        }
    
        var model = new ProductListViewModel
        {
            Products = filteredProducts
                .OrderBy(x => x.Name)
                .Skip((pageNum - 1) * itemsPerPage)
                .Take(itemsPerPage),

            PaginationInfo = new PaginationInfo
            {
                CurrentPage = pageNum,
                ItemsPerPage = itemsPerPage,
                TotalItems = filteredProducts.Count()
            },
        
            Categories = _repo.Categories
                .OrderBy(x => x.CategoryName),
        
            Colors = allColors,
            SelectedCategories = categories,
            SelectedColors = colors,
            ItemsPerPage = itemsPerPage
        };

        return View(model);
    }

    public IActionResult AboutUs()
    {
        return View();
    }

    public IActionResult ProductDetails(int productId)
    {
        Product product = _repo.Products
            .FirstOrDefault(x => x.ProductId == productId);

        ViewBag.Product = product;

        List<byte?> recommendationIds = _repo.ItemRecommendations
            .Where(x => x.ProductId == productId)
            .Select(x => new List<byte?> { x.Recommendation1, x.Recommendation2, x.Recommendation3, x.Recommendation4, x.Recommendation5 })
            .ToList()
            .SelectMany(x => x)
            .ToList();


        List<Product> recommendationProducts = _repo.Products
            .Where(x => recommendationIds.Contains(x.ProductId))
            .ToList();

        ViewBag.Recommendations = recommendationProducts;
        
        return View();
    }

    [HttpGet]
    public IActionResult Checkout()
    {
        return View();
    }
    
    public IActionResult OrderSubmit()
    {
        return View();
    }
    
    // public IActionResult Checkout(Cart cart)
    // {
    //     // Check if user is signed in
    //     if (_signInManager.IsSignedIn(User))
    //     {
    //         // Get the current signed-in user's email
    //         string? userEmail = _signInManager.UserManager.GetUserName(User);
    //         
    //     }
    //
    //     return View();
    // }
}