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
using System.Web;
using Linka;

namespace Linka
{
    /// <summary>
    /// Interaction logic for ucStudentEmail.xaml
    /// </summary>
    public partial class ucStudentEmail : UserControl
    {
        public EmailStuff _em;
        protected string _fn;
        protected string _ln;

        public ucStudentEmail()
        {
            InitializeComponent();

            // Initialize email object
            _em = new EmailStuff(App.Current.Resources["GID"].ToString(),
                                 App.Current.Resources["GPW"].ToString(),
                                 App.Current.Resources["GSD"].ToString());
        }

        public void UpdateForm(DataRow dr)
        {
            // Set general related info
            txtStuSsn.Text = dr["SPBPERS_SSN"].ToString();
            txtStuId.Text = dr["SPRIDEN_ID"].ToString();
            txtStuName.Text = String.Format("{0}, {1}", dr["SPRIDEN_LAST_NAME"].ToString(), dr["SPRIDEN_FIRST_NAME"].ToString());

            // Set local variables
            _ln = dr["SPRIDEN_LAST_NAME"].ToString();
            _fn = dr["SPRIDEN_FIRST_NAME"].ToString();

            // Check for email
            
        }

        public void HideStatusLabel()
        {
            UpdateStatusLabel("", Brushes.Black, System.Windows.Visibility.Hidden);
        }

        private void UpdateStatusLabel(string txt, Brush color, Visibility state)
        {
            txtStatus.Text = txt;
            txtStatus.Foreground = color;
            txtStatus.Visibility = state;
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSuggest_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnPassword_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
