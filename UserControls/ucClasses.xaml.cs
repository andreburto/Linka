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
        // These top two should be Nullable, but in a rush, so compromise!
        private ucRosters _rosterlist;
        private ucTeachers _classteachers;
        private bool yesnoR = false;
        private bool yesnoT = false;

        public ucRosters Roster { set { yesnoR = true;  _rosterlist = value; } }
        public ucTeachers Teachers { set { yesnoT = true; _classteachers = value; } }

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
            if (dataGrid1.Items.Count == 0) { return; }

            // If a roster form is set Load the datagrid
            if (yesnoR == true)
            {
                _rosterlist.UpdateForm((DataRowView)dataGrid1.SelectedItem);
            }
            
            // If a teacher form is set Load the datagrid
            if (yesnoT == true)
            {
                _classteachers.UpdateForm((DataRowView)dataGrid1.SelectedItem);
            }
        }



    }
}
