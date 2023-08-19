using System;
using System.Collections.Generic;

namespace Eduverse.Backend.Entity.DBModels;

public partial class Folder
{
    public int FolderId { get; set; }

    public string FolderName { get; set; } = null!;

    public string? Userid { get; set; }

    public virtual ICollection<SubItem> SubItemFolders { get; set; } = new List<SubItem>();

    public virtual ICollection<SubItem> SubItemLinkedFolders { get; set; } = new List<SubItem>();

    public virtual Credential? User { get; set; }
}
