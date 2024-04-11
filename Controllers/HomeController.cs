using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BrickVault.Models;
using BrickVault.Models.ViewModels;

namespace BrickVault.Controllers;

public class HomeController : Controller
{
    private IConfiguration _configuration;
    private ILegoRepository _repo;
    public HomeController(IConfiguration configuration, ILegoRepository tmp)
    {
        _configuration = configuration;
        _repo = tmp;
    }
    
    public IActionResult Index()
    {
        ViewBag.Products = _repo.Products
            .OrderBy(x => x.Name)
            .ToList();
        
        return View();
    }

    public IActionResult Register()
    {
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
            //SelectedCategories = categories,
            //SelectedColors = colors,
            //ItemsPerPage = itemsPerPage
        };

        return View(model);
    }


    
    public IActionResult AboutUs()
    {
        return View();
    }
    
    public IActionResult ReviewOrders()
    {
        return View();
    }
    
    public IActionResult Cart()
    {
        return View();
    }
}