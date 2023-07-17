using Eduverse.Backend.Entity.DBModels;

namespace Eduverse.Backend.WebApi.Models.Response
{
    public class Streams
    {
        public long StreamerId { get; set; }

        public string EduverseId { get; set; } = null!;

        public string StreamerName { get; set; } = null!;

        public int StreamerType { get; set; }

        public byte? Public { get; set; }

        public byte[]? Image { get; set; }

        public string? StreamerDescription { get; set; }

        public Boolean Paid { get; set; }

        public decimal Price { get; set; }
        
    }
}
