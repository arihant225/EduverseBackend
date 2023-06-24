using Eduverse.Backend.Entity.DBModels;
using Eduverse.Backend.Entity.PropertyClasses;
using Eduverse.Backend.Entity.Repository;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Eduverse.Backend.Entity.Functionality
{
   
    internal class Mail
    {
        static IEduverseRepository repository = new EduverseRepository();

        public void SendText(string mail,string message,string subject) {

            SmtpMailCredential? credentials = repository.getSMTPCredentials(SMTPCredentialsRole.otp);
            if (credentials == null)
                return;

            using (SmtpClient client = new SmtpClient(credentials.Server, credentials.Port.GetValueOrDefault()))
            {
                client.EnableSsl = true;
                NetworkCredential networkCredential = new NetworkCredential(credentials.EmailId, credentials.Password);

                client.Credentials = networkCredential; 
                MailMessage mailMessage = new MailMessage(credentials.EmailId,mail);
                mailMessage.Body = message;
                mailMessage.Subject = subject;

                try
                {

                    client.Send(mailMessage);
                }
                catch {


                
                }


            }
        
        }
    }
}
