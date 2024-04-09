using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BrickVault.Models;

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
}