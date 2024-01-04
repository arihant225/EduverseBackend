using System;
using System.Collections.Generic;

namespace Eduverse.Backend.Entity.DBModels;

public partial class Stream
{
    public long Orginisationid { get; set; }

    public string EduverseId { get; set; } = null!;

    public string Orginisationname { get; set; } = null!;

    public int Orginisationtype { get; set; }

    public byte? Public { get; set; }

    public byte[]? Image { get; set; }

    public string? Orginisationdescription { get; set; }

    public byte Paid { get; set; }

    public decimal Price { get; set; }

    public virtual Credential Eduverse { get; set; } = null!;

    public virtual Subgenre OrginisationtypeNavigation { get; set; } = null!;
}
