using Eduverse.Backend.WebApi.Models.Request;

namespace Eduverse.Backend.WebApi.Services.Interface
{
    public interface IInqueryService
    {
        public Task<bool> AddInquery(Inquery inquery);
    }
}
