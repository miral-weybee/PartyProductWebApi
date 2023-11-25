using System;
using System.Collections.Generic;

namespace PartyProductWebApi.Models;

public partial class AssignParty
{
    public int AssignPartyId { get; set; }

    public int PartyPartyId { get; set; }

    public int ProductProductId { get; set; }

    public virtual Party PartyParty { get; set; } = null!;

    public virtual Product ProductProduct { get; set; } = null!;
}
