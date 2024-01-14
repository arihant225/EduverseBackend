using Eduverse.Backend.Entity.DBModels;
using Eduverse.Backend.Entity.Repository;
using Eduverse.Backend.WebApi.Models.Response;
using Eduverse.Backend.WebApi.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace Eduverse.Backend.WebApi.Services
{
    public class AdminService : IAdminService
    {
        private readonly IEduverseRepository _eduverseRepo;
        public AdminService(IEduverseRepository repo) {
            _eduverseRepo = repo;

        }

        public async Task<List<Domain>?> GetAllDomains(int? id,string? accessor)
        {
            if(string.IsNullOrWhiteSpace(accessor))
            {
                return null;
            }
            else
            {
                int? instId = await _eduverseRepo.InstitutionalId(accessor);
                if(instId == null) {
                    return null; 
                }
                else 
                {
                    return await GetDomains(id, instId.GetValueOrDefault());
                }

            }

        }
        public async Task<List<Domain>> GetDomains(int? id,int instId)
        {

            List<InstitutionalDomain> domains = await _eduverseRepo.Context.InstitutionalDomains.Where(domain => domain.ParentDomainId == id && domain.Status == "Active"&&domain.InstituteId==instId).ToListAsync();
            List<Domain> domainsDto = new();
            foreach (var obj in domains)
            {
                domainsDto.Add(
                new Domain()
                {
                    domainId = obj.DomainId,
                    domainName = obj.DomainName ?? ""
                }
                );
            };

            foreach (Domain domain in domainsDto)
            {
                domain.SubDomains.AddRange(await GetDomains(domain.domainId,instId));
            }
           domainsDto= domainsDto.OrderBy(domain => domain.domainName).ToList();   

            return domainsDto;

        }

        public async Task<Domain> UpdateOrAddDomain(Domain domain,string accessor)
        {
            if( domain.domainId==0)
            {
                InstitutionalDomain DomainEntity = new()
                {
                    DomainId = domain.domainId,
                    DomainName = domain.domainName,
                    ParentDomainId = domain.parentDomainId,
                    InstituteId = await this._eduverseRepo.InstitutionalId(accessor),
                    Status="Active"

                };
                this._eduverseRepo.Context.InstitutionalDomains.Add(DomainEntity);

                this._eduverseRepo.Context.SaveChanges();
                domain.domainId = DomainEntity.DomainId;

            }
             var domainEntity=await this._eduverseRepo.Context.InstitutionalDomains.FirstOrDefaultAsync(obj => obj.DomainId == domain.domainId);
            if (domainEntity != null)
            {
                domainEntity.DomainName = domain.domainName ?? "";
                domainEntity.Status = domain.Status ?? domainEntity.Status;
                _eduverseRepo.Context.InstitutionalDomains.Update(domainEntity);
                await _eduverseRepo.Context.SaveChangesAsync();
                return domain;
            }
            
            return domain;

        }


       


    }
}