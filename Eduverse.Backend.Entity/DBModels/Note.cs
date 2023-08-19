using System;
using System.Collections.Generic;

namespace Eduverse.Backend.Entity.DBModels;

public partial class Note
{
    public long NotesId { get; set; }

    public string? Title { get; set; }

    public string? Body { get; set; }

    public string? TitleStyle { get; set; }

    public string? BodyStyle { get; set; }

    public string? UserId { get; set; }

    public bool? IsPrivate { get; set; }

    public virtual ICollection<SubItem> SubItems { get; set; } = new List<SubItem>();

    public virtual Credential? User { get; set; }
}
