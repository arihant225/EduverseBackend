using System;
using System.Collections.Generic;

namespace Eduverse.Backend.Entity.DBModels;

public partial class TempOtp
{
    public string Id { get; set; } = null!;

    public string? Method { get; set; }

    public decimal? Otp { get; set; }

    public DateTime? GeneratedTimeStamp { get; set; }
}
