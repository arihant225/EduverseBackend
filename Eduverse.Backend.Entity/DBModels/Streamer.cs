using System;
using System.Collections.Generic;

namespace Eduverse.Backend.Entity.DBModels;

public partial class Streamer
{
    public long Streamerid { get; set; }

    public string EduverseId { get; set; } = null!;

    public string Streamername { get; set; } = null!;

    public string Streamertype { get; set; } = null!;

    public byte? Private { get; set; }

    public byte[]? Image { get; set; }

    public string? Streamerdescription { get; set; }

    public virtual Credential Eduverse { get; set; } = null!;
}
