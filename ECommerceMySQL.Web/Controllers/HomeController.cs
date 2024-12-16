using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerceMySQL.Web.Models;
using ECommerceMySQL.Web.Data;

namespace ECommerceMySQL.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Get all categories
            ViewBag.Categories = await _context.Categories.ToListAsync();

            // Get featured products (random selection for demo)
            ViewBag.FeaturedProducts = await _context.Products
                .Include(p => p.Category)
                .OrderBy(r => Guid.NewGuid())
                .Take(8)
                .ToListAsync();

            // Get recommended products (random selection for demo)
            ViewBag.RecommendedProducts = await _context.Products
                .Include(p => p.Category)
                .OrderBy(r => Guid.NewGuid())
                .Take(8)
                .ToListAsync();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
