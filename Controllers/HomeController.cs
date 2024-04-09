using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BrickVault.Models;
using BrickVault.Models.ViewModels;

namespace BrickVault.Controllers;

public class HomeController : Controller
{

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    
    public IActionResult Products(int pageNum)
    {
        int pageSize = 10;
        
        var model = new ProductListViewModel
        {
            // Products = _repo.Products
            //     .OrderBy(x => x.Title)
            //     .Skip((pageNum - 1) * pageSize)
            //     .Take(pageSize),
        
            // PaginationInfo = new PaginationInfo
            // {
            //     CurrentPage = pageNum,
            //     ItemsPerPage = pageSize,
            //     TotalItems = _repo.Products.Count()
            // }
            
            PaginationInfo = new PaginationInfo
            {
                CurrentPage = 1,
                ItemsPerPage = 5,
                TotalItems = 37
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