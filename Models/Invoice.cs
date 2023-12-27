using System;
using System.Collections.Generic;

namespace PartyProductWebApi.Models;

public partial class Invoice
{
    public int InvoiceId { get; set; }

    public int PartyId { get; set; }

    public DateOnly? Date { get; set; }

    public virtual ICollection<Invoiceproduct> Invoiceproducts { get; set; } = new List<Invoiceproduct>();
}
