using Eduverse.Backend.WebApi.Models.Request;

namespace Eduverse.Backend.WebApi.Services.Interface
{
    public interface IDirectoryService
    {
        public Task<AllItems?> GetDirectory(string accessor);
        public Task<EduverseDirectory?> SaveFolder(EduverseDirectory dto, string accessor);
        public  Task<AllItems?> OpenFolder(int? folderId, string accessor);
        public  Task<bool> DeleteFolder(int? folderId, string accessor);
    }
}
