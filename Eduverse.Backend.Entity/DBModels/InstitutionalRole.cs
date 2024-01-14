using System;
using System.Collections.Generic;

namespace Eduverse.Backend.Entity.DBModels;

public partial class InstitutionalRole
{
    public int RoleId { get; set; }

    public string? EduverseId { get; set; }

    public int? InstitutionalId { get; set; }

    public string? RoleType { get; set; }

    public virtual Credential? Eduverse { get; set; }

    public virtual RegisterdInstitute? Institutional { get; set; }
}
