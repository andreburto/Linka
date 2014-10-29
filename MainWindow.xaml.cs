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
using Oracle.DataAccess.Client;
using Linka;

namespace Linka
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Creds cred = new Creds();
        public string connStr = "";
        public string term = "";

        public MainWindow()
        {
            InitializeComponent();

            // LOGIN CREDENTIALS
            if (cred.IsFile)
            {
                cred.Load();
                App.Current.Resources["ID"] = cred.Id;
                App.Current.Resources["PW"] = cred.Pw;
                App.Current.Resources["SERVER"] = cred.Server;
                App.Current.Resources["DB"] = cred.Database;
            }
            else
            {
                Login l = new Login();
                l.ShowDialog();
                if (l.DialogResult == false)
                {
                    this.Close();
                    return;
                }
                else
                {
                    cred.Load();
                    App.Current.Resources["ID"] = cred.Id;
                    App.Current.Resources["PW"] = cred.Pw;
                    App.Current.Resources["SERVER"] = cred.Server;
                    App.Current.Resources["DB"] = cred.Database;
                }
            }

            // SELECT A TERM
            if (Terms.IsFile == true)
            {
                App.Current.Resources["TERM"] = Terms.Read();
            }
            else
            {
                frmTerm t = new frmTerm();
                t.ShowDialog();
                if (t.DialogResult == false)
                {
                    this.Close();
                    return;
                }
                else
                {
                    App.Current.Resources["TERM"] = Terms.Read();
                }
            }

            // Set the local term variable
            term = App.Current.Resources["TERM"].ToString();

            // LOAD the main class
            ucClassesBySemester.UpdateTable(term);

            // Set UserControl dependency chain
            ucClassesBySemester.Roster = ucRostersByClass;
            ucClassesBySemester.Teachers = ucTeachersOfClass;
            ucRostersByClass.FindStudents = ucFindAStudent;
        }

        private void mConnection_Click(object sender, RoutedEventArgs e)
        {
            Login l = new Login();
            l.ShowDialog();
            if (l.DialogResult == true)
            {
                cred.Load();
                App.Current.Resources["ID"] = cred.Id;
                App.Current.Resources["PW"] = cred.Pw;
                App.Current.Resources["SERVER"] = cred.Server;
                App.Current.Resources["DB"] = cred.Database;
            }
        }

        private void mAbout_Click(object sender, RoutedEventArgs e)
        {
            frmAbout fa = new frmAbout();
            fa.ShowDialog();
        }

        private void mExit_Click(object sender, RoutedEventArgs e)
        {
            cred.Save();
            this.Close();
        }

        private void mSemester_Click(object sender, RoutedEventArgs e)
        {
            frmTerm t = new frmTerm();
            t.ShowDialog();
            if (t.DialogResult == true)
            {
                term = App.Current.Resources["TERM"].ToString();
                ucClassesBySemester.UpdateTable(term);
            }
            else
            {
                App.Current.Resources["TERM"] = term;
            }
        }

        private void mGoogle_Click(object sender, RoutedEventArgs e)
        {
            frmGoogle g = new frmGoogle();
            g.ShowDialog();
        }
    }
}
