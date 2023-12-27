﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PartyProductWebApi.Models;
using System.Diagnostics.Metrics;

namespace PartyProductWebApi.Controllers
{
    [Route("/Invoice")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly EvaluationTaskDbContext _context;

        public InvoiceController(EvaluationTaskDbContext context)
        {
            this._context = context;

        }

        [HttpPost]
        public async Task<ActionResult<InvoiceAddDTO>> GenerateInvoice([FromBody] Invoice invoiceAdd)
        {
            if (invoiceAdd == null)
                return BadRequest();

            _context.Invoices.Add(new Invoice
            {
                PartyId = invoiceAdd.PartyId,
                Date = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
            });

            await _context.SaveChangesAsync();


            foreach (var item in invoiceAdd.Invoiceproducts)
            {
                var invoiceIteam = new Invoiceproduct
                {
                    Invoiceid = item.Invoiceid,
                    Productid = item.Productid,
                    Qty = item.Qty,
                    Rate = item.Rate
                };
                _context.Invoiceproducts.Add(invoiceIteam);
            }

            await _context.SaveChangesAsync();
            return Ok(invoiceAdd);
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
                    PartyId = item.PartyId,
                    PartyName = GetPartyName(item.PartyId),
                    Date = (DateOnly)item.Date

                });
            }

            string GetPartyName(int partyId)
            {
                var party = _context.Parties.FirstOrDefault(p => p.PartyId == partyId);
                return party?.PartyName ?? "Unknown";
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
        public async Task<ActionResult> deleteinvoice([FromRoute] int id)
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

        [HttpGet("InvoiceProductRate/{Id}")]
        public async Task<ActionResult<decimal>> GetInvoiceProductRate(int Id)
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
