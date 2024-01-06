using Eduverse.Backend.Entity.DBModels;
using Eduverse.Backend.Entity.Repository;
using Eduverse.Backend.WebApi.Controllers;
using Eduverse.Backend.WebApi.Models.Request;
using Eduverse.Backend.WebApi.Models.Response;
using Eduverse.Backend.WebApi.Services.Interface;
using Microsoft.EntityFrameworkCore;

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
            dictionary.Add(InstitutionalStatus.Query.ToString(), 0);
            dictionary.Add(InstitutionalStatus.CredentialCreated.ToString(), 0);
            dictionary.Add(InstitutionalStatus.Withdrawn.ToString(), 0);
            List<RegisterdInstitute> institutes = await _repository.GetAllInstitutes();
            institutes.ForEach(inst =>
                                {
                          dictionary[inst.Status]++;
                          dictionary[InstitutionalStatus.Total.ToString()]++;
                                });
            return dictionary;  


        }

        public async Task<List<Inquery>> SearchInstitute(string institituteType)
        {
            if (institituteType == InstitutionalStatus.Total.ToString())
            {
                return await this._repository.Context.RegisterdInstitutes.Select(
                     obj => new Inquery()
                     {
                         InstituteName = obj.InstituteName,
                         InstituteType = obj.InstituteType == null ? "" : obj.InstituteType,
                         EmailId = obj.Email == null ? "" : obj.Email,
                         Path = obj.Logo,
                         Url = obj.Url,
                         Comment = obj.Comment,
                         Status = obj.Status,
                         Accessor = obj.Guidaccessor


                     }

            ).ToListAsync();
           } 

            else
            {
                return await this._repository.Context.RegisterdInstitutes.Where(inst => inst.Status == institituteType).Select(
                    obj =>
                        new Inquery()
                        {
                            InstituteName = obj.InstituteName,
                            InstituteType = obj.InstituteType == null ? "" : obj.InstituteType,
                            EmailId = obj.Email == null ? "" : obj.Email,
                            Path = obj.Logo,
                            Url = obj.Url,
                            Comment = obj.Comment,
                            Status = obj.Status,
                            Accessor = obj.Guidaccessor

                        }
                    ).ToListAsync();

            } 
        }
    }
}
