using Eduverse.Backend.Entity.DBModels;
using Eduverse.Backend.Entity.Repository;
using Eduverse.Backend.WebApi.Controllers;
using Eduverse.Backend.WebApi.Models.Request;
using Eduverse.Backend.WebApi.Services.Interface;
using System.Net.Mail;
using System.Net;
using Eduverse.Backend.Entity.PropertyClasses;
using Microsoft.EntityFrameworkCore;

namespace Eduverse.Backend.WebApi.Services
{
    public class InqueryService: IInqueryService
    {
        public readonly IEduverseRepository _repo;
        public readonly IMailService _mailService;  
        public InqueryService(IEduverseRepository repo,IMailService _mailService) {
            this._mailService = _mailService;
            this._repo = repo;
        }
        
        public async Task<string?> AddInquery (Inquery inquery){
            if(inquery==null) return null; 

            string path= UploadDownloadService.upload(inquery.Img);
            RegisterdInstitute institute= new RegisterdInstitute();     
            institute.InstituteType = inquery.InstituteType;    
            institute.InstituteName = inquery.InstituteName;   
            institute.PhoneNo=inquery.PhoneNo;
            institute.Comment = inquery.Comment;
            institute.Logo = path;
            institute.Email = inquery.EmailId;
            institute.Url = inquery.Url;    
            institute.Guidaccessor =  Guid.NewGuid().ToString();
            institute.Status = InstitutionalStatus.Query.ToString();
        
            await _repo.Context.RegisterdInstitutes.AddAsync(institute);   
            await _repo.Context.SaveChangesAsync();
            this.SendCopyOfProposal(institute);
            return institute.Guidaccessor;    
        }

        public int SendCopyOfProposal(RegisterdInstitute institute)
        {
            string body= $"Hello,\n\n"
                + $"This is to confirm that your inquiry to Eduverse has been successfully Submitted.\n\n"
                + $"Institute Name: {institute.InstituteName}\n"
                + $"Institute Type: {institute.InstituteType}\n"
                + $"Email: {institute.Email}\n"
                + $"Phone Number: {institute.PhoneNo}\n"
                + $"Inquery Number: {institute.Guidaccessor}\n\n"
                + $"Thank you for choosing Eduverse!\n\n"
                + $"Best regards,\nThe Eduverse Team";
            string subject= $"proposal Submitted : {institute.InstituteName}";
            string? sender = institute.Email;
            return _mailService.sendMail(sender, subject, body);
        }

        public async  Task<Inquery?> SearchProposal(string accessor)
        {
             RegisterdInstitute? inst= await _repo.Context.RegisterdInstitutes.FirstOrDefaultAsync(inst=>inst.Guidaccessor == accessor);      
            if (inst == null) return null;
            Inquery? inquery = new()
            {
                Accessor = accessor,
                Status = inst.Status,
                Path = inst.Logo,
                Comment = inst.Comment,
                EmailId = inst.Email == null ? "" : inst.Email,
                PhoneNo = inst.PhoneNo == null ? "" : inst.PhoneNo,
                InstituteName = inst.InstituteName,
                InstituteType = inst.InstituteType == null ? "" : inst.InstituteType,
                Url = inst.Url,

            };
            return inquery; 
        }

        public Task<bool> validProposal(string accessor, string Id)
        {
         return   this._repo.Context.RegisterdInstitutes.AnyAsync(obj => obj.Guidaccessor == accessor && obj.Email == Id);
        }
        public async Task<bool> ModifyProposal(InstitutionalStatus status ,string proposal)
            

        {
            RegisterdInstitute? inst = await this._repo.Context.RegisterdInstitutes.FirstOrDefaultAsync(inst => inst.Guidaccessor == proposal);

            switch (status) {

                case InstitutionalStatus.Withdrawn:
                    if(inst!=null)
                    {
                        inst.Status = InstitutionalStatus.Withdrawn.ToString();
                         this._repo.Context.RegisterdInstitutes.Update(inst);
                        await this._repo.Context.SaveChangesAsync();
                        return true;
                    }
                    break;


            }

            return false;
        }

    }
}
