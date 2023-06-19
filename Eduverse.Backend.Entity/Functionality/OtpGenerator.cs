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
using Eduverse.Backend.Entity.Enums;
using System.Xml.Linq;

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
                    message.Subject = "Your OTP for Eduverse Account Verification";

;
                    int otp = new Random().Next(100000, 999999);
                   

                    message.Body = $"Dear {username},\r\n\r\nThank you for choosing Eduverse as your preferred learning platform. We are delighted to have you on board! As part of our commitment to ensuring the utmost security of your account, we are pleased to provide you with a One-Time Password (OTP) to complete the verification process.\r\n\r\nPlease find your OTP below:\r\n\r\nOTP: {otp}\r\n\r\nTo verify your Eduverse account, simply enter this OTP on the verification page within the next 5 minutes. Kindly note that the OTP is case-sensitive.\r\n\r\nShould you encounter any difficulties or if you did not initiate this request, please do not hesitate to contact our dedicated support team immediately. They can be reached at [jarihant225@gmail.com/7024857237], and they will be more than happy to assist you.\r\n\r\nWe kindly request that you refrain from sharing this OTP with anyone else, as it is unique to your account and serves as a crucial element for the verification process. We want to assure you that the privacy and security of your personal information are of the utmost importance to us. Rest assured that we have implemented robust security measures to safeguard your data.\r\n\r\nThank you for your cooperation in ensuring the safety and integrity of your Eduverse account. We are thrilled to have the opportunity to provide you with an exceptional learning experience.\r\n\r\nBest regards,\r\n\r\nArihant Jain(DEV)\r\nEduverse Support Team";
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
        
        public bool VerifyOtp(string id,int otp,DateTime requestedTime,out OtpEnums message) {
            return OtpGenerator.eduverseRepository.CheckOtp(id, otp, requestedTime, out message);
        
        }
    }
}
