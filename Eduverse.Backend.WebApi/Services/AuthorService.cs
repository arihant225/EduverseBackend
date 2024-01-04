using Eduverse.Backend.Entity.DBModels;
using Eduverse.Backend.Entity.Repository;
using Eduverse.Backend.WebApi.Controllers;
using Eduverse.Backend.WebApi.Models.Response;
using Eduverse.Backend.WebApi.Services.Interface;
using System.Runtime.Serialization.Formatters;

namespace Eduverse.Backend.WebApi.Services
{
    public class AuthorService : IAuthorService
    {
        public readonly IEduverseRepository _repository ;
        public AuthorService( IEduverseRepository repository)
        {
            this._repository = repository ; 
            
        }
        public async Task<SortedList<string,int>> GetStats()
        {
          SortedList<string, int> dictionary = new SortedList<string, int>();
            dictionary.Add(InstitutionalStatus.Inactive.ToString(), 0);
            dictionary.Add(InstitutionalStatus.Active.ToString(), 0);
            dictionary.Add(InstitutionalStatus.Total.ToString(), 0);
            dictionary.Add(InstitutionalStatus.Blocked.ToString(), 0);
            dictionary.Add(InstitutionalStatus.NewRequests.ToString(), 0);
            List<RegisterdInstitute> institutes = await _repository.GetAllInstitutes();
            institutes.ForEach(inst =>
                                {
                          dictionary[inst.Status]++;
                          dictionary[InstitutionalStatus.Total.ToString()]++;
                                });
            return dictionary;  


        }
    }
}
