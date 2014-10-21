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
    /// Interaction logic for ucClasses.xaml
    /// </summary>
    public partial class ucClasses : UserControl
    {
        private ucRosters _rosterlist;
        private bool yesno = false;

        public ucRosters Roster { set { yesno = true;  _rosterlist = value; } }

        public ucClasses()
        {
            InitializeComponent();
        }

        public void UpdateTable(string term)
        {
            dataGrid1.ItemsSource = null;
            DataSet ds = DbStuff.LoadClassesBySemester(term);

            if (ds.Tables.Count > 0)
            {
                dataGrid1.ItemsSource = ds.Tables[0].DefaultView;
            }
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Avoid erors
            if (yesno == false) { return; }
            if (dataGrid1.Items.Count == 0) { return; }
            // Load the datagrid
            DataRowView CurRow = (DataRowView)dataGrid1.SelectedItem;
            _rosterlist.UpdateTable(CurRow[0].ToString());   
        }


    }
}
