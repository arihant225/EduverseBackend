using Eduverse.Backend.Entity.DBModels;
using Eduverse.Backend.Entity.Functionality;
using Eduverse.Backend.Entity.Repository;
using Eduverse.Backend.WebApi.Controllers;
using Eduverse.Backend.WebApi.Models.Request;
using Eduverse.Backend.WebApi.Models.Response;
using Eduverse.Backend.WebApi.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Eduverse.Backend.WebApi.Services
{
    public class AuthorService : IAuthorService
    {
        public readonly IEduverseRepository _repository ;

        public readonly IMailService _mailService;
        public AuthorService(IEduverseRepository repository, IMailService mailservice)
        {
            this._repository = repository ; 
            this._mailService = mailservice ;   
            
        }
        public async Task<SortedList<string,int>> GetStats()
        {
          SortedList<string, int> dictionary = new SortedList<string, int>();
            dictionary.Add(InstitutionalStatus.Inactive.ToString(), 0);
            dictionary.Add(InstitutionalStatus.Active.ToString(), 0);
            dictionary.Add(InstitutionalStatus.Total.ToString(), 0);
            dictionary.Add(InstitutionalStatus.Blocked.ToString(), 0);
            dictionary.Add(InstitutionalStatus.Query.ToString(), 0);
            dictionary.Add(InstitutionalStatus.Withdrawn.ToString(), 0);
            dictionary.Add(InstitutionalStatus.Rejected.ToString(), 0);
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

        public async Task ManageInstitutes(InstituteActions actions)
        {
        actions.Inqueries = new List<Inquery>();  
          if (actions.proposals == null)
                return;
            foreach (var proposal in actions.proposals) {
                {
                    RegisterdInstitute? inst = await _repository.Context.RegisterdInstitutes.FirstOrDefaultAsync(inst => inst.Guidaccessor == proposal);
                    if (inst != null)
                    {
                        InstitutionalStatus actionEnum;
                        Enum.TryParse(actions.Action, true, out actionEnum);

                        switch (actionEnum)
                        {
                            case InstitutionalStatus.Activate:
                                if (inst.Status == InstitutionalStatus.Query.ToString())
                                    if (CreateAdminCredentials(inst))
                                    {
                                        inst.Status = InstitutionalStatus.Active.ToString();
                                        this._repository.Context.RegisterdInstitutes.Update(inst);
                                        await this._repository.Context.SaveChangesAsync();
                                        this.sendMail(InstitutionalStatus.Active, inst);
                                        actions.Inqueries.Add(MapInquery(inst));
                                    }

                                break;
                            case InstitutionalStatus.Active:
                                if (inst != null)
                                {
                                    if (inst.Status == InstitutionalStatus.Inactive.ToString() || inst.Status == InstitutionalStatus.Blocked.ToString())
                                    {
                                        inst.Status = InstitutionalStatus.Active.ToString();
                                        this._repository.Context.RegisterdInstitutes.Update(inst);
                                        await this._repository.Context.SaveChangesAsync();
                                        this.sendMail(InstitutionalStatus.Active, inst);
                                        actions.Inqueries.Add(MapInquery(inst));
                                    }
                                }
                                break;
                            case InstitutionalStatus.Blocked:
                                if (inst != null)
                                {
                                    if (inst.Status == InstitutionalStatus.Active.ToString())
                                    {
                                        inst.Status = InstitutionalStatus.Blocked.ToString();
                                        this._repository.Context.RegisterdInstitutes.Update(inst);
                                        await this._repository.Context.SaveChangesAsync();
                                        this.sendMail(InstitutionalStatus.Blocked, inst);
                                        actions.Inqueries.Add(MapInquery(inst));
                                    }
                                }
                                break;
                            case InstitutionalStatus.Inactive:
                                if (inst != null)
                                {
                                    if (inst.Status == InstitutionalStatus.Active.ToString())
                                    {
                                        inst.Status = InstitutionalStatus.Inactive.ToString();
                                        this._repository.Context.RegisterdInstitutes.Update(inst);
                                        await this._repository.Context.SaveChangesAsync();
                                        this.sendMail(InstitutionalStatus.Blocked, inst);
                                        actions.Inqueries.Add(MapInquery(inst));
                                    }
                                }
                                break;
                            case InstitutionalStatus.Rejected:
                                if (inst != null)
                                {
                                    if (inst.Status == InstitutionalStatus.Query.ToString())
                                    {
                                        inst.Status = InstitutionalStatus.Rejected.ToString();
                                        this._repository.Context.RegisterdInstitutes.Update(inst);
                                        await this._repository.Context.SaveChangesAsync();
                                        this.sendMail(InstitutionalStatus.Rejected, inst);
                                        actions.Inqueries.Add(MapInquery(inst));
                                    }
                                }
                                break;
                            default:
                                break;


                        }
                    }

                }
            }
        }
        public int sendMail(InstitutionalStatus status,RegisterdInstitute institute)
        {
            string instituteName = institute.InstituteName; // Replace with the actual institute name
            string uniqueKey = institute.Guidaccessor??""; // Replace with the actual unique key
            string instituteStatus = status.ToString(); // Replace with the actual status

            string subject;
            string emailBody;

            switch (instituteStatus)
            {
                case "Active":
                    subject = $"Status Update: {instituteName} is Active in Eduverse";
                    emailBody = $"Dear Administrator,\r\n\r\nWe wanted to inform you that {instituteName} is currently active on Eduverse.\r\n\r\nYour unique key for communication is: {uniqueKey}\r\n\r\nThank you for being a part of Eduverse.\r\n\r\nBest regards,\r\nEduverse Team";
                    break;

                case "Blocked":
                    subject = $"Status Update: {instituteName} is Blocked in Eduverse";
                    emailBody = $"Dear Administrator,\r\n\r\nWe regret to inform you that {instituteName} has been blocked on Eduverse due to certain reasons.\r\n\r\nYour unique key for communication is: {uniqueKey}\r\n\r\nIf you have any queries, please contact our support team.\r\n\r\nBest regards,\r\nEduverse Team";
                    break;

                case "Inactive":
                    subject = $"Status Update: {instituteName} is Inactive in Eduverse";
                    emailBody = $"Dear Administrator,\r\n\r\n{instituteName} is currently marked as inactive on Eduverse.\r\n\r\nYour unique key for communication is: {uniqueKey}\r\n\r\nIf you wish to reactivate, please contact our support team.\r\n\r\nBest regards,\r\nEduverse Team";
                    break;

                case "Rejected":
                    subject = $"Status Update: {instituteName} is Rejected in Eduverse";
                    emailBody = $"Dear Administrator,\r\n\r\nWe regret to inform you that your proposal of {instituteName} has been rejected from joining Eduverse.\r\n\r\nYour unique key for communication is: {uniqueKey}\r\n\r\nIf you have any questions or concerns, please contact our team for further details.\r\n\r\nBest regards,\r\nEduverse Team";
                    break;

                default:
                    // Default case if the status doesn't match any of the above
                    subject = "Status Update: Unknown Status in Eduverse";
                    emailBody = "Dear Administrator,\r\n\r\nWe have an update regarding your institute's status in Eduverse. Please reach out to our support team for further assistance.\r\n\r\nBest regards,\r\nEduverse Team";
                    break;
            }

            return _mailService.sendMail(institute.Email, subject, emailBody);
            // Use 'subject' and 'emailBody' for sending emails or further processing


        }
        public Inquery MapInquery(RegisterdInstitute inst)
        {
            Inquery inq = new()
            {
                Accessor = inst.Guidaccessor,
                Status = inst.Status,
                Path = inst.Logo,
                Comment = inst.Comment,
                EmailId = inst.Email == null ? "" : inst.Email,
                PhoneNo = inst.PhoneNo == null ? "" : inst.PhoneNo,
                InstituteName = inst.InstituteName,
                InstituteType = inst.InstituteType == null ? "" : inst.InstituteType,
                Url = inst.Url
            };
            return inq; 
        }

        public bool CreateAdminCredentials(RegisterdInstitute institute)
        {
            Credential credential=new Credential();
            credential.InstitutitionalId = institute.InstitutitionalId;
            credential.Name = institute.InstituteName;
            credential.Role = "ORGANISATION";
            credential.Password = PasswordGenerator.GeneratePassword(14);
            bool update= this._repository.CreateCredentials(credential,institute.Email, "ORGANISATION");
            return update;
          
        }
        
    }
}

