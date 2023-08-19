using Eduverse.Backend.WebApi.Services.Interface;
using Eduverse.Backend.Entity.DBModels;
using Eduverse.Backend.Entity.Repository;
using Eduverse.Backend.WebApi.Models.Request;
using System.Reflection.Metadata.Ecma335;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Runtime.Serialization.Formatters;

namespace Eduverse.Backend.WebApi.Services
{
    public class DirectoryService:IDirectoryService
    {
        public  readonly IEduverseRepository repo;
        public DirectoryService(IEduverseRepository _repo) { 
        repo= _repo;    
        }

        public async Task<EduverseDirectory?> SaveFolder(EduverseDirectory dto,string emailId,decimal? phoneNo) { 

            Folder? Entity=new Folder();
            Entity.FolderId = dto.FolderId;
            Entity.FolderName = dto.FolderName; 
       
            string? userId= await repo.userId(emailId, phoneNo.GetValueOrDefault());

                if (userId == null ) {
                    return null;
                }

                Entity.Userid= userId;  
 
             Entity=await this.repo.SaveFolder(Entity, userId,dto.ParentFolderId);
            if(Entity!=null)
             dto.FolderId = Entity.FolderId;
            return dto;
        }


        public async Task<AllItems?> OpenFolder(int? folderId, string emailId, decimal? phoneNo)
        {
            AllItems dtoDirectory = new();
            string? userId = await repo.userId(emailId, phoneNo.GetValueOrDefault());
            if (userId == null)
            {
                return null;
            }
            else
            {
                List<SubItem> items=await this.repo.GetSubItems(folderId);
                if (items!=null&&items.Any()) {
                   
                    dtoDirectory.IsolatedItemsNote = new();
                    List<EduverseDirectory> dto = new();
                    foreach(SubItem item in items)
                    if (item.FolderId != null&&item.Folder!=null)
                    {
                        
                            EduverseDirectory directory = new EduverseDirectory();
                            directory.FolderName = item.Folder.FolderName;
                            directory.FolderId = item.Folder.FolderId;
                            dto.Add(directory);
                        
                    }
                    else if(item.NoteId != null && item.Note != null)
                    {
                            NoteItems notes = new NoteItems();  
                            notes.NotesId=item.Note.NotesId;
                            notes.Title= item.Note.Title;  
                            dtoDirectory.IsolatedItemsNote.Add(notes);  


                    }
                    dtoDirectory.Directories = dto;

                }


            }

            return dtoDirectory;
        }

        public async Task<AllItems?> GetDirectory(string emailId, decimal? phoneNo) {
                return await this.OpenFolder(null,emailId,phoneNo);
}
           

            

        }
    }
