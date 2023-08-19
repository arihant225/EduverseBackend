using System;
using System.Collections.Generic;

namespace Eduverse.Backend.Entity.DBModels;

public partial class SubItem
{
    public long ItemId { get; set; }

    public int? LinkedFolderId { get; set; }

    public long? NoteId { get; set; }

    public int? FolderId { get; set; }

    public virtual Folder? Folder { get; set; }

    public virtual Folder? LinkedFolder { get; set; }

    public virtual Note? Note { get; set; }
}
