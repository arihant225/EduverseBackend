using System;
using System.Collections.Generic;

namespace Eduverse.Backend.Entity.DBModels;

public partial class EduverseRoles
{
    public long EduverseRoleId { get; set; }

    public string EduverseId { get; set; } = null!;

    public string EduverseRole { get; set; } = null!;

    public virtual Credential Eduverse { get; set; } = null!;

    
}
