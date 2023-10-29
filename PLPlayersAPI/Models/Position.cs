using System;
using System.Collections.Generic;

namespace PLPlayersAPI.Models;

public partial class Position
{
    public int PositionId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Player> Players { get; set; } = new List<Player>();
}
