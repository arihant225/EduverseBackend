using System;
using System.Collections.Generic;

namespace Eduverse.Backend.Entity.DBModels;

public partial class FieldsOfStudy
{
    public int FieldId { get; set; }

    public string FieldName { get; set; } = null!;

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
