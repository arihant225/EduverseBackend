using System;
using System.Collections.Generic;

namespace Eduverse.Backend.Entity.DBModels;

public partial class Credential
{
    public string EduverseId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string EmailId { get; set; } = null!;

    public decimal PhoneNumber { get; set; }

    public string Password { get; set; } = null!;

    public string? Role { get; set; }
}
