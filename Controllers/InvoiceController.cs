using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PartyProductWebApi.Models;

namespace PartyProductWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly PartyProductWebApiContext _context;

        public InvoiceController(PartyProductWebApiContext context)
        {
            this._context = context;

        }

        [HttpPost]
        public async Task<ActionResult<InvoiceAddDTO>> SaveInvoice([FromBody] InvoiceAddDTO invoiceAdd)
        {
            if (invoiceAdd == null)
                return BadRequest();

            _context.Invoices.Add(new Invoice
            {
                CurrentRate = invoiceAdd.CurrentRate,
                Quantity = invoiceAdd.Quantity,
                PartyId = invoiceAdd.PartyId,
                ProductId = invoiceAdd.ProductId,
                Date = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
            });
            await _context.SaveChangesAsync();
            return Ok("Invoice Added Successfully..");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateInvoice([FromRoute] int id, InvoiceAddDTO invoice)
        {
            var invoiceFromDb = _context.Invoices.FirstOrDefault(x=> x.Id == id);
            if (invoiceFromDb == null)
            {
                return NotFound();
            }

            invoiceFromDb.CurrentRate = invoice.CurrentRate;
            invoiceFromDb.Quantity = invoice.Quantity;
            invoiceFromDb.PartyId = invoice.PartyId;
            invoiceFromDb.ProductId = invoice.ProductId;
            invoiceFromDb.Date = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);


            await _context.SaveChangesAsync();
            return Ok("Invoice Updated Successfully..");
        }

        [HttpGet("{partyId}")]
        public async Task<ActionResult> GetInvoiceList(string partyId, [FromQuery] string? productName = null, [FromQuery] string? date = null)
        {
            var invoiceList = await _context.InvoiceDTOs
                .FromSqlRaw("EXEC GetInvoicesByPartyAndProductAndDate @PartyId, @ProductName, @Date",
                    new SqlParameter("@PartyId", partyId),
                    new SqlParameter("@ProductName", (object)productName ?? DBNull.Value),
                    new SqlParameter("@Date", (object)date ?? DBNull.Value)).ToListAsync();

            var list = new List<InvoiceDTO>();
            foreach (var item in invoiceList)
            {
                list.Add(new InvoiceDTO()
                {
                    Id = item.Id,
                    CurrentRate = item.CurrentRate,
                    Quantity = item.Quantity,
                    PartyName = item.PartyName,
                    ProductName = item.ProductName,
                    Date = item.Date,
                    Total = item.Quantity * item.CurrentRate
                });
            }

            return Ok(list);
        }
    }
}
