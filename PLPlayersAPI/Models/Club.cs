using System;
using System.Collections.Generic;

namespace PLPlayersAPI.Models;

public partial class Club
{
    public int ClubId { get; set; }

    public string? Name { get; set; }

    public string? BadgeSrc { get; set; }

    public virtual ICollection<Player> Players { get; set; } = new List<Player>();
}
