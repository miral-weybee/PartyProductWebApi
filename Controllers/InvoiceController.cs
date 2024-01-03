using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PartyProductWebApi.Models;


namespace PartyProductWebApi.Controllers
{
    [Authorize]
    [Route("Invoice")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly EvaluationTaskDbContext _context;
       
        public InvoiceController(EvaluationTaskDbContext context)
        {
            this._context = context;  
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] InvoiceAddDTO invoiceCreate)
        {
            var invoice = new Invoice
            {
                PartyId = invoiceCreate.PartyId,
                Date = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
            };
            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            foreach (var item in invoiceCreate.Products)
            {
                var invoiceIteam = new Invoiceproduct
                {
                    Invoiceid = invoice.InvoiceId,
                    Productid = item.ProductId,
                    Rate = (int)item.Rate,
                    Qty = item.Quantity
                };
                _context.Invoiceproducts.Add(invoiceIteam);
            }
            await _context.SaveChangesAsync();

            return Ok(invoice);

        }

        [HttpGet]
        public async Task<ActionResult<List<InvoiceDTO>>> GetInvoices()
        {
            var list = new List<InvoiceDTO>();
            var invoiceList = await _context.Invoices.ToListAsync();


            foreach (var item in invoiceList)
            {
                list.Add(new InvoiceDTO()
                {
                    InvoiceId = item.InvoiceId,
                    PartyName = GetPartyName(item.PartyId),
                    Date = (DateOnly)item.Date
                });
            }

            string GetPartyName(int partyId)
            {
                var party = _context.Parties.FirstOrDefault(p => p.PartyId == partyId);
                return party?.PartyName ?? "N/A";
            }

            return Ok(list);
        }

        [HttpGet("date")]
        public async Task<ActionResult> GetInvoiceListByDate([FromQuery] DateOnly startdate,[FromQuery] DateOnly enddate)
        {
            var list = new List<InvoiceDTO>();
            var result = await _context.Invoices.Where(x => x.Date >= startdate && x.Date <= enddate).ToListAsync();
            if (!result.Any())
            {
                return NotFound();
            }

            foreach(var item in result)
            {
                list.Add(new InvoiceDTO
                {
                    InvoiceId = item.InvoiceId,
                    PartyName = _context.Parties.Find(item.PartyId).PartyName,
                    Date = (DateOnly)item.Date
                });
            }
            return Ok(list);
        }

        [HttpGet("InvoiceProducts/{Id}")]
        public async Task<ActionResult<List<ProductDTO>>> GetProductsForDropdown(int Id)
        {
            var productsForParty = await (from product in _context.Products
                                          join productRate in _context.ProductRates on product.ProductId equals productRate.ProductNameProductId
                                          join assignParty in _context.AssignParties on product.ProductId equals assignParty.ProductId
                                          where assignParty.PartyId == Id
                                          select new
                                          {
                                              product.ProductId,
                                              product.ProductName
                                          })
                                         .Distinct()
                                         .ToListAsync();

            var productDTOs = productsForParty
                .Select(p => new ProductDTO
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName
                })
                .ToList();

            return productDTOs;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Deleteinvoice([FromRoute] int id)
        {
            var invoice = _context.Invoices.SingleOrDefault(x => x.InvoiceId == id);
            if (invoice == null)
                return NotFound();

            _context.Invoices.Remove(invoice);
            var invoiceproducts = _context.Invoiceproducts.Where(x => x.Invoiceid== id);
            foreach (var product in invoiceproducts)
            {
                _context.Invoiceproducts.Remove(product);
            }
            await _context.SaveChangesAsync();
            return Ok("invoice deleted successfully..");
        }

        [HttpPut("edit/{id}")]
        public async Task<ActionResult> Editinvoice([FromRoute] int id, [FromBody] InvoiceProductEditDTO invoiceproductresponse)
        {
            
            var invoiceproducts = _context.Invoiceproducts.Where(x => x.Invoiceid == id);
            if (invoiceproducts == null)
                return NotFound();

            foreach(var item in invoiceproducts)
            {
                _context.Invoiceproducts.Remove(item);
            }

            await _context.SaveChangesAsync();

            foreach (var item in invoiceproductresponse.Products)
            {
                var invoiceItem = new Invoiceproduct
                {
                    Invoiceid = invoiceproductresponse.InvoiceId,
                    Productid = item.ProductId,
                    Rate = (int)item.Rate,
                    Qty = item.Quantity
                };
                _context.Invoiceproducts.Add(invoiceItem);
            }
            await _context.SaveChangesAsync();
            return Ok("Invoice edited successfully..");
        }

        [HttpGet("InvoiceProductRate/{Id}")]
        public async Task<ActionResult<int>> GetInvoiceProductRate(int Id)
        {
            var latestRate = await _context.ProductRates
                .Where(pr => pr.ProductNameProductId == Id)
                .OrderByDescending(pr => pr.DateOfRate)
                .Select(pr => pr.Rate)
                .FirstAsync();

            return latestRate;
        }

        [HttpGet("{Id}")]

        public async Task<ActionResult<InvoiceGetDTO>> Get(int Id)
        {
            var invoiceQuery = from invoice in _context.Invoices
                               join party in _context.Parties on invoice.PartyId equals party.PartyId
                               join invoiceproducts in _context.Invoiceproducts on invoice.InvoiceId equals invoiceproducts.Invoiceid
                               join product in _context.Products on invoiceproducts.Productid equals product.ProductId
                               where invoiceproducts.Invoiceid == Id
                               group new { invoice, party, product, invoiceproducts } by invoice.InvoiceId into g
                               select new InvoiceGetDTO
                               {
                                   InvoiceId = g.Key,
                                   PartyId = g.First().invoice.PartyId,
                                   PartyName = g.First().party.PartyName,
                                   Date = (DateOnly)g.First().invoice.Date,
                                   Products = g.Select(item => new InvoiceProductsDTO
                                   {
                                       ProductId = item.product.ProductId,
                                       ProductName = item.product.ProductName,
                                       Quantity = (int)item.invoiceproducts.Qty,
                                       Rate = (int)item.invoiceproducts.Rate,
                                       Total = (int)(item.invoiceproducts.Qty * item.invoiceproducts.Rate)
                                   }).ToList()
                               };
            var finalinvoice = await invoiceQuery.FirstOrDefaultAsync();
            return finalinvoice;
        }

    }
}
