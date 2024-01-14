using Eduverse.Backend.WebApi.Controllers;
using Eduverse.Backend.WebApi.Models.Request;

namespace Eduverse.Backend.WebApi.Services.Interface
{
    public interface IInqueryService
    {
        public Task<string?> AddInquery(Inquery inquery);
        public Task<Inquery?> SearchProposal(string accessor);
         
        public Task<bool> validProposal(string accessor,string Id);
        public Task<bool> ModifyProposal(InstitutionalStatus status, string proposal);
    }
}
