using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartyProductWebApi.Models;

namespace PartyProductWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductRateController : ControllerBase
    {
        private readonly PartyProductWebApiContext _context;

        public ProductRateController(PartyProductWebApiContext context)
        {
            this._context = context;

        }
        [HttpGet]
        public async Task<ActionResult<List<ProductRateDTO>>> GetProductRateList()
        {
            var list = new List<ProductRateDTO>();
            var temp = await _context.ProductRates.Include(x => x.ProductNameProduct)
                .ToListAsync();
            foreach (var item in temp)
            {
                list.Add(new ProductRateDTO
                {
                    ProductRateId = item.Id,
                    ProductName = item.ProductNameProduct.ProductName,
                    Rate = item.Rate,
                    DateOfRate = item.DateOfRate

                });
            }
            return Ok(list);
        }

        [HttpPost]
        public async Task<ActionResult<ProductRateAddDTO>> SaveProductRate([FromBody] ProductRateAddDTO product)
        {
            if (product == null)
                return BadRequest();

            _context.ProductRates.Add(new ProductRate()
            {
                Rate = product.Rate,
                DateOfRate = product.DateOfRate,
                ProductNameProductId = product.ProductId

            }) ;
            await _context.SaveChangesAsync();
            return Ok("Product Rate Added Successfully..");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProductRate([FromRoute] int id, ProductRateAddDTO productDto)
        {
            var product = _context.ProductRates.Where(x => x.Id == id).FirstOrDefault();
            if (product == null)
            {
                return NotFound();
            }
            product.DateOfRate = productDto.DateOfRate;
            product.Rate    = productDto.Rate;
            product.ProductNameProductId = productDto.ProductId;

            await _context.SaveChangesAsync();
            return Ok("Product Rate updated Successfully..");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProductRate([FromRoute] int id)
        {
            var product = _context.ProductRates.SingleOrDefault(x => x.Id == id);
            if (product == null)
                return NotFound();

            _context.ProductRates.Remove(product);
            await _context.SaveChangesAsync();
            return Ok("Product Rate Deleted Successfully..");

        }
    }
}
