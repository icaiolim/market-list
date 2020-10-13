using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketList.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarketList.Controllers
{
    public class MarketListController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ProductListController _productListController;
        private readonly CategoryController _categoryController;

        [BindProperty]
        public Models.MarketList MarketList { get; set; }
        public IList<Models.MarketList> MarketLists { get; set; }

        public MarketListController(ApplicationDbContext db)
        {
            _db = db;
            _productListController = new ProductListController(_db);
            _categoryController = new CategoryController(_db);
        }

        [HttpGet]
        public async Task<IList<Models.MarketList>> GetAll()
        {
            try
            {
                var marketLists = await _db.MarketList.ToListAsync();

                foreach (var item in marketLists)
                {
                    item.ProductList = await _productListController.GetAll(item.Id);
                }

                return marketLists;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Upsert(int? idMarketList)
        {
            if (idMarketList == null || idMarketList == 0)
            {
                // Insert
                MarketList = new Models.MarketList();

                return View(MarketList);
            }
            else
            {
                // Update
                MarketList = await _db.MarketList.FirstOrDefaultAsync(m => m.Id == idMarketList);

                if (MarketList == null)
                    return NotFound();

                return View(MarketList);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Upsert()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (MarketList.Id == 0)
                    {
                        // Insert

                        MarketList.DateCreation = DateTime.Now;
                        await _db.MarketList.AddAsync(MarketList);
                    }
                    else
                    {
                        // Update
                        var marketListDb = await _db.MarketList.FirstOrDefaultAsync(m => m.Id == MarketList.Id);

                        if (marketListDb == null)
                            return NotFound();

                        marketListDb.Name = MarketList.Name;
                    }

                    await _db.SaveChangesAsync();
                    return RedirectToAction("Index", "MarketList");
                }
                else
                {
                    return RedirectToAction("Upsert", new { id = MarketList.Id });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return RedirectToAction("Upsert", new { id = MarketList.Id });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var marketListDb = await _db.MarketList.Include("ProductList").FirstOrDefaultAsync(p => p.Id == id);

                if (marketListDb == null)
                    return NotFound();


                _db.ProductList.RemoveRange(marketListDb.ProductList);
                await _db.SaveChangesAsync();

                _db.MarketList.Remove(marketListDb);
                await _db.SaveChangesAsync();

                return Json(new { success = true, message = "Apagado com sucesso" });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Json(new { success = false, message = e.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Item(int id, int? idCategory)
        {
            try
            {
                MarketList = await _db.MarketList.FirstOrDefaultAsync(m => m.Id == id);

                if (MarketList == null)
                    return NotFound();

                if (idCategory == null)
                {
                    MarketList.ProductList = await _productListController.GetAll(id);
                }
                else
                {
                    MarketList.ProductList = await _productListController.GetAllOfCategory(MarketList.Id, (int)idCategory);
                    if (MarketList.ProductList == null)
                        return NotFound();
                }

                ViewData["IdCategory"] = idCategory == null ? 0 : idCategory;
                ViewData["Categories"] = await _productListController.GetAllCategoriesOfMarketList(MarketList.Id);
                return View(MarketList);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return View(MarketList);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            MarketLists = await GetAll();
            
            if (MarketLists == null)
            {
                MarketLists = new List<Models.MarketList>();
            }

            ViewData["MarketLists"] = MarketLists;

            return View();
        }
    }
}
