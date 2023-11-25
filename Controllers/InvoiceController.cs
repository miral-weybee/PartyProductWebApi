using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PartyProductWebApi.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public async Task<ActionResult<InvoiceAddDTO>> SaveInvoice([FromBody] InvoiceAddDTO invoiceAddDTO)
        {
            if (invoiceAddDTO == null)
                return BadRequest();

            _context.Invoices.Add(new Invoice
            {
                Id = invoiceAddDTO.InvoiceId,
                CurrentRate = invoiceAddDTO.CurrentRate,
                Quantity = invoiceAddDTO.Quantity,
                PartyPartyId = invoiceAddDTO.PartyPartyId,
                ProductProductId = invoiceAddDTO.ProductProductId,
                Date = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
            });
            await _context.SaveChangesAsync();
            return Ok("Invoice Added Successfully..");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateInvoice([FromRoute] int id, InvoiceAddDTO invoiceAddDto)
        {
            var invoice = _context.Invoices.Where(x => x.Id == id).FirstOrDefault();
            if (invoice == null)
            {
                return NotFound();
            }

            invoice.CurrentRate = invoiceAddDto.CurrentRate;
            invoice.Quantity = invoiceAddDto.Quantity;
            invoice.PartyPartyId = invoiceAddDto.PartyPartyId;
            invoice.ProductProductId = invoiceAddDto.ProductProductId;
            invoice.Date = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);


            await _context.SaveChangesAsync();
            return Ok("Invoice Updated Successfully..");
        }

        [HttpGet("{partyId}")]
        public async Task<ActionResult> GetInvoiceList(int partyId, [FromQuery] string? productName = null, [FromQuery] string? date = null)
        {
            //var invoiceList = await _context.Invoices.FromSqlRaw("Exec GetInvoicesByPartyAndProductAndDate " + partyId+ ","+productName+ "," + date).ToListAsync();
            var invoiceList = await _context.Invoices
                .FromSqlRaw("EXEC GetInvoicesByPartyAndProductAndDate @PartyId, @ProductName, @Date",
                    new SqlParameter("@PartyId", partyId),
                    new SqlParameter("@ProductName", (object)productName ?? DBNull.Value),
                    new SqlParameter("@Date", (object)date ?? DBNull.Value)).ToListAsync();
            var list = new List<InvoiceGetDTO>();
            foreach (var item in invoiceList)
            {
                list.Add(new InvoiceGetDTO()
                {
                    InvoiceId = item.Id,
                    CurrentRate = item.CurrentRate,
                    Quantity = item.Quantity,
                    PartyName = _context.Parties.Find(item.PartyPartyId)?.PartyName,
                    ProductName = _context.Products.Find(item.ProductProductId)?.ProductName,
                    Date = item.Date,
                    Total = item.Quantity * item.CurrentRate
                });
            }
           
            return Ok(list);
        }

    }
}
