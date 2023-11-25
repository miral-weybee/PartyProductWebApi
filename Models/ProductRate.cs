using System;
using System.Collections.Generic;

namespace PartyProductWebApi.Models;

public partial class ProductRate
{
    public int Id { get; set; }

    public int Rate { get; set; }

    public DateTime DateOfRate { get; set; }

    public int ProductNameProductId { get; set; }

    public virtual Product ProductNameProduct { get; set; } = null!;
}
