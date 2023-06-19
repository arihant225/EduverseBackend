namespace Eduverse.Backend.WebApi.Models.Response
{
    public class Otp
    {
        public bool IsGenerate { get; set; }
        public int successCode { get; set; }
        public int AuthenticateCode { get; set; }
        public bool isAuthenticate { get; set; }
        public string? Message { set; get; }
    }
}
