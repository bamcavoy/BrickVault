using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BrickVault.Models;
using BrickVault.Models.ViewModels;

namespace BrickVault.Controllers;

public class AdminController: Controller
{
    private readonly ILegoRepository _repository;

    public AdminController(ILegoRepository repository)
    {
        _repository = repository;
    }
    
    [HttpGet]
    public IActionResult AdminProductList()
    {
        var products = _repository.Products.ToList();
        return View(products);
    }

    public IActionResult AdminDashboard()
    {
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

    public IActionResult AdminUserList()
    {
        var users = _repository.AspNetUsers.ToList(); // Fetch the list of users
        return View(users); // Pass this list to the view
    }
    
    public IActionResult DeleteProductConfirmation(int productId)
    {
        var product = _repository.Products.FirstOrDefault(p => p.ProductId == productId);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }


    [HttpPost]
    public IActionResult DeleteProductConfirmed(int productId)
    {
        Product product = null;
        try
        {
            product = _repository.Products.FirstOrDefault(p => p.ProductId == productId);
            if (product != null)
            {
                _repository.DeleteProduct(product);
            }
        }
        catch (Exception ex)
        {
            // Log the exception or display an error message
            ModelState.AddModelError("", "An error occurred while deleting the product.");

            // Ensure that a product object is passed to the view, even if it's null
            return View("DeleteProductConfirmation", product);
        }

        return RedirectToAction("AdminProductList");
    }





    [HttpGet]
    public IActionResult AdminAddProduct()
    {
        Product newProduct = new Product();
        return View(newProduct);
    }

    [HttpPost]
    public IActionResult AdminAddProduct(Product product)
    {
        if (ModelState.IsValid)
        {
            _repository.AddProduct(product);
            _repository.SaveChanges();
            // Assuming you have a Save method in your repository
            return RedirectToAction("AdminProductList");
        }
        return View(product);
    }


   
}