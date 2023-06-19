namespace Eduverse.Backend.WebApi.Models.Request
{
    public class Otp
    {
        public string Id { get; set; }  
        public string? UserName { get; set; } 
        
        public int? code { get; set; }
        public string? Password { get; set; } 

        public string? Method { get; set; }
        public DateTime? Time { get; set; }
        public int? RequestedOtp { get; set; }
    }
}
