using BrickVault.Controllers.Infrastructure;
using BrickVault.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BrickVault.Pages
{
    [Authorize(Roles = "Customer")]
    public class CartModel : PageModel
    {
        private ILegoRepository _repo;
        public Cart Cart { get; set; }
        public CartModel(ILegoRepository temp, Cart cartService)
        {
            _repo = temp;
            Cart = cartService;
        }
        //return to where they were shopping. Else, default to home.
        public string ReturnUrl { get; set; } = "/";
        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl ?? "/";
            //Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
        }

        public IActionResult OnPost(int productId, int quantity, int price, string returnUrl)
        {
            Product prod = _repo.Products

                .FirstOrDefault(x => x.ProductId == productId);

            if (prod != null)
            {
                if (quantity == null || quantity == 0)
                {
                    quantity = 1;
                }
                Cart.AddItem(prod, quantity, price);
            }
            
            // pass in the redirect url
            return RedirectToPage(new { returnUrl = returnUrl });

        }

        public IActionResult OnPostRemove(int productId, string returnUrl)
        {
            Cart.RemoveLine(Cart.Lines.First(x => x.Product.ProductId == productId).Product);
            return RedirectToPage(new { returnUrl = returnUrl });
        }
    }
}
