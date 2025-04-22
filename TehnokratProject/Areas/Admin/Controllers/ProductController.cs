using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TehnokratProject.Areas.Admin.ViewModels;
using TehnokratProject.Data;
using TehnokratProject.Models;

namespace TehnokratProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        ApplicationDbContext db;
        public ProductController(ApplicationDbContext context)
        {
            db = context;
        }
        public async Task<IActionResult> Index()
        {
            var products = await db.products
                .Include(p => p.category)
                .ToListAsync();
            return View(products);
        }

        public async Task<IActionResult> Create()
        {
            var categories = await db.categories.ToListAsync();
            var viewModel = new ProductCreateViewModel
            {
                Categories = categories
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateViewModel model)
        {
            var product = new Product
            {
                title = model.title,
                description = model.description,
                status = true,
                quantity = 0,
                image_path = "",
                category_id = model.category_id.Value,
                category = db.categories.FirstOrDefault(p => p.id == model.category_id),
                
                price = model.price
            };
            db.products.Add(product);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditPost(Product product)
        {
            Product existing_product = db.products.FirstOrDefault(p => p.id == product.id);
            if (existing_product == null)
                return NotFound();
            existing_product.image_path = "";
            existing_product.title = product.title;
            existing_product.description = product.description;
            existing_product.price = product.price;
            existing_product.quantity = product.quantity;
            existing_product.status = product.status;
            existing_product.image_path = "";
            
            db.products.Update(existing_product);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                Product? product = await db.products.FirstOrDefaultAsync(c => c.id == id);
                if (product != null)
                {
                    return View(product);
                }
            }
            return NotFound();
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Product? product = await db.products.FirstOrDefaultAsync(p => p.id == id);
                if (product != null)
                {
                    db.products.Remove(product);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }
    }
}
