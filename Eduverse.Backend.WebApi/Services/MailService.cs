using Eduverse.Backend.Entity.DBModels;
using Eduverse.Backend.Entity.PropertyClasses;
using Eduverse.Backend.WebApi.Models.Request;
using System.Net.Mail;
using System.Net;
using Eduverse.Backend.Entity.Repository;
using Eduverse.Backend.WebApi.Services.Interface;

namespace Eduverse.Backend.WebApi.Services
{
    public class MailService:IMailService
    {
        public MailService(IEduverseRepository eduverseRepository)
        {
            this._repo = eduverseRepository;        

        }
        private IEduverseRepository _repo;

        public int sendMail(string? to, string subject, string body)
        {
            if (to == null)
            {
                return -1;
            }
            int successStatus = -1;


            SmtpMailCredential? mailCredential = _repo.getSMTPCredentials(SMTPCredentialsRole.otp);
            if (mailCredential == null)
                return -1;
            using (SmtpClient client = new(mailCredential.Server, mailCredential.Port.GetValueOrDefault()))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(mailCredential.EmailId, mailCredential.Password);
                MailMessage message = new MailMessage(mailCredential.EmailId, to);
                message.Subject = subject;

                ;


                message.Body = body;
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
