using System;
using System.Collections.Generic;

namespace Eduverse.Backend.Entity.DBModels;

public partial class InstitutionalDomain
{
    public int DomainId { get; set; }

    public string? Status { get; set; }

    public int? ParentDomainId { get; set; }

    public string? DomainName { get; set; }

    public int? InstituteId { get; set; }

    public virtual RegisterdInstitute? Institute { get; set; }

    public virtual ICollection<InstitutionalDomain> InverseParentDomain { get; set; } = new List<InstitutionalDomain>();

    public virtual InstitutionalDomain? ParentDomain { get; set; }
}
