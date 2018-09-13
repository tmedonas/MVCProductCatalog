using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tm_api.Models;

namespace tm_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCatalogController : Controller
    {
        private readonly ProductContext _context;

        public ProductCatalogController(ProductContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductDto>>> GetProductList()
        {
            List<ProductDto> items;
            try
            {
                items = await _context.ProductDto.OrderBy(o => o.Id).ToListAsync();
                if (items == null)
                    return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error:\n" + ex.Message);
            }

            return items;
        }

        [HttpGet("{id}", Name = "GetProduct")]
        public async Task<ActionResult<ProductDto>> GetProduct(long id)
        {
            ProductDto item;
            try
            {
                item = await _context.ProductDto.FindAsync(id);
                if (item == null)
                    return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error:\n" + ex.Message);
            }

            return item;
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductDto item)
        {
            try
            {
                if (_context.ProductDto.Count(c => c.Code == item.Code) > 0)
                    return StatusCode(422, "Product Code already exists!");
                item.LastUpdated = DateTime.Now;
                _context.ProductDto.Add(item);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
            return CreatedAtRoute("GetProduct", new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, ProductDto item)
        {
            try
            {
                if (_context.ProductDto.Count(c => c.Id != item.Id && c.Code == item.Code) > 0)
                    return StatusCode(422, "Product Code already exists!");
                var item_to_update = _context.ProductDto.Find(id);
                item_to_update.Code = item.Code;
                item_to_update.Name = item.Name;
                item_to_update.Photo = item.Photo;
                item_to_update.Price = item.Price;
                item_to_update.LastUpdated = DateTime.Now;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
            return CreatedAtRoute("GetProduct", new { id = item.Id }, item);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var item = _context.ProductDto.Find(id);
                _context.ProductDto.Remove(item);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error:\n" + ex.Message);
            }
            return StatusCode(200);
        }
    }
}