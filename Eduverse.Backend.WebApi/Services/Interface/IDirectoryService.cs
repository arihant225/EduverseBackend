using Eduverse.Backend.WebApi.Models.Request;

namespace Eduverse.Backend.WebApi.Services.Interface
{
    public interface IDirectoryService
    {
        public Task<AllItems?> GetDirectory(string emailId, decimal? phoneNo);
        public Task<EduverseDirectory?> SaveFolder(EduverseDirectory dto, string emailId, decimal? phoneNo);
    }
}
