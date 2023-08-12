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
using Stream = Eduverse.Backend.Entity.DBModels.Stream;

namespace Eduverse.Backend.Entity.Repository
{
    public class EduverseRepository : IEduverseRepository
    {
        public EduverseContext Context { get; set; }
        static Mail EmailSender = new Mail();

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
            string authenticationType = "";
            if (id.Length == 10)
            {
                authenticationType = "phone";
            }
            if (id.Contains("@"))
            {
                authenticationType = "email";
            }

            try
            {

                if (authenticationType == "email")
                {
                    credentials = this.Context.Credentials.Where(cred => cred.EmailId.ToLower().Equals(id.ToLower()) && cred.Password == password).Include(obj => obj.EduverseRoles).ToList();
                }
                else if (authenticationType == "phone")
                {
                    credentials = this.Context.Credentials.Where(cred => cred.PhoneNumber == Convert.ToInt64(id) && cred.Password == password).Include(obj => obj.EduverseRoles).ToList();
                }
            }
            catch { }
            if (credentials.Count == 1)
            {
                return credentials.FirstOrDefault();
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
            }
            catch
            {

            }
            return false;


        }



        public bool CreateCredentials(Credential credential)
        {
            credential.Role = "self";
            credential.EduverseId = "";

            int rowManipulated = this.Context.Database.ExecuteSqlInterpolated($"EXEC EnteringCredentials {credential.Name},{credential.EmailId.ToLower()},{credential.PhoneNumber},{credential.Password},{credential.Role}");

            Context.SaveChanges();
            if (rowManipulated > 0)
            {
                var temp = this.Context.Credentials.Where(cred => cred.Equals(credential.EmailId.ToLower()) && cred.Password == credential.Password).FirstOrDefault();

                var subject = "welcome to Eduverse";
                if (temp != null)
                {
                    var message = $"Dear {credential.Name},\r\n\r\nCongratulations! Your account has been successfully created on Eduverse.\r\n\r\nYour unique Eduverse ID is: {temp?.EduverseId}\r\n\r\nThank you for joining Eduverse. We are excited to have you on board!\r\n\r\nBest regards,\r\nEduverse Team\r\n";
                    EmailSender.SendText(credential.EmailId, message, subject);

                }
            }
            return rowManipulated > 0;
        }


        public List<Stream>? EduverseStreams()
        {
            try
            {
                return this.Context.Streams.Where(obj => obj.Public != 0).ToList();
            }
            catch (Exception e)
            {
                return null;
            }

        }
        public async Task<Note?> SaveNotes(string emailId, decimal phoneNumber, Note notes)
        {
            try
            {

                string? userId = await this.userId(emailId, phoneNumber);
                if (userId != null)
                {
                    var tempNote = await this.Context.Notes.Where(note => note.NotesId == notes.NotesId).FirstOrDefaultAsync();

                    if (tempNote!=null)
                    {
                    
                        tempNote.Body = notes.Body;
                        tempNote.BodyStyle = notes.BodyStyle;   
                        tempNote.IsPrivate = notes.IsPrivate;
                        tempNote.Title=notes.Title;
                        tempNote.TitleStyle=notes.TitleStyle;
                        this.Context.Notes.Update(tempNote);
                    }
                    else
                    {
                        notes.UserId = userId;
                        await this.Context.Notes.AddAsync(notes);
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

        public async Task<Note?> GetNotes(long id, string userId) => await this.Context.Notes.Where(obj => obj.NotesId == id && obj.UserId == userId && obj.IsPrivate == true || obj.NotesId == id  && obj.IsPrivate != true).FirstOrDefaultAsync();

        public async Task<string?> userId(string id, decimal number) => await this.Context.Credentials.Where(cred => cred.EmailId == id && number == cred.PhoneNumber).Select(obj => obj.EduverseId).FirstOrDefaultAsync();
    }
}
