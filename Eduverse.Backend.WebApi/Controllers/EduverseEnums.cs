using System.ComponentModel;

namespace Eduverse.Backend.WebApi.Controllers
{

    public enum InstitutionalStatus
    {
        [Description("Active")]
        Active,
        [Description("Blocked")]
        Blocked,
        [Description("Inactive")]
        Inactive,
        [Description("Total")]
        Total,
        [Description("Query")]
        Query,
        [Description("Withdrawn")]
        Withdrawn,
        [Description("Rejected")]
        Rejected,
        [Description("Activate")]
        Activate
    }


}
