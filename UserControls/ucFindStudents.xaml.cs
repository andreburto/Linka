using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Linka;

namespace Linka
{
    /// <summary>
    /// Interaction logic for ucFindStudents.xaml
    /// </summary>
    public partial class ucFindStudents : UserControl
    {
        private ucStudentInfo _studentinfo;
        private ucStudentClasses _studentclasses;
        private ucStudentEmail _studentemail;
        private bool _yesnoSI = false;
        private bool _yesnoSC = false;
        private bool _yesnoSE = false;

        public ucStudentInfo StudentInfo { set { _yesnoSI = true; _studentinfo = value; } }
        public ucStudentClasses StudentClasses { set { _yesnoSC = true; _studentclasses = value; } }
        public ucStudentEmail StudentEmail { set { _yesnoSE = true; _studentemail = value; } }

        public ucFindStudents()
        {
            InitializeComponent();

            // Set embeded user controls
            StudentInfo = ucInformationOfStudent;
            StudentClasses = ucClassesOfStudent;
        }

        private void txtSsn_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtSsn.Text.Length == 0) { btnSsn.IsEnabled = false; }
            else { btnSsn.IsEnabled = true; }
        }

        private void txtId_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtId.Text.Length == 0) { btnId.IsEnabled = false; }
            else { btnId.IsEnabled = true; }
        }

        private void txtPidm_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtPidm.Text.Length == 0) { btnPidm.IsEnabled = false; }
            else { btnPidm.IsEnabled = true; }
        }

        private void txtOldId_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtOldId.Text.Length == 0) { btnOldId.IsEnabled = false; }
            else { btnOldId.IsEnabled = true; }
        }

        private void btnSsn_Click(object sender, RoutedEventArgs e)
        {
            string ssn = txtSsn.Text.Replace("-", "").Replace(" ", "");
            if (ssn.Length != 9)
            {
                DbStuff.ErrMsg("Not a properly formatted Social Security Number.");
                txtSsn.Text = "";
                return;
            }
            CheckResults(DbStuff.LoadStudentInfoBySsn(ssn));
            txtSsn.Text = "";
        }

        private void btnId_Click(object sender, RoutedEventArgs e)
        {
            string id = txtId.Text.Replace(" ", "").ToUpper();
            if (id.Substring(0, 1) != "W")
            {
                DbStuff.ErrMsg("You are not sending a Banner ID. Results by vary.");
            }
            CheckResults(DbStuff.LoadStudentInfoById(id));
            txtId.Text = "";
        }

        private void btnPidm_Click(object sender, RoutedEventArgs e)
        {
            string pidm = txtSsn.Text.Replace(" ", "");
            CheckResults(DbStuff.LoadStudentInfoByPidm(pidm));
            txtPidm.Text = "";
        }

        private void btnOldId_Click(object sender, RoutedEventArgs e)
        {
            string oldid = txtOldId.Text.Replace(" ", "");
            CheckResults(DbStuff.LoadStudentByOldId(oldid));
            txtOldId.Text = "";
        }

        private void ResetForm()
        {
            // Reset this form
            txtId.Text = "";
            txtSsn.Text = "";
            txtPidm.Text = "";
            // Reset user controls
            if (_yesnoSC == true) { _studentclasses.ClearForm(); }
            if (_yesnoSI == true) { _studentinfo.Reset(); }
        }

        /* Private function that updates the embedded user controls */
        private void UpdateControls(DataRow dr)
        {
            // If the ucStudentClasses control is linked in...
            if (_yesnoSC == true)
            {
                _studentclasses.UpdateForm(dr["SPRIDEN_PIDM"].ToString());
            }

            // If the ucStudentInfo control is linked in...
            if (_yesnoSI == true)
            {
                _studentinfo.UpdateForm(dr);
            }

            // Update the external ucFixStudentEmail tab...
            if (_yesnoSE == true)
            {
                _studentemail.UpdateForm(dr);
            }
        }

        // Trying to filter DataSets through one function
        private void CheckResults(DataSet ds)
        {
            try
            {
                if (ds.Tables.Count == 0) { throw new Exception("No tables found."); }
                if (ds.Tables[0].Rows.Count == 0) { throw new Exception("No records found."); }
                UpdateControls(ds.Tables[0].Rows[0]);
            }
            catch (Exception ex)
            {
                DbStuff.ErrMsg(ex.Message);
            }
        }

        /* Publically accessible functions that update the embedded user controls via the above private function */
        public void UpdateForm(DataRowView cr)
        {
            try
            {
                CheckResults(DbStuff.LoadStudentInfoById(cr[0].ToString()));
            }
            catch(Exception ex)
            {
                DbStuff.ErrMsg(ex);
            }
        }
    }
}
