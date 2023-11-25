using System;
using System.Collections.Generic;

namespace PartyProductWebApi.Models;

public partial class Invoice
{
    public int Id { get; set; }

    public int CurrentRate { get; set; }

    public int Quantity { get; set; }

    public int PartyPartyId { get; set; }

    public int ProductProductId { get; set; }

    public DateOnly? Date { get; set; }

    public virtual Party PartyParty { get; set; } = null!;

    public virtual Product ProductProduct { get; set; } = null!;
}
