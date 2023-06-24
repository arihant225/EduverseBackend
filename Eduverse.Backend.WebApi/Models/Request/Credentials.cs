namespace Eduverse.Backend.WebApi.Models.Request
{
    public class Credentials
    {

        public string Name { get; set; } = null!;

        public string EmailId { get; set; } = null!;

        public decimal PhoneNumber { get; set; }

        public string Password { get; set; } = null!;

        public string? Role { get; set; }
    }
}
