using System;
using System.Collections.Generic;

namespace Eduverse.Backend.Entity.DBModels;

public partial class SmtpMailCredential
{
    public int SmtpMailCredentialsId { get; set; }

    public string Role { get; set; } = null!;

    public string EmailId { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int? Port { get; set; }

    public string? Server { get; set; }
}
