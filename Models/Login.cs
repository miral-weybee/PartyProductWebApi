using System;
using System.Collections.Generic;

namespace PartyProductWebApi.Models;

public partial class Login
{
    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int Uid { get; set; }
}
