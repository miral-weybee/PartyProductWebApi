using System;
using System.Collections.Generic;

namespace PartyProductWebApi.Models;

public partial class Invoiceproduct
{
    public int? Productid { get; set; }

    public int? Qty { get; set; }

    public int? Rate { get; set; }

    public int Id { get; set; }

    public int? Invoiceid { get; set; }

    public virtual Invoice? Invoice { get; set; }

    public virtual Product? Product { get; set; }
}
