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
    /// Interaction logic for ucRosters.xaml
    /// </summary>
    public partial class ucRosters : UserControl
    {
        private ucFindStudents _findstudents;
        private bool _fsyesno = false;

        public ucFindStudents FindStudents { set { _fsyesno = true; _findstudents = value; } }

        public ucRosters()
        {
            InitializeComponent();
        }

        public void UpdateForm(DataRowView cr)
        {
            // Update the datagrid
            dataGrid1.ItemsSource = null;
            DataSet ds = DbStuff.LoadRostersByClass(cr[0].ToString(), App.Current.Resources["TERM"].ToString());
            if (ds.Tables.Count > 0) { dataGrid1.ItemsSource = ds.Tables[0].DefaultView; }

            // Update the title
            string title = cr[1].ToString() + cr[2].ToString() + " " + cr[3].ToString() + " - " + cr[0].ToString();
            if (dataGrid1.HasItems) { title += " | Roster: " + ds.Tables[0].Rows.Count.ToString(); }
            txtLabel.Text = title;
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_fsyesno == false) { return; }
        }
    }
}
