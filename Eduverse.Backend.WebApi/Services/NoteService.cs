using Eduverse.Backend.Entity.DBModels;
using Eduverse.Backend.Entity.Repository;
using Eduverse.Backend.WebApi.Models.Request;
using Eduverse.Backend.WebApi.Services.Interface;
using System.Globalization;

namespace Eduverse.Backend.WebApi.Services
{
    public class NoteService : INotesService
    {
        private readonly IEduverseRepository eduverseRepository;
        public NoteService(IEduverseRepository repository)
        {
            eduverseRepository = repository;
        }

        public async Task<Notes?> getNotesById(long id,string accessor) {

            string? userId =await this.eduverseRepository.userId(accessor);
            if(userId == null) {
                return null;
            }

           Note?Entity= await this.eduverseRepository.GetNotes(id,userId);
            if (Entity == null)
            {
                return null;
            }
            Notes notes = new Notes();
            notes.NotesId = Entity.NotesId;
            notes.IsPrivate = Entity.IsPrivate; 
            notes.Body = Entity.Body;   
            notes.BodyStyle=Entity.BodyStyle;
            notes.Title = Entity.Title; 
            notes.isAuthorize=Entity.UserId == userId;  
            notes.TitleStyle=Entity.TitleStyle;
            return notes;

            

        
        }
        public async Task<Notes?> SaveNotes(string accessor, Notes dto)
        {
            Note? Entity = new();
            Entity.NotesId=dto.NotesId.GetValueOrDefault();
            Entity.Title = dto.Title;
            Entity.TitleStyle= dto.TitleStyle;
            Entity.Body = dto.Body;
            Entity.BodyStyle= dto.BodyStyle;    
            Entity.IsPrivate = dto.IsPrivate;
            Entity=await this.eduverseRepository.SaveNotes(accessor,Entity,dto.ParentFolderId);
            if(Entity!=null)
            dto.NotesId=Entity.NotesId ;
            if(Entity==null)
            return null;
            return dto;

        
        }
    }
}
