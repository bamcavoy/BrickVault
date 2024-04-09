using Microsoft.AspNetCore.Mvc;

namespace BrickVault.Controllers;

public class TestController : Controller
{
    
    // GET
    public IActionResult Index()
    {
        return View("~/Views/Home/Index.cshtml");
    }
    
    public IActionResult Privacy()
    {
        return View("~/Views/Home/Privacy.cshtml");
    }
}