using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BrickVault.Models;
using BrickVault.Models.ViewModels;

namespace BrickVault.Controllers;

public class AdminController: Controller
{
    [HttpPost]
    public IActionResult AdminAddProduct()
    {
        //This will essentially be a form for admins that will pass the necessary data to the joinning table.  
        return View();
    }

    public IActionResult AdminDashboard()
    {
        return View();
    }
    
    [HttpGet]
    public IActionResult AdminProductList()
    {
        //This method will gather all of the current products in our db by extracting the product_id.  
        //In the cshtml there should be buttons to link you to the add product.  and two buttons one to edit and one to delete.  
        return View();
    }
        
    [HttpDelete]
    public void DeleteProduct()
    {
        //Make da function
    }
    
    [HttpGet]
    public IActionResult AdminEditProduct()
    {
        //This will extract data from the db after the admin selects a product to edit from the ProductList
        //You'll be pulling off the production_id
        return View();
    }
    
    [HttpPost]
    public IActionResult AdminEditUsers()
    {
        return View();
    }

    [HttpDelete]
    public void DeleteUser()
    {
        //Delete da user.
    }

    

    public IActionResult AdminReviewOrders()
    {
        //In this method, we want the admin to be able to look at fraudulent activity, and have a way to resolve this solution??
        return View();
    }

    public IActionResult AdminAddUser()
    {
        return View();
    }

    public void AdminSwitchBetweenViews()
    {
        //MakeDaFunction
    }
}