using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BrickVault.Models;
using BrickVault.Models.ViewModels;

namespace BrickVault.Controllers;

public class HomeController : Controller
{
    private ILegoRepository _repo;
    public HomeController(ILegoRepository tmp)
    {
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
    
    public IActionResult Products(int pageNum = 1)
    {
        int pageSize = 5;
        
        var model = new ProductListViewModel
        {
            Products = _repo.Products
                .OrderBy(x => x.Name)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize),
        
            PaginationInfo = new PaginationInfo
            {
                CurrentPage = pageNum,
                ItemsPerPage = pageSize,
                TotalItems = _repo.Products.Count()
            }
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