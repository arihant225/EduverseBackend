namespace Eduverse.Backend.WebApi.Models.Request
{

    public class AllItems {
       public List<EduverseDirectory>? Directories { get; set; }

        public List<NoteItems>? IsolatedItemsNote { get; set; }

    }
    public class EduverseDirectory
    {
        public int FolderId { get; set; }
        public string FolderName { get; set; } = null!;
        public List<NoteItems>? Notes { get; set; } = null!;

        public int? ParentFolderId { get; set; }  
    }

    public class NoteItems{
        
        public long? NotesId { get; set; }
        
        public string? Title { get; set; }
    }
}
