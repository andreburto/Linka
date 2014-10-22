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

        public static DataSet LoadTeachersOfClass(string classid, string term)
        {
            string sql = "select SPRIDEN.SPRIDEN_PIDM,SPRIDEN.SPRIDEN_ID,SPRIDEN.SPRIDEN_LAST_NAME,SPRIDEN.SPRIDEN_FIRST_NAME,SPRIDEN.SPRIDEN_MI from SATURN.SIRASGN SIRASGN left join SATURN.SPRIDEN SPRIDEN on SIRASGN.SIRASGN_PIDM = SPRIDEN.SPRIDEN_PIDM where SPRIDEN.SPRIDEN_CHANGE_IND is null and SIRASGN.SIRASGN_TERM_CODE ='"+term+"' and SIRASGN.SIRASGN_CRN ='"+classid+"' order by SPRIDEN.SPRIDEN_LAST_NAME,SPRIDEN.SPRIDEN_FIRST_NAME,SPRIDEN.SPRIDEN_MI,SPRIDEN.SPRIDEN_ID";
            return FetchDataSet(sql);
        }

        public static DataSet LoadTeacherInfo(string pidm)
        {
            string sql = "select * from SATURN.SPRIDEN SPRIDEN where SPRIDEN.SPRIDEN_PIDM ="+pidm+" order by SPRIDEN_ACTIVITY_DATE desc";
            return FetchDataSet(sql);
        }

        public static DataSet LoadStudentInfo(string pidm)
        {
            string sql = "";
            return FetchDataSet(sql);
        }

        public static DataSet LoadStudentClasses(string pidm, string term)
        {
            string sql = "";
            return FetchDataSet(sql);
        }

        public static DataSet LoadRostersByClass(string classid, string term)
        {
            string sql = "select SPRIDEN.SPRIDEN_PIDM,SPRIDEN.SPRIDEN_ID,SPRIDEN.SPRIDEN_FIRST_NAME,SPRIDEN.SPRIDEN_LAST_NAME,STVRSTS.STVRSTS_CODE,STVRSTS.STVRSTS_DESC,NVL(SFRSTCR.SFRSTCR_GRDE_CODE_MID,'NULL') SFRSTCR_GRDE_CODE_MID from SATURN.SFRSTCR SFRSTCR,SATURN.SPRIDEN SPRIDEN,SATURN.STVRSTS STVRSTS where (SFRSTCR.SFRSTCR_PIDM = SPRIDEN.SPRIDEN_PIDM (+) and SFRSTCR.SFRSTCR_RSTS_CODE = STVRSTS.STVRSTS_CODE (+)) and (SFRSTCR.SFRSTCR_TERM_CODE="+term+" and SFRSTCR.SFRSTCR_CRN="+classid+" and SPRIDEN.SPRIDEN_CHANGE_IND is null ) order by SPRIDEN.SPRIDEN_LAST_NAME,SPRIDEN.SPRIDEN_FIRST_NAME,SPRIDEN.SPRIDEN_MI,SPRIDEN.SPRIDEN_ID";
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
