﻿using Eduverse.Backend.Entity.DBModels;
using Eduverse.Backend.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduverse.Backend.Entity.Repository
{
    internal interface IEduverseRepository
    {
        public EduverseContext Context { get;  set; }
        public SmtpMailCredential? getSMTPCredentials(string role);
        public bool GenerateOtpRecord(string id, int otp, string method);
        public bool CheckOtp(string id, int otp, DateTime requestedTime, out OtpEnums message);
    }
}
