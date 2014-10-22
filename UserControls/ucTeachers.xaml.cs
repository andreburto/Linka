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
    /// Interaction logic for ucTeachers.xaml
    /// </summary>
    public partial class ucTeachers : UserControl
    {
        public ucTeachers()
        {
            InitializeComponent();
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateTeacherInfo((DataRowView)dataGrid1.SelectedItem);
        }

        public void UpdateForm(DataRowView cr)
        {
            // Update the datagrid
            dataGrid1.ItemsSource = null;
            DataSet ds = DbStuff.LoadTeachersOfClass(cr[0].ToString(), App.Current.Resources["TERM"].ToString());
            if (ds.Tables.Count > 0) { dataGrid1.ItemsSource = ds.Tables[0].DefaultView; }

            // Update the title
            string title = cr[1].ToString() + cr[2].ToString() + " " + cr[3].ToString() + " - " + cr[0].ToString();
            txtLabel.Text = title;
        }

        public void UpdateTeacherInfo(DataRowView cr)
        {
            dataGrid2.ItemsSource = null;
            DataSet ds = DbStuff.LoadTeacherInfo(cr[0].ToString());
            if (ds.Tables.Count > 0) { dataGrid2.ItemsSource = ds.Tables[0].DefaultView; }
        }
    }
}
