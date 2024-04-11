using System.Diagnostics;
using Azure;
using Microsoft.AspNetCore.Mvc;
using BrickVault.Models;
using BrickVault.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BrickVault.Controllers;

public class AdminController : Controller
{
    private readonly ILegoRepository _repository;
    private readonly ILogger<AdminController> _logger;
    
    public AdminController(ILegoRepository repository, ILogger<AdminController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult AdminProductList()
    {
        var products = _repository.Products.ToList();
        return View("~/Pages/Admin/AdminProductList.cshtml", products);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult AdminDashboard()
    {
        return View();
    }

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public void DeleteProduct()
    {
        //Make da function
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult AdminEditProduct(int id)
    {
        var product = _repository.Products.FirstOrDefault(p => p.ProductId == id);
        if (product == null)
        {
            return NotFound();
        }
        return View("~/Pages/Admin/AdminEditProduct.cshtml", product);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult AdminEditProduct(Product product)
    {
        if (ModelState.IsValid)
        {
            _logger.LogInformation("Updating product with ID {ProductId}", product.ProductId);
            _repository.UpdateProduct(product);
            _repository.SaveChanges();
            _logger.LogInformation("Product with ID {ProductId} updated successfully", product.ProductId);
            return RedirectToAction("AdminProductList");
        }
        else
        {
            _logger.LogWarning("Model state is invalid. Errors: {ModelStateErrors}", string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
        }
        return View("~/Pages/Admin/AdminEditProduct.cshtml", product);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult AdminEditUsers()
    {
        return View("~/Pages/Admin/AdminEditUsers.cshtml");
    }

    [Authorize(Roles = "Admin")]
    public IActionResult AdminReviewOrders()
    {
        //In this method, we want the admin to be able to look at fraudulent activity, and have a way to resolve this solution??
        return View("~/Pages/Admin/AdminReviewOrders.cshtml");
    }

    [Authorize(Roles = "Admin")]
    public IActionResult AdminAddUser()
    {
        return View("~/Pages/Admin/AdminAddUser.cshtml");
    }

    public void AdminSwitchBetweenViews()
    {
        //MakeDaFunction
    }

    [Authorize(Roles = "Admin")]
    public IActionResult AdminUserList()
    {
        var usersQuery = _repository.AspNetUsers; // Keep it as IQueryable
        if (usersQuery == null)
        {
            // Handle the null case, maybe log it or return a different view
            return View("~/Pages/Admin/AdminUserList.cshtml");
        }
        return View("~/Pages/Admin/AdminUserList.cshtml", usersQuery.ToList()); // Materialize the query here
    }

    [Authorize(Roles = "Admin")]
    public IActionResult DeleteProductConfirmation(int productId)
    {
        var product = _repository.Products.FirstOrDefault(p => p.ProductId == productId);
        if (product == null)
        {
            return NotFound();
        }
        return View("~/Pages/Admin/DeleteProductConfirmation.cshtml", product);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
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
            ModelState.AddModelError("", "An error occurred while deleting the product.");
            return View("~/Pages/Admin/DeleteProductConfirmation.cshtml", product);
        }

        return RedirectToAction("AdminProductList");
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult AdminAddProduct()
    {
        Product newProduct = new Product();
        return View("~/Pages/Admin/AdminAddProduct.cshtml", newProduct);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult AdminAddProduct(Product product)
    {
        if (ModelState.IsValid)
        {
            _repository.AddProduct(product);
            _repository.SaveChanges();
            return RedirectToAction("AdminProductList");
        }
        return View("~/Pages/Admin/AdminAddProduct.cshtml", product);
    }
}
