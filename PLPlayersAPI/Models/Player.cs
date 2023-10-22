using System;
using System.Collections.Generic;

namespace PLPlayersAPI.Models;

public partial class Player
{
    public int PlayerId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? ImgSrc { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public int? NationalityId { get; set; }

    public int? ClubId { get; set; }

    public int? PositionId { get; set; }

    public virtual Club? Club { get; set; }

    public virtual Nationality? Nationality { get; set; }

    public virtual Position? Position { get; set; }
}
