using System;
using System.Collections.Generic;

namespace Eduverse.Backend.Entity.DBModels;

public partial class Stream
{
    public long StreamerId { get; set; }

    public string EduverseId { get; set; } = null!;

    public string StreamerName { get; set; } = null!;

    public int StreamerType { get; set; }

    public byte? Public { get; set; }

    public byte[]? Image { get; set; }

    public string? StreamerDescription { get; set; }

    public byte Paid { get; set; }

    public decimal Price { get; set; }

    public virtual Credential Eduverse { get; set; } = null!;

    public virtual Subgenre StreamerTypeNavigation { get; set; } = null!;
}
