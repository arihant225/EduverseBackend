namespace Eduverse.Backend.WebApi.Models.Response
{
    public class Domain
    {
        public int domainId { get; set; }   
        public string domainName { get; set; }  

        public List<Domain> SubDomains { get; set; }   =new List<Domain>(); 
        
        public string? Status { get; set; }
        public int? parentDomainId { get; set; }
    }
}
