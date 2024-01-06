using System.ComponentModel.DataAnnotations;

namespace Eduverse.Backend.WebApi.Models.Request
{
    public class Inquery
    {
        [Required(ErrorMessage = "Institute name is required")]
        public string InstituteName { get; set; }

        [Required(ErrorMessage = "Institute type is required")]
        public string InstituteType { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string EmailId { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        public string PhoneNo { get; set; }
        public string? Status { get; set; }
        public string? Comment { get; set; }
        public string? Url { get; set; }
        [Required(ErrorMessage = "Img  is required")]
        public IFormFile? Img { get; set; }
        public string? Path { get; internal set; }
        public string? Accessor { get; internal set; }
    }
}
