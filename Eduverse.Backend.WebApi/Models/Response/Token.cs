namespace Eduverse.Backend.WebApi.Models.Response
{
    public class Token
    {
        public string JWTToken { get; set; }
        public string username { get; set; }
        public DateTime expiration { get; set; }
        public string Email { get; set; }

    }
}
