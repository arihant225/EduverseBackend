using Eduverse.Backend.Entity.DBModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduverse.Backend.Entity.Repository
{
    internal class EduverseRepository : IEduverseRepository
    {
        public EduverseContext Context { get;  set; }

        public EduverseRepository()
        {
            Context = new EduverseContext();    
        }

        public SmtpMailCredential? getSMTPCredentials(string role)
        {
            return this.Context.SmtpMailCredentials.Where(obj => obj.Role == role).FirstOrDefault();
        }
        public bool GenerateOtpRecord(string id, int otp,string method)
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
