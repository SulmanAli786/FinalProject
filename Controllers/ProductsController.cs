using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping_Cart.Models;

namespace Shopping_Cart.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ShoppingCartContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(ShoppingCartContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction(nameof(UsersController.Login), "Users");
            }
            //return _context.Products != null ? 
            //            View(await _context.Products.ToListAsync()) :
            //            Problem("Entity set 'ShoppingCartContext.Products'  is null.");
            var ProductList = (from product in _context.Products
                               from cata in _context.Categories.Where(x => x.Id == product.CategoryId)
                               select new Product
                               {
                                   Id = product.Id,
                                   Name = product.Name,
                                   CategoryId = product.CategoryId,
                                   Description = product.Description,
                                   Quantity = product.Quantity,
                                   Images = product.Images,
                                   Sku = product.Sku,

                               }).ToList();
            return View(ProductList);
        }
        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction(nameof(UsersController.Login), "Users");
            }
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction(nameof(UsersController.Login), "Users");
            }
            ViewBag.CategoryList = _context.Categories.ToList();
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Images,Quantity,Description,Price,Sku,CategoryId,Currency,Status,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")] Product product, IList<IFormFile> FileImages)
        {
            var CommaSeperatedString = "";
            if (FileImages != null)
            {
                foreach(IFormFile item in FileImages)
                {
                    var ImagePath = "/imgs/" + Guid.NewGuid().ToString() + Path.GetExtension(item.FileName);
                    using(FileStream dd = new FileStream(_webHostEnvironment.WebRootPath+ImagePath, FileMode.Create))
                    {
                        item.CopyTo(dd);
                    }
                    CommaSeperatedString+=","+ ImagePath;
                }
            }
            if (ModelState.IsValid)
            {
                if (CommaSeperatedString.StartsWith(","))
                {
                    product.Images = CommaSeperatedString.Remove(0, 1);
                }
                product.CreatedDate= DateTime.Now;
                product.CreatedBy = "System";
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction(nameof(UsersController.Login), "Users");
            }
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Images,Quantity,Description,Price,Sku,CategoryId,Currency,Status,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")] Product product, IFormFile?Images)
        {
            var ImagePath = "/imgs/" + Guid.NewGuid().ToString() + Path.GetExtension(Images.FileName);
            using (FileStream dd = new FileStream(_webHostEnvironment.WebRootPath + ImagePath, FileMode.Create))
            {
                Images.CopyTo(dd);
            }
            product.Images += "" + ImagePath;
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Edit));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction(nameof(UsersController.Login), "Users");
            }
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'ShoppingCartContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
     
    }
}
