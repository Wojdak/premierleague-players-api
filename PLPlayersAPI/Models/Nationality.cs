using System;
using System.Collections.Generic;

namespace PLPlayersAPI.Models;

public partial class Nationality
{
    public int NationalityId { get; set; }

    public string Country { get; set; } = null!;

    public string? FlagSrc { get; set; }

    public virtual ICollection<Player> Players { get; set; } = new List<Player>();
}
