using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketList.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MarketList.Controllers
{
    public class ProductListController : Controller
    {
        [BindProperty]
        public ProductList ProductList { get; set; }

        private readonly ApplicationDbContext _db;
        private readonly ProductController _productController;
        private readonly CategoryController _categoryController;

        public ProductListController(ApplicationDbContext db)
        {
            _db = db;
            _productController = new ProductController(_db);
            _categoryController = new CategoryController(_db);
        }

        [HttpGet]
        public async Task<IList<ProductList>> GetAll()
        {
            try
            {
                var productList = await _db.ProductList
                    .Include("Product")
                    .ToListAsync();

                if (productList == null || productList.Count == 0)
                    return productList;

                foreach (var product in productList)
                {
                    product.Product = await _productController.GetById(product.IdProduct);
                }

                return productList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        [HttpGet]
        public async Task<IList<ProductList>> GetAll(int idMarketList)
        {
            try
            {
                var productList = await _db.ProductList
                    .Where(w => w.IdMarketList == idMarketList)
                    .ToListAsync();

                foreach (var product in productList)
                {
                    product.Product = await _productController.GetById(product.IdProduct);
                }

                return productList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        [HttpGet]
        public async Task<IList<ProductList>> GetAllOfCategory(int idMarketList, int idCategory)
        {
            try
            {
                var productList = await _db.ProductList
                    .Where(
                        p => p.IdMarketList == idMarketList && 
                        p.Product.IdCategory == idCategory
                    ).ToListAsync();

                foreach (var item in productList)
                {
                    item.Product = await _productController.GetById(item.IdProduct);
                }

                return productList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        [HttpGet]
        public async Task<IList<Category>> GetAllCategoriesOfMarketList(int idMarketList)
        {
            try
            {
                var idsCategory = await _db.ProductList
                    .Include("Product")
                    .Where(p => p.IdMarketList == idMarketList)
                    .Select(p => p.Product.IdCategory)
                    .Distinct().ToListAsync();

                if (idsCategory == null)
                    return null;

                var listCategories = new List<Category>();
                foreach (var id in idsCategory)
                {
                    var category = await _categoryController.GetById(id);
                    listCategories.Add(category);
                }

                return listCategories;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Upsert(int idMarketList, int? idProductList)
        {
            var products = await _productController.GetAll();
            ViewData["IdMarketList"] = idMarketList;
            ViewData["Products"] = new SelectList(products, "Id", "Name");

            if (idProductList == null || idProductList == 0)
            {
                // Insert
                ProductList = new ProductList();
                ProductList.IdMarketList = idMarketList;

                return View(ProductList);
            }
            else
            {
                // Update
                ProductList = await _db.ProductList.FirstOrDefaultAsync(p => p.Id == idProductList);

                if (ProductList == null)
                    return NotFound();

                ProductList.Product = await _productController.GetById(ProductList.IdProduct);

                return View(ProductList);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Upsert()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (ProductList.Id == 0)
                    {
                        // Insert
                        await _db.ProductList.AddAsync(ProductList);
                    }
                    else
                    {
                        // Update
                        var productListDb = await _db.ProductList.FirstOrDefaultAsync(p => p.Id == ProductList.Id);

                        if (productListDb == null)
                            return NotFound();

                        productListDb.IdProduct = ProductList.IdProduct;
                        productListDb.Qty = ProductList.Qty;
                        productListDb.Checked = ProductList.Checked;
                    }

                    await _db.SaveChangesAsync();
                    return RedirectToAction("Item", "MarketList", new { id = ProductList.IdMarketList });
                }
                else
                {
                    return RedirectToAction("Upsert", new { idProductList = ProductList.Id, idMarketList = ProductList.IdMarketList });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return RedirectToAction("Upsert", new { idProductList = ProductList.Id, idMarketList = ProductList.IdMarketList });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int idProductList)
        {
            try
            {
                var productListDb = await _db.ProductList.FirstOrDefaultAsync(p => p.Id == idProductList);
                
                if (productListDb == null)
                    return NotFound();
                
                var idMarketList = productListDb.IdMarketList;

                _db.ProductList.Remove(productListDb);
                await _db.SaveChangesAsync();

                return Json(new { success = true, message = "Apagado com sucesso", idMarketList = idMarketList });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Json(new { success = false, message = e.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> CheckItem(int idProductList)
        {
            try
            {
                var productListDb = await _db.ProductList.FirstOrDefaultAsync(p => p.Id == idProductList);

                if (productListDb == null)
                    return NotFound();

                if (!productListDb.Checked)
                    productListDb.Checked = true;
                else
                    productListDb.Checked = false;

                await _db.SaveChangesAsync();
                return Json(new { success = true, isChecked = productListDb.Checked });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Json(new { success = false, message = e.Message });
            }
        }
    }
}
