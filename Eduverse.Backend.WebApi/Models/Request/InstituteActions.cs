using Eduverse.Backend.WebApi.Controllers;

namespace Eduverse.Backend.WebApi.Models.Request
{
    public class InstituteActions
    {
        public List<string>? proposals { get; set; }
        public List<Inquery> Inqueries { get; set; } 
        public string Action { get; set; }

    }
}
