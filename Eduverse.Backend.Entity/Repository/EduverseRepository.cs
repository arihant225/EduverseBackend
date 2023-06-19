using Eduverse.Backend.Entity.DBModels;
using Eduverse.Backend.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eduverse.Backend.Entity.Enums;

namespace Eduverse.Backend.Entity.Repository
{
    internal class EduverseRepository : IEduverseRepository
    {
        public EduverseContext Context { get; set; }

        public EduverseRepository()
        {
            Context = new EduverseContext();
        }

        public SmtpMailCredential? getSMTPCredentials(string role)
        {
            return this.Context.SmtpMailCredentials.Where(obj => obj.Role == role).FirstOrDefault();
        }

        public bool CheckOtp(string id, int otp, DateTime requestedTime, out OtpEnums message)
        {
            message = OtpEnums.InvalidMail;
            TempOtp? otpObj = this.Context.TempOtps.Where(obj => obj.Id == id).FirstOrDefault();
            bool success = false;
            if (otpObj != null)
            {
                if (otpObj.GeneratedTimeStamp != null)
                {
                    if (otpObj.Otp == otp)
                    {

                        if (otpObj.GeneratedTimeStamp.GetValueOrDefault().AddMinutes(5) >= requestedTime)
                        {

                            message = OtpEnums.Success;
                            success = true;

                            this.Context.TempOtps.Remove(otpObj);
                            this.Context.SaveChanges();
                        }
                        else
                        {

                            message = OtpEnums.Expired;
                            this.Context.TempOtps.Remove(otpObj);
                        }
                    }
                    else
                    {
                        message = OtpEnums.InvalidOtp;
                    }
                }
                else
                {
                    message = OtpEnums.NotGenerated;
                }


            }
            return success;
        }
    
        public bool GenerateOtpRecord(string id, int otp, string method)
        {
            try
            {
                TempOtp? tempOtpObject = this.Context.TempOtps.Where(tempobj => tempobj.Id == id).FirstOrDefault();
                if (tempOtpObject != null)
                {
                    tempOtpObject.Method = method;
                    tempOtpObject.Otp = otp;
                    tempOtpObject.GeneratedTimeStamp = DateTime.Now;
                    this.Context.Update(tempOtpObject);
                    this.Context.SaveChanges();
                }
                else
                {
                    tempOtpObject = new TempOtp()
                    {
                        Id = id,
                        Otp = otp,
                        Method = method,
                        GeneratedTimeStamp = DateTime.Now,
                    };
                    this.Context.Add(tempOtpObject);
                    this.Context.SaveChanges();

                }
                return true;
            }
            catch {
                return false;

            }


        }
    
       
    }
}
