using System.Diagnostics;
using Azure;
using Microsoft.AspNetCore.Mvc;
using BrickVault.Models;
using BrickVault.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BrickVault.Controllers;

public class AdminController : Controller
{
    private readonly ILegoRepository _repository;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ILogger<AdminController> _logger;

    public AdminController(ILegoRepository repository, ILogger<AdminController> logger, UserManager<IdentityUser>usermanager)
    {
        _repository = repository;
        _logger = logger;
        _userManager = usermanager;
    }

    [HttpGet]
    public IActionResult AdminProductList()
    {
        var products = _repository.Products.ToList();
        return View("~/Pages/Admin/AdminProductList.cshtml", products);
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
    public IActionResult AdminEditProduct(int id)
    {
        var product = _repository.Products.FirstOrDefault(p => p.ProductId == id);
        if (product == null)
        {
            return NotFound();
        }

        ViewBag.Categories = new SelectList(_repository.Categories, "CategoryId", "Name");
        return View("~/Pages/Admin/AdminEditProduct.cshtml", product);
    }


    [HttpPost]
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

    public IActionResult AdminUserList()
    {
        var identityUsers = _userManager.Users.ToList(); // Get the list of IdentityUser objects

        // Convert the list of IdentityUser objects to a list of AspNetUser objects
        var aspNetUsers = identityUsers.Select(iu => new AspNetUser
        {
            Id = iu.Id,
            UserName = iu.UserName,
            Email = iu.Email,
            PhoneNumber = iu.PhoneNumber
            // Map other properties as needed
        }).ToList();

        // Pass the list of AspNetUser objects to the view
        return View("~/Pages/Admin/AdminUserList.cshtml", aspNetUsers);
    }  
    [HttpGet]
    public async Task<IActionResult> AdminEditUsers(string id)
    {
        var identityUser = await _userManager.FindByIdAsync(id);
        if (identityUser == null)
        {
            return NotFound();
        }

        var aspNetUser = new AspNetUser
        {
            Id = identityUser.Id,
            UserName = identityUser.UserName,
            Email = identityUser.Email,
            PhoneNumber = identityUser.PhoneNumber,
            // Map other properties as needed
        };

        return View("~/Pages/Admin/AdminEditUsers.cshtml", aspNetUser);
    }


    [HttpPost]
    public async Task<IActionResult> AdminEditUsers(string id, AspNetUser updatedUser)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            // Update the user properties
            user.UserName = updatedUser.UserName;
            user.Email = updatedUser.Email;
            user.PhoneNumber = updatedUser.PhoneNumber;
            // Add other properties as needed

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("AdminUserList");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
        }

        return View("~/Pages/Admin/AdminEditUsers.cshtml", updatedUser);
    }
    
    [HttpGet]
    public async Task<IActionResult> DeleteUserConfirmation(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        var aspNetUser = new AspNetUser
        {
            Id = user.Id,
            UserName = user.UserName,
            // Map other properties as needed
        };

        return View("~/Pages/Admin/DeleteUserConfirmation.cshtml", aspNetUser);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteUserConfirmed(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user != null)
        {
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                // Handle errors (optional)
            }
        }

        return RedirectToAction("AdminUserList");
    }




    public IActionResult AdminReviewOrders()
    {
        //In this method, we want the admin to be able to look at fraudulent activity, and have a way to resolve this solution??
        return View("~/Pages/Admin/AdminReviewOrders.cshtml");
    }

    public IActionResult AdminAddUser()
    {
        return View("~/Pages/Admin/AdminAddUser.cshtml");
    }

    public void AdminSwitchBetweenViews()
    {
        //MakeDaFunction
    }

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
    public IActionResult AdminAddProduct()
    {
        Product newProduct = new Product();
        return View("~/Pages/Admin/AdminAddProduct.cshtml", newProduct);
    }

    [HttpPost]
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
