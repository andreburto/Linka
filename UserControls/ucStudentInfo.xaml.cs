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
    /// Interaction logic for ucStudentInfo.xaml
    /// </summary>
    public partial class ucStudentInfo : UserControl
    {
        public ucStudentInfo()
        {
            InitializeComponent();
        }

        public void UpdateForm(DataRow dr)
        {
            // Build the name from data
            string name = "";
            if (dr["SPRIDEN_FIRST_NAME"].ToString().Length > 0) { name = dr["SPRIDEN_FIRST_NAME"].ToString(); }
            if (dr["SPRIDEN_MI"].ToString().Length > 0) { name += " " + dr["SPRIDEN_MI"].ToString(); }
            if (dr["SPRIDEN_LAST_NAME"].ToString().Length > 0) { name += " " + dr["SPRIDEN_LAST_NAME"].ToString(); }

            // Set the ucStuLabel controls
            lblName.Set("Student Name", name);
            lblId.Set("Student ID", dr["SPRIDEN_ID"].ToString());
            lblSsn.Set("Student SSN", dr["SPBPERS_SSN"].ToString());
            lblBirth.Set("Birth Date", dr["SPBPERS_BIRTH_DATE"].ToString());
            lblGender.Set("Gender", dr["SPBPERS_SEX"].ToString());

            // Set the address datagrid
            if (dr["SPRIDEN_PIDM"].ToString().Length == 0) { return; }
            DataSet ds = DbStuff.LoadStudentAddresses(dr["SPRIDEN_PIDM"].ToString());
            if (ds.Tables.Count == 0) { return; }
            if (ds.Tables[0].Rows.Count == 0) { return; }
            dataGrid1.ItemsSource = ds.Tables[0].DefaultView;
        }

        // Clear the controls
        public void Reset()
        {
            // Reset labels
            lblName.Reset();
            lblId.Reset();
            lblSsn.Reset();
            lblGender.Reset();
            lblBirth.Reset();
            // Reset the data grids
            dataGrid1.ItemsSource = null;
        }
    }
}
