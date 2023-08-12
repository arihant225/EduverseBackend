namespace Eduverse.Backend.WebApi.Models.Request
{
    public class Notes
    {

        public long? NotesId { get; set; }

        public string? Title { get; set; }

        public string? Body { get; set; }

        public string? TitleStyle { get; set; }

        public string? BodyStyle { get; set; }

        public bool? isAuthorize { get; set; }

        public bool? IsPrivate { get; set; }
    }
}
