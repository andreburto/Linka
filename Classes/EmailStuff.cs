using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using Linka;
using GoogleStudentsLib;

namespace Linka
{
    public class EmailStuff
    {
        protected Google _goog;

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
            return _goog.CheckUserExists(userid);
        }

        public bool ConfirmOwner(string stuid, string userid)
        {
            DataRow dr = DbStuff.LoadStudentEmail(stuid);
            if (dr == null) return false;
            if (dr["STUMAIL_ADDRESS"].ToString().ToLower() == userid.ToLower()) return true;
            return false;
        }

        public string[] FindSimilar(string fn, string ln)
        {
            string[] ids = { };

            return ids;
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

        public string SuggestId(string fn, string ln, int c)
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

        public string SuggestId(string fn, string ln)
        {
            return SuggestId(fn, ln, 1);
        }

        public bool DeleteEmail(string id)
        {
            return _goog.DeleteUser(id);
        }

        public bool ChangePassword(string id, string fn, string ln, string pw)
        {
            User u = _goog.UpdateUser(id, fn, ln, pw);
            return u.exists;
        }

        public bool CreateEmail(string id, string fn, string ln, string pw)
        {
            User u = _goog.CreateUser(id, fn, ln, pw);
            return u.exists;
        }

        public override string ToString()
        {
            return String.Format("AUTHKEY: {0}, DOMAIN: {1}", _goog.authkey, _goog.domain);
        }

        public EmailStuff(string gid, string gpw, string domain)
        {
            _goog = new Google();
            _goog.authkey = _goog.GetAuth(gid, gpw);
            _goog.domain = domain;
        }
    }
}
