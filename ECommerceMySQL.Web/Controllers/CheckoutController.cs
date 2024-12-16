using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerceMySQL.Web.Data;
using ECommerceMySQL.Web.Models;
using System.Security.Claims;

namespace ECommerceMySQL.Web.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CheckoutController(ApplicationDbContext context)
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

            if (!cartItems.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            ViewBag.CartTotal = cartItems.Sum(item => item.Product.Price * item.Quantity);
            return View(new CheckoutModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessPayment(CheckoutModel model)
        {
            if (!ModelState.IsValid)
            {
                // Get cart total for the view
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var currentCartItems = await _context.CartItems
                    .Include(c => c.Product)
                    .Where(c => c.UserId == currentUserId)
                    .ToListAsync();
                ViewBag.CartTotal = currentCartItems.Sum(item => item.Product.Price * item.Quantity);
                return View("Index", model);
            }

            try
            {
                // Clear the user's cart
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var itemsToRemove = await _context.CartItems
                    .Where(c => c.UserId == userId)
                    .ToListAsync();

                _context.CartItems.RemoveRange(itemsToRemove);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Payment processed successfully! Thank you for your purchase.";
                return RedirectToAction("Success");
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error processing payment: {ex.Message}");
                TempData["Error"] = "An error occurred while processing your payment. Please try again.";
                return RedirectToAction("Index");
            }
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
