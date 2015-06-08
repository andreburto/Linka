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
        protected EmailStuff _em;
        protected string _fn;
        protected string _ln;
        protected string _id;

        public ucStudentEmail()
        {
            InitializeComponent();
        }

        public void UpdateForm(DataRow dr)
        {
            // Clean managemnt fields
            txtEmailId.Text = "";
            txtPassword.Text = "";
            txtEmail.Text = "";

            // Set buttons
            btnCreate.IsEnabled = false;
            btnDelete.IsEnabled = false;
            btnPassword.IsEnabled = false;

            // Initialize object for each new user
            _em = new EmailStuff(App.Current.Resources["GID"].ToString(),
                                 App.Current.Resources["GSF"].ToString(),
                                 App.Current.Resources["GSD"].ToString());

            // Set general related info
            txtStuSsn.Text = dr["SPBPERS_SSN"].ToString();
            txtStuId.Text = dr["SPRIDEN_ID"].ToString();
            txtStuName.Text = String.Format("{0}, {1}", dr["SPRIDEN_LAST_NAME"].ToString(), dr["SPRIDEN_FIRST_NAME"].ToString());

            // Set local variables
            _ln = dr["SPRIDEN_LAST_NAME"].ToString();
            _fn = dr["SPRIDEN_FIRST_NAME"].ToString();
            _id = dr["SPRIDEN_ID"].ToString();

            // Check for email
            EmailStatus status = _em.FullCheck(_id);

            // Report Status
            switch (status.status())
            {
                case 1:
                    UpdateStatusLabel("Only in database.", Brushes.Red, Visibility.Visible);
                    break;
                case 2:
                    UpdateStatusLabel("Only in Google.", Brushes.Red, Visibility.Visible);
                    break;
                case 3:
                    UpdateStatusLabel("Exists.", Brushes.Green, Visibility.Visible);
                    txtEmail.Text = status.userid;
                    break;
                default:
                    UpdateStatusLabel("No such email", Brushes.Red, Visibility.Visible);
                    break;
            }
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
            try
            {
                if (txtEmail.Text.Length > 0) { throw new Exception("ID for user exists."); }
                if (txtEmailId.Text.Length == 0) { throw new Exception("You must input an ID."); }
                if (txtPassword.Text.Length == 0) { throw new Exception("You must input a password."); }
                if (_em.CreateEmail(txtEmailId.Text, _fn, _ln, txtPassword.Text))
                {
                    if (DbStuff.InsertEmail(txtStuId.Text, txtEmailId.Text, App.Current.Resources["TERM"].ToString()))
                    {
                        txtEmail.Text = txtEmailId.Text;
                        txtEmailId.Text = "";
                        txtPassword.Text = "";
                        UpdateStatusLabel("Created!", Brushes.Green, Visibility.Visible);
                    }
                    else
                    {
                        _em.DeleteEmail(txtEmailId.Text);
                        UpdateStatusLabel("Database error!", Brushes.Red, Visibility.Visible);
                        throw new Exception("Could not add to database.");
                    }
                }
                else
                {
                    UpdateStatusLabel("Error Creating!", Brushes.Red, Visibility.Visible);
                    throw new Exception("Could not create email.");
                }
            }
            catch(Exception ex)
            {
                DbStuff.ErrMsg(ex.Message);
            }
        }

        private void btnSuggest_Click(object sender, RoutedEventArgs e)
        {
            txtEmailId.Text = _em.SuggestId(_fn, _ln);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            frmConfirmDelete cd = new frmConfirmDelete(txtEmail.Text);

            try
            {
                cd.ShowDialog();
                if (cd.DialogResult == true)
                {
                    if (_em.DeleteEmail(txtEmail.Text))
                    {
                        if (DbStuff.DeleteEmailByUserid(txtEmail.Text))
                        {
                            UpdateStatusLabel("No address", Brushes.Red, Visibility.Visible);
                        }
                        else
                        {
                            throw new Exception("Could not complete; still in database.");
                        }
                    }
                    else
                    {
                        throw new Exception("Could not remove from Google.");
                    }
                }
            }
            catch (Exception ex)
            {
                DbStuff.ErrMsg(ex.Message);
            }
        }

        private void btnPassword_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtPassword.Text.Length == 0) { throw new Exception("You must input a password."); }
                if (_em.ChangePassword(txtEmail.Text, txtPassword.Text) == false)
                {
                    DbStuff.ErrMsg("Password update failed.");
                }
                else
                {
                    MessageBox.Show("Password changed!");
                    txtPassword.Text = "";
                }
            }
            catch(Exception ex)
            {
                DbStuff.ErrMsg(ex.Message);
            }
        }

        // You can only create an account if there's txt in txtEmailId
        private void txtEmailId_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtEmailId.Text.Length > 0) { btnCreate.IsEnabled = true; }
            else { btnCreate.IsEnabled = false; }
        }

        // You can only delete an account that exists
        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtEmail.Text.Length > 0) { btnDelete.IsEnabled = true; }
            else { btnDelete.IsEnabled = false; }
        }

        // You can only update a password with a password
        private void txtPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtPassword.Text.Length > 0) { btnPassword.IsEnabled = true; }
            else { btnPassword.IsEnabled = false; }
        }
    }
}
