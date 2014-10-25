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
        private bool _yesnoSI = false;
        private bool _yesnoSC = false;

        public ucStudentInfo StudentInfo { set { _yesnoSI = true; _studentinfo = value; } }
        public ucStudentClasses StudentClasses { set { _yesnoSC = true; _studentclasses = value; } }

        public ucFindStudents()
        {
            InitializeComponent();

            // Set embeded user controls
            StudentInfo = ucInformationOfStudent;
            StudentClasses = ucClassesOfStudent;
        }

        private void txtSsn_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtSsn.Text.Length == 0)
            {
                btnSsn.IsEnabled = false;
            }
            else
            {
                btnSsn.IsEnabled = true;
            }
        }

        private void txtId_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtPidm_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnSsn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnId_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnPidm_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ResetForm()
        {
            // Reset this form
            txtId.Text = "";
            txtSsn.Text = "";
            txtPidm.Text = "";
            // Reset user controls
            _studentclasses.ClearForm();
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

            }
        }

        /* Publically accessible functions that update the embedded user controls via the above private function */
        public void UpdateForm(DataRowView cr)
        {
            try
            {
                DataSet ds = DbStuff.LoadStudentInfoById(cr[0].ToString());
                if (ds.Tables.Count == 0) { throw new Exception("No tables found."); }
                if (ds.Tables[0].Rows.Count == 0) { throw new Exception("No records found."); }
                UpdateControls(ds.Tables[0].Rows[0]);
            }
            catch(Exception ex)
            {
                DbStuff.ErrMsg(ex);
            }
        }
    }
}
