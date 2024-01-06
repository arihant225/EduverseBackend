using Eduverse.Backend.Entity.DBModels;
using Eduverse.Backend.Entity.Repository;
using Eduverse.Backend.WebApi.Controllers;
using Eduverse.Backend.WebApi.Models.Request;
using Eduverse.Backend.WebApi.Services.Interface;
using System.Net.Mail;
using System.Net;
using Eduverse.Backend.Entity.PropertyClasses;
namespace Eduverse.Backend.WebApi.Services
{
    public class InqueryService: IInqueryService
    {
        public readonly IEduverseRepository _repo;
        public InqueryService(IEduverseRepository repo) {
            this._repo = repo;
        }
        
        public async Task<bool> AddInquery (Inquery inquery){
            if(inquery==null) return false; 

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
            return true;    
        }

        public int SendCopyOfProposal(RegisterdInstitute institute)
        {
            int successStatus = -1;


            SmtpMailCredential? mailCredential = _repo.getSMTPCredentials(SMTPCredentialsRole.otp);
            if (mailCredential == null)
                return -1;
            using (SmtpClient client = new(mailCredential.Server, mailCredential.Port.GetValueOrDefault()))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(mailCredential.EmailId, mailCredential.Password);
                MailMessage message = new MailMessage(mailCredential.EmailId, institute.Email);
                message.Subject = $"proposal Submitted : {institute.InstituteName}";

                ;


                message.Body = $"Hello,\n\n"
                + $"This is to confirm that your inquiry to Eduverse has been successfully Submitted.\n\n"
                + $"Institute Name: {institute.InstituteName}\n"
                + $"Institute Type: {institute.InstituteType}\n"
                + $"Email: {institute.Email}\n"
                + $"Phone Number: {institute.PhoneNo}\n"
                + $"Inquery Number: {institute.Guidaccessor}\n\n"
                + $"Thank you for choosing Eduverse!\n\n"
                + $"Best regards,\nThe Eduverse Team";

                try
                {
                    client.Send(message);
                }
                catch { }


            }

            return successStatus;
        }


    }
}
