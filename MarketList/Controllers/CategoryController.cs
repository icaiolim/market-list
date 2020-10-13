using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketList.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarketList.Controllers
{
    public class CategoryController : Controller
    {
        [BindProperty]
        public Category Category { get; set; }

        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpPut]
        public async Task<IActionResult> Create(IList<Category> categories)
        {
            try
            {
                if (categories == null || categories.Count == 0)
                    throw new ArgumentException("Lista de Categorias null ou vazia", nameof(categories));

                await _db.Category.AddRangeAsync(categories);
                await _db.SaveChangesAsync();

                return Json(new { success = true, message = "Categorias adicionadas com sucesso!" });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Json(new { success = false, message = e.Message });
            }
        }

        [HttpGet]
        public async Task<IList<Category>> GetAll()
        {
            try
            {
                return await _db.Category.ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        [HttpGet]
        public async Task<Category> GetById(int id)
        {
            try
            {
                return await _db.Category.FirstOrDefaultAsync(c => c.Id == id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}
