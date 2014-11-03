using System;
using System.Data;
using System.Collections.Generic;
using System.Windows.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Oracle.DataAccess.Client;
using Linka;

namespace Linka
{
    public static class DbStuff
    {
        public static void MakeConnection()
        {
            OracleConnection connOra = new OracleConnection();

            try
            {
                // Connect to the database
                connOra.ConnectionString = MakeConn();
                connOra.Open();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n\n" + ex.StackTrace + "\n\n" + ex.Source);
            }
            finally
            {
                // No matterv what, close and dispose
                connOra.Close();
                connOra.Dispose();
            }
        }

        public static bool InsertEmail(string stuid, string userid, string term)
        {
            string sql = "INSERT INTO NOBOTO.STUMAIL (STUMAIL_ID,STUMAIL_ADDRESS,STUMAIL_TERM) VALUES ('{0}','{1}','{2}')";
            return WriteData(String.Format(sql, stuid, userid, term));
        }

        public static bool DeleteEmailByUserid(string userid)
        {
            string sql = "DELETE FROM NOBOTO.STUMAIL WHERE STUMAIL_ADDRESS = '{0}' AND ROWNUM < 2";
            return WriteData(String.Format(sql, userid));
        }

        public static DataRow LoadStudentEmail(string id)
        {
            string sql = "select * from noboto.STUMAIL where STUMAIL_ID='"+id+"'";
            DataSet ds = FetchDataSet(sql);
            if (ds.Tables.Count == 0) { return null; }
            if (ds.Tables[0].Rows.Count == 0) { return null; }
            return ds.Tables[0].Rows[0];
        }

        public static Int16 CountSimilarAddresses(string address)
        {
            string sql = "select count(*) c FROM noboto.stumail where STUMAIL_ADDRESS like '"+address+"%'";
            DataSet ds = FetchDataSet(sql);
            if (ds.Tables.Count == 0) { return 0; }
            if (ds.Tables[0].Rows.Count == 0) { return 0; }
            DataRow dr = ds.Tables[0].Rows[0];
            Int16 c = Int16.Parse(dr["c"].ToString());
            return c;
        }

        public static bool EmailExists(string address)
        {
            string sql = "select count(*) c from noboto.stumail where STUMAIL_ADDRESS = '" + address + "'";
            DataSet ds = FetchDataSet(sql);
            if (ds.Tables.Count == 0) { return false; }
            if (ds.Tables[0].Rows.Count == 0) { return false; }
            return true;
        }

        public static Int16 HighestEmailCount(string address)
        {
            if (DbStuff.CountSimilarAddresses(address) == 0) { return 0; }
            if (DbStuff.CountSimilarAddresses(address) == 1) { return 1; }
            string sql = "select substr(stumail_address, -1, 1) lastn from noboto.stumail where stumail_address like '"+address+"%' and rownum < 2 order by stumail_address desc";
            DataSet ds = FetchDataSet(sql);
            if (ds.Tables.Count == 0) { return 0; }
            if (ds.Tables[0].Rows.Count == 0) { return 0; }
            DataRow r = ds.Tables[0].Rows[0];
            return Int16.Parse(r["LASTN"].ToString());
        }

        public static DataSet LoadTeachersOfClass(string classid, string term)
        {
            string sql = "select SPRIDEN.SPRIDEN_PIDM,SPRIDEN.SPRIDEN_ID,SPRIDEN.SPRIDEN_LAST_NAME,SPRIDEN.SPRIDEN_FIRST_NAME,SPRIDEN.SPRIDEN_MI from SATURN.SIRASGN SIRASGN left join SATURN.SPRIDEN SPRIDEN on SIRASGN.SIRASGN_PIDM = SPRIDEN.SPRIDEN_PIDM where SPRIDEN.SPRIDEN_CHANGE_IND is null and SIRASGN.SIRASGN_TERM_CODE ='"+term+"' and SIRASGN.SIRASGN_CRN ='"+classid+"' order by SPRIDEN.SPRIDEN_LAST_NAME,SPRIDEN.SPRIDEN_FIRST_NAME,SPRIDEN.SPRIDEN_MI,SPRIDEN.SPRIDEN_ID";
            return FetchDataSet(sql);
        }

