using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping_Cart.Models;
using System.Diagnostics;

namespace Shopping_Cart.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ShoppingCartContext _context;

        public HomeController(ILogger<HomeController> logger, ShoppingCartContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var CategoriesList = _context.Categories.Select(m => m.Name).ToList();
            HttpContext.Session.SetString("CategoriesList", String.Join(",", CategoriesList));
            //ViewBag.category = _context.Products.ToList();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult About()
        {
            ViewBag.category = _context.Products.ToList();
            return View();
        }
        public IActionResult Contact()
        {
            ViewBag.category = _context.Products.ToList();
            return View();
        }

        // GET: Products/Details/5
      



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
      public IActionResult Shirts()
        {
            ViewBag.category = _context.Products.ToList();

            return View();
        }
       public IActionResult Jackets()
        {
            ViewBag.category = _context.Products.ToList();
            return View();
        }
        public IActionResult Jeans()
        {
            ViewBag.category = _context.Products.ToList();
            return View();
        }
        public IActionResult Shoes()
        {
            ViewBag.category = _context.Products.ToList();
            return View();
        }
       
    }
}