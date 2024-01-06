using System;
using System.Collections.Generic;

namespace Eduverse.Backend.Entity.DBModels;

public partial class RegisterdInstitute
{
    public int InstitutitionalId { get; set; }

    public string InstituteName { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string? Logo { get; set; }

    public string? InstituteType { get; set; }

    public string? Url { get; set; }

    public string? PhoneNo { get; set; }

    public string? Comment { get; set; }

    public string? Guidaccessor { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<Credential> Credentials { get; set; } = new List<Credential>();
}
