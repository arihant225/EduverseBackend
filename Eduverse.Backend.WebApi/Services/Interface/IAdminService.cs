using Eduverse.Backend.WebApi.Models.Response;

namespace Eduverse.Backend.WebApi.Services.Interface
{
    public interface IAdminService
    {

        public  Task<List<Domain>?> GetAllDomains(int? id, string? accessor);
        public  Task<Domain> UpdateOrAddDomain(Domain domain, string accessor);
    }
}
