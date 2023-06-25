using Eduverse.Backend.WebApi.Models.Request;

namespace Eduverse.Backend.WebApi.Services.Interface
{
    public interface ILoginService
    {
        public string? GenerateToken(Login login);
    }
}
