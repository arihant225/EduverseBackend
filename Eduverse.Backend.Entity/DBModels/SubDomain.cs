using System;
using System.Collections.Generic;

namespace Eduverse.Backend.Entity.DBModels;

public partial class SubDomain
{
    public int SubDomainId { get; set; }

    public string? DomainName { get; set; }

    public int? DomainId { get; set; }

    public virtual InstitutionalDomain? Domain { get; set; }
}
