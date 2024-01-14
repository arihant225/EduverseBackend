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
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using Eduverse.Backend.Entity.Functionality;
using System.Net;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Eduverse.Backend.Entity.Repository
{
    public class EduverseRepository : IEduverseRepository
    {
        public EduverseContext Context { get; set; }
        Mail EmailSender = new();
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
            catch
            {
                return false;

            }


        }

        public Credential? getCredentials(string id, string password)
        {
            List<DBModels.Credential> credentials = new();
            id = id.ToLower();

            try
            {


                credentials = this.Context.Credentials.Where(cred => (cred.EduverseId.ToLower() == id||(cred.EmailId!=null&&cred.EmailId.ToLower()==id)||(""+cred.PhoneNumber==id)) && cred.Password == password).Include(inst=>inst.InstitutionalRoles).Include(obj => obj.EduverseRoles).Include(inst => inst.Institutitional).ToList();
               
            }
            catch { }
            if (credentials.Count == 1)
            {
                return credentials.FirstOrDefault(cred=>cred.Institutitional==null||cred.Institutitional.Status=="Active");
            }
            else
                return null;


        }

        public bool RecordExist(string identity, CheckEnums check)
        {
            try
            {
                if (check == CheckEnums.PHONE)
                {
                    if (this.Context.Credentials.Any(obj => obj.PhoneNumber == Convert.ToDecimal(identity)))
                    {
                        return true;
                    }
                    return false;
                }
                if (check == CheckEnums.EMAIL)
                {
                    if (this.Context.Credentials.Any(obj => obj.EmailId.ToLower().Equals(identity.ToLower())))
                    {
                        return true;
                    }
                    return false;
                }
                if(check == CheckEnums.ID) {
                    if (this.Context.Credentials.Any(obj => obj.EduverseId.Equals(identity.ToLower())))
                    {
                        return true;
                    }
                    return false;

                }
            }
            catch
            {

            }
            return false;


        }



        public bool CreateCredentials(Credential credential,string? emailId="", string role = "User"   )
        {
            credential.Role = String.IsNullOrEmpty(credential.Role)?"user":credential.Role;
            credential.EduverseId = "";
            string accessor = Guid.NewGuid().ToString();
            SqlParameter param1 = new SqlParameter("@NAME",credential.Name != null ? credential.Name : DBNull.Value);
            if(credential.EmailId!=null)
                credential.EmailId=credential.EmailId.ToLower();
           
            SqlParameter param2 = new SqlParameter("@EMAIL",credential.EmailId!=null?credential.EmailId:DBNull.Value);
            SqlParameter param3 = new SqlParameter("@PHONENO",credential.PhoneNumber!=null?credential.PhoneNumber:DBNull.Value);
            SqlParameter param4 = new SqlParameter("@PASSWORD",credential.Password);
            SqlParameter param5 = new SqlParameter("@ROLE",credential.Role != null ? credential.Role : DBNull.Value);
            SqlParameter param7 = new SqlParameter("@instituitionalId",credential.InstitutitionalId != null ? credential.InstitutitionalId : DBNull.Value);
            SqlParameter param8 = new SqlParameter("@UserId",!string.IsNullOrEmpty(credential.EduverseId)? credential.EduverseId : DBNull.Value);
            SqlParameter param6 = new SqlParameter("@GuidAccessor",accessor);
            param1.IsNullable = true;
            param2.IsNullable = true;
            param3.IsNullable = true;
            param4.IsNullable = true; param7.IsNullable = true;
            param5.IsNullable = true;
            param6.IsNullable = true;
            int rowManipulated = 0;
            try
            {
                 rowManipulated = this.Context.Database.ExecuteSqlRaw("EXEC AddCredentials @NAME, @EMAIL,@PHONENO,@PASSWORD,@ROLE,@GuidAccessor,@instituitionalId,@UserId", param1, param2, param3, param4, param5, param6, param7,param8);
            }
            catch (Exception ex) { 
            Console.WriteLine(ex.ToString());   
            }
            Context.SaveChanges();
            if (rowManipulated > 0)
            {  
               
                Credential? temp = this.Context.Credentials.Where(cred =>  cred.Guidaccessor==accessor&& cred.Password == credential.Password).FirstOrDefault();

                var subject = "welcome to Eduverse";
                if(emailId!=null&&temp!=null)
                {
                    var message = $"Dear {credential.Name},\r\n\r\nCongratulations! Your account has been successfully created on Eduverse.\r\n\r\nYour unique Eduverse ID is: {temp?.EduverseId}\r\nYour Password is: {temp?.Password}\r\n\r\nThank you for joining Eduverse. We are excited to have you on board!\r\n\r\nBest regards,\r\nEduverse Team\r\n";
                    EmailSender.SendText(emailId, message, subject);

                }
                else if (temp != null&&credential.EmailId!=null)
                {
                    var message = $"Dear {credential.Name},\r\n\r\nCongratulations! Your account has been successfully created on Eduverse.\r\n\r\nYour unique Eduverse ID is: {temp?.EduverseId}\r\n\r\nThank you for joining Eduverse. We are excited to have you on board!\r\n\r\nBest regards,\r\nEduverse Team\r\n";
                    EmailSender.SendText(credential.EmailId, message, subject);

                }
                if(InstitutionalId!=null) { 
                InstitutionalRole institutionalRole = new InstitutionalRole()
                { 
                InstitutionalId=credential.InstitutitionalId,
                RoleType=role         
                };

                    this.Context.InstitutionalRoles.Add(institutionalRole);
                    this.Context.SaveChanges();
                }
            }
            return rowManipulated > 0;
        }


      
        public async Task<Note?> SaveNotes(string accessor, Note notes, int? parentFolderId)
        {
            try
            {

                string? userId = await this.userId(accessor);
                if (userId != null)
                {
                    var tempNote = await this.Context.Notes.Where(note => note.NotesId == notes.NotesId).FirstOrDefaultAsync();

                    if (tempNote != null)
                    {

                        tempNote.Body = notes.Body;
                        tempNote.BodyStyle = notes.BodyStyle;
                        tempNote.IsPrivate = notes.IsPrivate;
                        tempNote.Title = notes.Title;
                        tempNote.TitleStyle = notes.TitleStyle;
                        this.Context.Notes.Update(tempNote);
                    }
                    else
                    {
                        notes.UserId = userId;
                        await this.Context.Notes.AddAsync(notes);
                    }
                    await this.Context.SaveChangesAsync();

                    SubItem? linkedRecord = await this.Context.SubItems.Where(obj => obj.NoteId == notes.NotesId).FirstOrDefaultAsync();
                    if (linkedRecord == null)
                    {

                        linkedRecord = new()
                        {
                            NoteId = notes.NotesId,
                            LinkedFolderId = parentFolderId

                        };
                        if (linkedRecord != null)
                        {
                            this.Context.SubItems.Add(linkedRecord);
                        }

                    }
                    else
                    {
                        linkedRecord.LinkedFolderId = parentFolderId;
                        this.Context.SubItems.Update(linkedRecord);
                    }
                    await this.Context.SaveChangesAsync();
                    return notes;

                }
                else
                {
                    return null;
                }
            }
            catch {
                return null;
            }
        }



        public async Task<Folder?> SaveFolder(Folder folder, string userId, int? parentFolderId) {
            if (folder == null)
                return null;
            Folder? temp = await this.Context.Folders.Where(obj => obj.FolderId == folder.FolderId).FirstOrDefaultAsync();
            if (temp != null)
            {

                temp.FolderName = folder.FolderName;
                this.Context.Folders.Update(temp);
            }


            else {
                folder.Userid = userId;
                this.Context.Folders.Add(folder);
            }
            await this.Context.SaveChangesAsync();

            SubItem? linkedRecord = await this.Context.SubItems.Where(obj => obj.LinkedFolderId == folder.FolderId).FirstOrDefaultAsync();
            if (linkedRecord == null)
            {

                linkedRecord = new()
                {
                    FolderId = folder.FolderId,
                    LinkedFolderId = parentFolderId

                };
                if (linkedRecord != null)
                {
                    this.Context.SubItems.Add(linkedRecord);
                }

            }
            else
            {
                linkedRecord.LinkedFolderId = parentFolderId;
                this.Context.SubItems.Update(linkedRecord);
            }
            await this.Context.SaveChangesAsync();
            return folder;
        }
        public async Task<bool> DeleteFolder(int id) {
         Folder? folder=  await this.Context.Folders.FindAsync(id);
            if (folder != null)
            {
                try
                {
                    List<SubItem> subItems = await this.Context.SubItems.Where(obj => obj.LinkedFolderId == id||obj.FolderId==id).ToListAsync();
                    if (subItems.Any())
                    {
                        this.Context.SubItems.RemoveRange(subItems);
                        await this.Context.SaveChangesAsync(true);
                    }
                    this.Context.Folders.Remove(folder);
                    await this.Context.SaveChangesAsync();
                    return true;
                }
                catch { 
                return false;
                }

                
            }
            return false;

        }
        public async Task<bool> validateFolder(int? folderId,string userId) => folderId == null || (await this.Context.Folders.Where(obj=>obj.FolderId==folderId&&obj.Userid==userId).FirstOrDefaultAsync()!=null);

        public async Task<List<SubItem>> GetSubItems(int? folderId) => await this.Context.SubItems.Where(obj => obj.LinkedFolderId == folderId).Include(obj=>obj.Note).Include(obj=>obj.Folder).ToListAsync();

        public async Task<Note?> GetNotes(long id, string userId) => await this.Context.Notes.Where(obj => obj.NotesId == id && obj.UserId == userId && obj.IsPrivate == true || obj.NotesId == id  && obj.IsPrivate != true).FirstOrDefaultAsync();

        public async Task<string?> userId(string accessor) => await this.Context.Credentials.Where(cred => cred.Guidaccessor==accessor).Select(obj => obj.EduverseId).FirstOrDefaultAsync();
        public async Task<int?> InstitutionalId(string accessor) => await this.Context.Credentials.Where(cred => cred.Guidaccessor == accessor).Select(obj => obj.InstitutitionalId).FirstOrDefaultAsync();
        public async Task<List<RegisterdInstitute>> GetAllInstitutes() => await this.Context.RegisterdInstitutes.ToListAsync();
    }
}
