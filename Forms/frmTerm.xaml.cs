using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Linka;

namespace Linka
{
    /// <summary>
    /// Interaction logic for frmTerm.xaml
    /// </summary>
    public partial class frmTerm : Window
    {
        DataSet ds;
        public frmTerm()
        {
            InitializeComponent();

            txtTerm.Text = App.Current.Resources["TERM"].ToString();

            try
            {
                ds = DbStuff.LoadTerms();
                if (ds.Tables.Count == 0) { throw new Exception("No terms found."); }
                if (ds.Tables[0].Rows.Count == 0) { throw new Exception("No terms found."); }
                cmbTerms.ItemsSource = ds.Tables[0].DefaultView;
                cmbTerms.DisplayMemberPath = ds.Tables[0].Columns["ROVTERM_DESC"].ToString();
                cmbTerms.SelectedValuePath = ds.Tables[0].Columns["ROVTERM_CODE"].ToString();
            }
            catch (Exception ex)
            {
                DbStuff.ErrMsg(ex.Message);
                cmbTerms.IsEnabled = false;
            }
        }

        private void btnTerm_Click(object sender, RoutedEventArgs e)
        {
            Terms.Save(txtTerm.Text);
            App.Current.Resources["TERM"] = txtTerm.Text;
            this.DialogResult = true;
            this.Close();
        }

        private void cmbTerms_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtTerm.Text = cmbTerms.SelectedValue.ToString();
        }
    }
}
