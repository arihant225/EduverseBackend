using Eduverse.Backend.WebApi.Models.Request;
using Eduverse.Backend.WebApi.Models.Response;

namespace Eduverse.Backend.WebApi.Services.Interface
{
    public interface ILoginService
    {
        public Token? GenerateToken(Login login);
    }
}
