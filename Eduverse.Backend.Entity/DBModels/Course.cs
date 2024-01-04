using System;
using System.Collections.Generic;

namespace Eduverse.Backend.Entity.DBModels;

public partial class Course
{
    public int SubjectId { get; set; }

    public string SubjectName { get; set; } = null!;

    public int? FieldId { get; set; }

    public virtual FieldsOfStudy? Field { get; set; }
}
