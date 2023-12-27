using System;
using System.Collections.Generic;

namespace PartyProductWebApi.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public virtual ICollection<AssignParty> AssignParties { get; set; } = new List<AssignParty>();

    public virtual ICollection<Invoiceproduct> Invoiceproducts { get; set; } = new List<Invoiceproduct>();

    public virtual ICollection<ProductRate> ProductRates { get; set; } = new List<ProductRate>();
}
