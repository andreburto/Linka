using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Admin.Directory.directory_v1;
using Google.Apis.Admin.Directory.directory_v1.Data;
using Google.Apis.Services;
using Google.Apis.Util;
using Google.Apis.Util.Store;

using Linka;

namespace Linka
{
    public class EmailStuff
    {
        protected UserCredential credential;
        protected DirectoryService service;
        protected string StudentDomain;

        public bool CheckForEmailInDatabase(string stuid)
        {
            // Check the database
            DataRow dr = DbStuff.LoadStudentEmail(stuid);
            if (dr == null) { return false; }
            else { return true; }
        }

        public bool CheckForEmailInGoogle(string userid)
        {
            // Checks google
            try
            {
                if (userid.Contains('@') == false) { userid += "@" + StudentDomain; }
                User res = service.Users.Get(userid).Execute();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ConfirmOwner(string stuid, string userid)
        {
            DataRow dr = DbStuff.LoadStudentEmail(stuid);
            if (dr == null) return false;
            if (dr["STUMAIL_ADDRESS"].ToString().ToLower() == userid.ToLower()) return true;
            return false;
        }

        public EmailStatus FullCheck(string stuid)
        {
            EmailStatus status = new EmailStatus();
            DataRow dr = DbStuff.LoadStudentEmail(stuid);
            // User not in database
            if (dr == null) { return status; }
            // User in database
            status.InOracle = true;
            status.userid = dr["STUMAIL_ADDRESS"].ToString().ToLower();
            // User in Google
            if (CheckForEmailInGoogle(status.userid)) { status.InGoogle = true; }
            // Return status however it is
            return status;
        }

        public string SuggestId(string fn, string ln, int c = 1)
        {
            try
            {
                if (c == 5) { throw new Exception("Too many checks with Google."); }
                string id = fn.ToLower() + '.' + ln.ToLower();
                int number = DbStuff.HighestEmailCount(id);
                if (number > 0)
                {
                    number += c;
                    id += number.ToString();
                }
                if (CheckForEmailInGoogle(id))
                {
                    Thread.Sleep(1000);
                    return SuggestId(fn, ln, c+1);
                }
                return id;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public bool DeleteEmail(string userid)
        {
            try
            {
                if (userid.Contains('@') == false) { userid += "@" + StudentDomain; }
                service.Users.Delete(userid).Execute();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ChangePassword(string userid, string pw)
        {
            try
            {
                if (userid.Contains('@') == false) { userid += "@" + StudentDomain; }
                User student = new User();
                student.Password = pw;
                service.Users.Update(student, userid).Execute();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CreateEmail(string userid, string fn, string ln, string pw)
        {
            try
            {
                if (CheckForEmailInGoogle(userid) == true) { throw new Exception("Email exists."); }
                if (userid.Contains('@') == false) { userid += "@" + StudentDomain; }
                User student = new User();
                UserName stuname = new UserName();
                stuname.GivenName = fn;
                stuname.FamilyName = ln;
                student.Name = stuname;
                student.PrimaryEmail = userid;
                student.Password = pw;
                service.Users.Insert(student).Execute();
                return true;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message+"\r\n\r\n"+ex.StackTrace);
                return false;
            }
        }

        public EmailStuff(string gid, string gsf, string domain)
        {
            try
            {
                using (var stream = new FileStream(gsf, FileMode.Open, FileAccess.Read))
                {
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        new[] { DirectoryService.Scope.AdminDirectoryUser,
                                DirectoryService.Scope.AdminDirectoryGroup },
                        gid, CancellationToken.None).Result;
                }

                service = new DirectoryService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = App.Current.Resources["APPNAME"].ToString(),
                });

                StudentDomain = domain;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not authorize app.", ex);
            }   
        }
    }
}
