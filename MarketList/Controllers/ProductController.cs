using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using MarketList.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace MarketList.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProductController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IList<Product>> GetAll()
        {
            try
            {
                var products = await _db.Product.Include("Category").ToListAsync();
                return products;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        [HttpGet]
        public async Task<Product> GetById(int id)
        {
            try
            {
                var product = await _db.Product
                    .Include("Category")
                    .FirstOrDefaultAsync(p => p.Id == id);
                
                return product;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        [HttpPut]
        public async Task<IActionResult> Create(string productsJson)
        {
            try
            {
                var products = JsonConvert.DeserializeObject<IList<Product>>(productsJson);
                
                if (products == null || products.Count == 0)
                    throw new ArgumentException("Lista de Produtos null ou vazia", nameof(products));

                _db.Product.AddRange(products);
                await _db.SaveChangesAsync();

                return Json(new { success = true, message = "Produtos adicionados com sucesso!" });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Json(new { success = false, message = e.Message });
            }
        }
    }
}