        public static DataSet LoadTeacherInfo(string pidm)
        {
            string sql = "select SPRIDEN_ID,SPRIDEN_LAST_NAME,SPRIDEN_FIRST_NAME,NVL(SPRIDEN_MI,'') SPRIDEN_MI,SPRIDEN_ACTIVITY_DATE from SATURN.SPRIDEN SPRIDEN where SPRIDEN.SPRIDEN_PIDM =" + pidm + " order by SPRIDEN_ACTIVITY_DATE desc";
            return FetchDataSet(sql);
        }

        public static DataSet LoadStudentAddresses(string pidm)
        {
            string sql = "select spraddr_atyp_code,TRIM(spraddr_street_line1||' '||spraddr_street_line2||' '||spraddr_street_line3) spraddr_street_line,spraddr_city,spraddr_stat_code,spraddr_zip from spraddr where spraddr_pidm=" + pidm + " order by spraddr_seqno asc";
            return FetchDataSet(sql);
        }

        public static DataSet LoadStudentInfoBySsn(string ssn)
        {
            string sql = "select spbpers.*, SPRIDEN.* from spbpers join spriden on spbpers.spbpers_pidm = spriden.spriden_pidm where spriden_change_ind is null and spbpers_ssn='"+ssn+"'";
            return FetchDataSet(sql);
        }

        public static DataSet LoadStudentInfoById(string id)
        {
            string sql = "select spbpers.*, SPRIDEN.* from spbpers join spriden on spbpers.spbpers_pidm = spriden.spriden_pidm where spriden_change_ind is null and spriden_id='" + id + "'";
            return FetchDataSet(sql);
        }

        public static DataSet LoadStudentInfoByPidm(string pidm)
        {
            string sql = "select spbpers.*, SPRIDEN.* from spbpers join spriden on spbpers.spbpers_pidm = spriden.spriden_pidm where spriden_change_ind is null and spriden_pidm='" + pidm + "'";
            return FetchDataSet(sql);
        }

        public static DataSet LoadStudentClasses(string pidm, string term)
        {
            string sql = "select SSBSECT.SSBSECT_SUBJ_CODE,SSBSECT.SSBSECT_CRSE_NUMB,SSBSECT.SSBSECT_CRN,SSBSECT.SSBSECT_TERM_CODE,SFBETRM.SFBETRM_AR_IND from ( ( SATURN.SFBETRM SFBETRM inner join SATURN.SFRSTCR SFRSTCR on SFBETRM.SFBETRM_PIDM = SFRSTCR.SFRSTCR_PIDM ) inner join SATURN.STVRSTS STVRSTS on SFRSTCR.SFRSTCR_RSTS_CODE = STVRSTS.STVRSTS_CODE ) left join SATURN.SSBSECT SSBSECT on SFRSTCR.SFRSTCR_CRN = SSBSECT.SSBSECT_CRN where STVRSTS.STVRSTS_INCL_SECT_ENRL = 'Y' and SFRSTCR.SFRSTCR_TERM_CODE ='"+term+"' and SFRSTCR.SFRSTCR_PIDM ="+pidm+" order by SSBSECT.SSBSECT_SUBJ_CODE,SSBSECT.SSBSECT_CRSE_NUMB";
            return FetchDataSet(sql);
        }

        public static DataSet LoadRostersByClass(string classid, string term)
        {
            string sql = "select SPRIDEN.SPRIDEN_ID,SPRIDEN.SPRIDEN_FIRST_NAME,SPRIDEN.SPRIDEN_LAST_NAME,STVRSTS.STVRSTS_CODE,STVRSTS.STVRSTS_DESC,NVL(SFRSTCR.SFRSTCR_GRDE_CODE_MID,'NULL') SFRSTCR_GRDE_CODE_MID from SATURN.SFRSTCR SFRSTCR,SATURN.SPRIDEN SPRIDEN,SATURN.STVRSTS STVRSTS where (SFRSTCR.SFRSTCR_PIDM = SPRIDEN.SPRIDEN_PIDM (+) and SFRSTCR.SFRSTCR_RSTS_CODE = STVRSTS.STVRSTS_CODE (+)) and (SFRSTCR.SFRSTCR_TERM_CODE="+term+" and SFRSTCR.SFRSTCR_CRN="+classid+" and SPRIDEN.SPRIDEN_CHANGE_IND is null ) order by SPRIDEN.SPRIDEN_LAST_NAME,SPRIDEN.SPRIDEN_FIRST_NAME,SPRIDEN.SPRIDEN_MI,SPRIDEN.SPRIDEN_ID";
            return FetchDataSet(sql);
        }

