using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduverse.Backend.Entity.Enums
{
    public enum OtpEnums
    {
        Generated,
        Expired,
        Success,
        NotGenerated,
        InvalidMail,
        ServicesNotAvailable,
        InvalidOtp
    }
}
