using Eduverse.Backend.WebApi.Models.Request;

namespace Eduverse.Backend.WebApi.Services.Interface
{
    public interface INotesService
    {
        public Task<Notes?> SaveNotes(string accessor, Notes dto);
        public  Task<Notes?> getNotesById(long id, string accessor);
    }
}
