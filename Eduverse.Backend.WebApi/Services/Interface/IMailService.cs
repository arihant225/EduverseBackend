namespace Eduverse.Backend.WebApi.Services.Interface
{
    public interface IMailService
    {

        public int sendMail(string? to, string subject, string body);
    }
}
