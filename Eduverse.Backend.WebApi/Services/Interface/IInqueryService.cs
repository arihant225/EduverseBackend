using Eduverse.Backend.WebApi.Models.Request;

namespace Eduverse.Backend.WebApi.Services.Interface
{
    public interface IInqueryService
    {
        public Task<string?> AddInquery(Inquery inquery);
        public Task<Inquery?> SearchProposal(string accessor);
    }
}