        public static DataSet LoadClassesBySemester(string classid)
        {
            string sql = "select SSBSECT.SSBSECT_CRN,SSBSECT.SSBSECT_SUBJ_CODE,SSBSECT.SSBSECT_CRSE_NUMB,SCBCRSE.SCBCRSE_TITLE,SSBSECT.SSBSECT_SEATS_AVAIL,SSBSECT.SSBSECT_SSTS_CODE,SSBSECT.SSBSECT_PTRM_START_DATE,SSBSECT.SSBSECT_PTRM_END_DATE,SSBSECT.SSBSECT_PTRM_CODE from SATURN.SSBSECT SSBSECT, SATURN.SCBCRSE SCBCRSE where ( SSBSECT.SSBSECT_SUBJ_CODE = SCBCRSE.SCBCRSE_SUBJ_CODE (+) and SSBSECT.SSBSECT_CRSE_NUMB = SCBCRSE.SCBCRSE_CRSE_NUMB (+) ) and ( SSBSECT.SSBSECT_TERM_CODE = " + classid + " ) order by SSBSECT.SSBSECT_SUBJ_CODE, SSBSECT.SSBSECT_CRSE_NUMB";
            return FetchDataSet(sql);
        }

        public static DataSet LoadTerms()
        {
            string sql = "select ROVTERM_CODE, ROVTERM_DESC from rovterm where rovterm_start_date > TO_DATE('01-MAY-2013', 'DD-MON-YYYY') and rovterm_start_date < TO_DATE('01-JAN-2999', 'DD-MON-YYYY') order by rovterm_start_date desc";
            return FetchDataSet(sql);
        }

        public static DataSet FetchDataSet(string queryA)
        {
            DataSet ds = new DataSet();
            OracleCommand cmd;
            OracleConnection connOra = new OracleConnection();
            
            try
            {
                // Reopen the connection
                connOra.ConnectionString = MakeConn();
                connOra.Open();
                // Create the command
                cmd = new OracleCommand(queryA);
                cmd.Connection = connOra;
                cmd.CommandType = CommandType.Text;
                // Execute
                OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                adapter.Fill(ds);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n\n" + ex.StackTrace + "\n\n" + ex.Source);
            }
            finally
            {
                connOra.Close();
                connOra.Dispose();
            }

            // Check for data
            if (ds.Tables.Count == 0) { return new DataSet(); }
            else { if (ds.Tables[0].Rows.Count == 0) { return new DataSet(); } }
            // Return
            return ds;
        }

        public static bool WriteData(string queryB)
        {
            bool yesno = false;
            OracleCommand cmd;
            OracleConnection connOra = new OracleConnection();

            try
            {
                // Reopen the connection
                connOra.ConnectionString = MakeConn();
                connOra.Open();
                // Create the command
                cmd = new OracleCommand(queryB);
                cmd.Connection = connOra;
                cmd.CommandType = CommandType.Text;
                // Execute
                cmd.ExecuteNonQuery();
                // This far means success
                yesno = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n\n" + ex.StackTrace + "\n\n" + ex.Source);
            }
            finally
            {
                connOra.Close();
                connOra.Dispose();
            }

            return yesno;
        }

        public static string MakeConn()
        {
            // Setup connection string
            if (App.Current.Resources["ID"].ToString().Length == 0) { throw new Exception("No id"); }
            if (App.Current.Resources["PW"].ToString().Length == 0) { throw new Exception("No password"); }
            if (App.Current.Resources["SERVER"].ToString().Length == 0) { throw new Exception("No server"); }
            if (App.Current.Resources["DB"].ToString().Length == 0) { throw new Exception("No database"); }
            return @"User ID=" + App.Current.Resources["ID"] + @";Password=" + App.Current.Resources["PW"] + @";Data Source=" + App.Current.Resources["SERVER"] + @":1521/" + App.Current.Resources["DB"];
        }

        public static void ErrMsg(string msg)
        {
            try
            {
                if (msg.Length == 0) { throw new Exception("No message passed for error"); }
                MessageBox.Show(msg, "ERROR", MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                ErrMsg(ex);
            }
        }

        public static void ErrMsg(Exception ex)
        {
            ErrMsg(ex.Message + "\n\n" + ex.StackTrace + "\n\n" + ex.Source);
        }
    }
}
