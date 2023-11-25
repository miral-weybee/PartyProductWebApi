using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartyProductWebApi.Models;

namespace PartyProductWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly PartyProductWebApiContext _context;

        public ProductController(PartyProductWebApiContext context)
        {
            this._context = context;

        }
        [HttpGet]
        public async Task<ActionResult<List<ProductDTO>>> GetProductList()
        {
            var list = new List<ProductDTO>();
            var temp = await _context.Products
                .ToListAsync();
            foreach (var item in temp)
            {
                list.Add(new ProductDTO
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName
                });
            }
            return Ok(list);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDTO>> SaveProduct([FromBody] Product product)
        {
            if (product == null)
                return BadRequest();

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return Ok("Product Added Successfully..");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct([FromRoute] int id, ProductDTO productDto)
        {
            var product = _context.Products.Where(x => x.ProductId == id).FirstOrDefault();
            if (product == null)
            {
                return NotFound();
            }
            product.ProductName = productDto.ProductName;

            await _context.SaveChangesAsync();
            return Ok("Product updated Successfully..");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct([FromRoute] int id)
        {
            var product = _context.Products.SingleOrDefault(x => x.ProductId == id);
            if (product == null)
                return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return Ok("Product Deleted Successfully..");

        }
    }
}
