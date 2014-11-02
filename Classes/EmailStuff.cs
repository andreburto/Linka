using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
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
            return _goog.CheckUserExists(userid);
        }

        public bool ConfirmOwner(string stuid, string userid)
        {
            DataRow dr = DbStuff.LoadStudentEmail(stuid);
            if (dr == null) return false;
            if (dr["STUMAIL_ADDRESS"].ToString().ToLower() == userid.ToLower()) return true;
            return false;
        }

        public bool DeleteEmail(string id)
        {
            return true;
        }

        public bool ChangePassword(string id, string pw)
        {
            return true;
        }

        public bool CreateEmail(string id, string pw)
        {
            return true;
        }

        public EmailStuff(string gid, string gpw, string domain)
        {
            _goog = new Google();
            _goog.GetAuth(gid, gpw);
            _goog.domain = domain;
        }
    }
}
