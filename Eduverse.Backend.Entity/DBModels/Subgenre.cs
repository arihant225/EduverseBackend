using System;
using System.Collections.Generic;

namespace Eduverse.Backend.Entity.DBModels;

public partial class Subgenre
{
    public int SubgenreId { get; set; }

    public string SubgenreName { get; set; } = null!;

    public int? GenreId { get; set; }

    public virtual Genre? Genre { get; set; }
}
