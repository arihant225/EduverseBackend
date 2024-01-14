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
using static System.Net.WebRequestMethods;
using System.Security;

namespace Eduverse.Backend.Entity.Functionality
{
    public class OtpGenerator
    {
     static IEduverseRepository eduverseRepository=new EduverseRepository();
        public void SaveOTPrecord(string id,int otp,string method)
        {
            eduverseRepository.GenerateOtpRecord(id, otp, method);

        }

        public int generateOtpForSignUp(string id, string username, string method,out bool generate)
        {
            int otp = new Random().Next(100000, 999999);
            generate = false;
            string subject = "Your OTP for Eduverse Account Verification";
            string body = $"Dear {username},\r\n\r\nThank you for choosing Eduverse as your preferred learning platform. We are delighted to have you on board! As part of our commitment to ensuring the utmost security of your account, we are pleased to provide you with a One-Time Password (OTP) to complete the verification process.\r\n\r\nPlease find your OTP below:\r\n\r\nOTP: {otp}\r\n\r\nTo verify your Eduverse account, simply enter this OTP on the verification page within the next 5 minutes. Kindly note that the OTP is case-sensitive.\r\n\r\nShould you encounter any difficulties or if you did not initiate this request, please do not hesitate to contact our dedicated support team immediately. They can be reached at [jarihant225@gmail.com/7024857237], and they will be more than happy to assist you.\r\n\r\nWe kindly request that you refrain from sharing this OTP with anyone else, as it is unique to your account and serves as a crucial element for the verification process. We want to assure you that the privacy and security of your personal information are of the utmost importance to us. Rest assured that we have implemented robust security measures to safeguard your data.\r\n\r\nThank you for your cooperation in ensuring the safety and integrity of your Eduverse account. We are thrilled to have the opportunity to provide you with an exceptional learning experience.\r\n\r\nBest regards,\r\n\r\nArihant Jain(DEV)\r\nEduverse Support Team";

            SaveOTPrecord(id, otp, "Mail");
            int state =sendMail(id,subject,body);
            generate = 1 == state;
            return state;
        }

        public int generateOtpForWithdrawingProposal(string id, string username, string method, out bool generate)
        {
            int otp = new Random().Next(100000, 999999);
            generate = false;
            string subject = $" Confirmation Required: Proposal Withdrawal for Your Institute on Eduverse";
            string body = $"Dear {username},\r\n\r\nA withdrawal request for the proposal to onboard your institute onto the Eduverse platform has been initiated. To validate and process this request securely, we require your confirmation through a one-time password (OTP) process.\r\n\r\nPlease use the following OTP for confirmation:\r\n\r\nOTP: {otp} \r\n\r\nIf this withdrawal request was not initiated by you or your team, please reach out to our support team immediately at jarihant225@gmail.com. Your swift attention to this matter is greatly appreciated.\r\n\r\nThank you for your cooperation.\r\n\r\nBest regards,\r\n Arihant Jain [dev] \r\nEduverse";
            SaveOTPrecord(id, otp, "Mail");
            int state = sendMail(id, subject, body);
            generate = 1 == state;
            return state;
        }


        public int generateOtpForInQuery(string to, string username, string method, out bool generate)
        {
            generate = false;
            if (method.ToLower().Equals("mail"))
            {

                int otp = new Random().Next(100000, 999999);

                string body = $"Hello {username},\n\n"
                 + $"Thank you for your inquiry to Enrolling your institute. Please confirm by providing the OTP below:\n\n"
                 + $"OTP: {otp}\n\n"
                 + $"Best regards,\nThe Eduverse Team"; 
                
                string subject = "OTP verification for enrolling your institute";

                SaveOTPrecord(to, otp, "Mail");
                int state= this.sendMail(to, subject, body);
                generate = 1 == state;
                return state;
            }
            else
            {
            }
            return -1;
        }

        public bool VerifyOtp(string id,int otp,DateTime requestedTime,out OtpEnums message) {
            return OtpGenerator.eduverseRepository.CheckOtp(id, otp, requestedTime, out message);
        
        }


        public int sendMail(string? to, string subject, string body)
        {
            if (to == null)
            {
                return -1;
            }
            int successStatus = -1;


            SmtpMailCredential? mailCredential = eduverseRepository.getSMTPCredentials(SMTPCredentialsRole.otp);
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
                    successStatus = 1;
                }
                catch { }


            }

            return successStatus;

        }

    }
}
