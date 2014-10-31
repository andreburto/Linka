using System;
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

namespace Linka
{
    /// <summary>
    /// Interaction logic for frmGoogle.xaml
    /// </summary>
    public partial class frmGoogle : Window
    {
        private goocreds _gc;
        private string _id = "";
        private string _pw = "";
        private string _domain = "";

        public frmGoogle()
        {
            InitializeComponent();

            _gc = new goocreds();
            if (_gc.IsFile)
            {
                _gc.Load();
                txtUser.Text = _gc.Id;
                txtPassword.Text = _gc.Pw;
                txtDomain.Text = _gc.Server;
            }
            else
            {
                txtUser.Text = App.Current.Resources["GID"].ToString();
                txtPassword.Text = App.Current.Resources["GPW"].ToString();
                txtDomain.Text = App.Current.Resources["GSD"].ToString();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtUser.Text.Length > 0) { _id = txtUser.Text; } else { _id = "gid"; }
            if (txtPassword.Text.Length > 0) { _pw = txtPassword.Text; } else { _pw = "gpw"; }
            if (txtDomain.Text.Length > 0) { _domain = txtDomain.Text; } else { _domain = "google.com"; }

            App.Current.Resources["GID"] = _id;
            App.Current.Resources["GPW"] = _pw;
            App.Current.Resources["GSD"] = _domain;

            _gc.Save(_id, _pw, _domain);

            this.DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
