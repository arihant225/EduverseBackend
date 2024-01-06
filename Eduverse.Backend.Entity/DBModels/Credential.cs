using System;
using System.Collections.Generic;

namespace Eduverse.Backend.Entity.DBModels;

public partial class Credential
{
    public string EduverseId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? EmailId { get; set; }

    public decimal? PhoneNumber { get; set; }

    public string Password { get; set; } = null!;

    public string? Role { get; set; }

    public int? InstitutitionalId { get; set; }

    public virtual ICollection<EduverseRole> EduverseRoles { get; set; } = new List<EduverseRole>();

    public virtual ICollection<Folder> Folders { get; set; } = new List<Folder>();

    public virtual RegisterdInstitute? Institutitional { get; set; }

    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();
}
