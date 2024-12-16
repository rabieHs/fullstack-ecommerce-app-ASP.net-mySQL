using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerceMySQL.Web.Data;
using ECommerceMySQL.Web.Models;
using System.Security.Claims;

namespace ECommerceMySQL.Web.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItems = await _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            return View(cartItems);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Account", new { returnUrl = Request.Headers["Referer"].ToString() });
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                
                // First check if the product exists
                var product = await _context.Products.FindAsync(productId);
                if (product == null)
                {
                    TempData["Error"] = "Product not found.";
                    return RedirectToAction("Index", "Products");
                }

                var cartItem = await _context.CartItems
                    .Include(c => c.Product)
                    .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);

                if (cartItem == null)
                {
                    cartItem = new CartItem
                    {
                        UserId = userId,
                        ProductId = productId,
                        Product = product,
                        Quantity = quantity,
                        CreatedAt = DateTime.UtcNow
                    };
                    _context.CartItems.Add(cartItem);
                }
                else
                {
                    cartItem.Quantity += quantity;
                }

                await _context.SaveChangesAsync();
                TempData["Success"] = "Item added to cart successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error adding item to cart: {ex.Message}");
                TempData["Error"] = "An error occurred while adding the item to cart.";
                return RedirectToAction("Index", "Products");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateQuantity([FromBody] UpdateCartItemRequest request)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var cartItem = await _context.CartItems
                    .Include(c => c.Product)
                    .FirstOrDefaultAsync(c => c.Id == request.Id && c.UserId == userId);

                if (cartItem == null)
                {
                    return Json(new { success = false, message = "Cart item not found" });
                }

                if (request.Quantity < 1)
                {
                    return Json(new { success = false, message = "Quantity must be at least 1" });
                }

                cartItem.Quantity = request.Quantity;
                await _context.SaveChangesAsync();

                var subtotal = cartItem.Product.Price * cartItem.Quantity;
                return Json(new { 
                    success = true, 
                    subtotal = subtotal.ToString("F2"),
                    quantity = cartItem.Quantity
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Failed to update quantity" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

            if (cartItem == null)
                return NotFound();

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }

    public class UpdateCartItemRequest
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
    }
}
