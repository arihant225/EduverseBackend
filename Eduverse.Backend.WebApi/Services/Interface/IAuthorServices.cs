using Eduverse.Backend.WebApi.Models.Request;
using Eduverse.Backend.WebApi.Models.Response;

namespace Eduverse.Backend.WebApi.Services.Interface
{
    public interface IAuthorService
    {
        public Task<SortedList<string, int>> GetStats();

        public Task<List<Inquery>> SearchInstitute(string institituteType);
    }
}
