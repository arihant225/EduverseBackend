﻿using Eduverse.Backend.Entity.DBModels;
using Eduverse.Backend.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stream = Eduverse.Backend.Entity.DBModels.Stream;

namespace Eduverse.Backend.Entity.Repository
{
    public interface IEduverseRepository
    {
        public EduverseContext Context { get;  set; }
        public SmtpMailCredential? getSMTPCredentials(string role);
        public bool GenerateOtpRecord(string id, int otp, string method);
        public bool CheckOtp(string id, int otp, DateTime requestedTime, out OtpEnums message);
        public bool RecordExist(string identity, CheckEnums check);
        public bool CreateCredentials(Credential credential);
        public Credential? getCredentials(string id, string password);
        public Task<Note?> SaveNotes(string emailId, decimal phoneNumber, Note notes, int? parentFolderId);
        public Task<Note?> GetNotes(long id, string userId);
        public  Task<string?> userId(string id, decimal number);
        public  Task<Folder?> SaveFolder(Folder folder, string userId,int? parentFolderId);
        public  Task<List<SubItem>> GetSubItems(int? folderId);
        public  Task<bool> validateFolder(int? folderId,string userId);
        public Task<bool> DeleteFolder(int id);
        public Task<List<RegisterdInstitute>> GetAllInstitutes();
    }

}
