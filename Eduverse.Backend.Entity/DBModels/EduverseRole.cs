using System;
using System.Collections.Generic;

namespace Eduverse.Backend.Entity.DBModels;

public partial class EduverseRole
{
    public long RoleId { get; set; }

    public string EduverseId { get; set; } = null!;

    public string Role { get; set; } = null!;

    public virtual Credential Eduverse { get; set; } = null!;
}
