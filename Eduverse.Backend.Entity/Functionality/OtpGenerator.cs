using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using Eduverse.Backend.Entity.Repository;
using Eduverse.Backend.Entity.DBModels;

using Eduverse.Backend.Entity.PropertyClasses;
using System.Net;

namespace Eduverse.Backend.Entity.Functionality
{
    public class OtpGenerator
    {
     static IEduverseRepository eduverseRepository=new EduverseRepository();
        public int generateOtp(string id, string username, string method,out bool generate) {
            int successStatus = -1;
            generate = false;
            if (method.ToLower().Equals("mail"))
            {

                SmtpMailCredential? mailCredential = eduverseRepository.getSMTPCredentials(SMTPCredentialsRole.otp);
                if (mailCredential == null)
                    return -1;
                using (SmtpClient client = new(mailCredential.Server, mailCredential.Port.GetValueOrDefault()))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(mailCredential.EmailId, mailCredential.Password);
                    MailMessage message = new MailMessage(mailCredential.EmailId, id);
                    message.Subject = "OTP for Sign Up";
                    int otp = new Random().Next(100000, 999999);

                    message.Body = $"Dear {username},\r\n\r\nThank you for choosing Eduverse as your online learning platform. To ensure the security of your account, we require OTP verification.\r\n\r\nPlease use the following OTP to complete your account verification:\r\n\r\nOTP: {otp}\r\n\r\nThis OTP is valid for 5 minutes. Please do not share it with anyone, as it grants access to your Eduverse account.\r\n\r\nIf you did not initiate this verification or have any concerns regarding your account security, please contact our support team immediately at eduverse1802@gmail.com or 7024857237 .\r\n\r\nThank you for your cooperation.\r\n\r\nBest regards,\r\nThe Eduverse Team\r\n\r\n\r\n\r\n";
                    try
                    {
                        client.Send(message);
                        eduverseRepository.GenerateOtpRecord(id, otp, method);
                        generate = true;
                        successStatus = 1;
                    }
                    catch {
                        successStatus = -2;
                    }



                }
            }
            else { 
            }
            return successStatus;
        }
      
    }
}
