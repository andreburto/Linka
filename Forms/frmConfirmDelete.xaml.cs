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
using Linka;

namespace Linka
{
    /// <summary>
    /// Interaction logic for frmConfirmDelete.xaml
    /// </summary>
    public partial class frmConfirmDelete : Window
    {
        protected const string _MSG = "Are you sure you wish to delete {0}?";

        public frmConfirmDelete(string userid)
        {
            InitializeComponent();

            txtConfirm.Text = String.Format(_MSG, userid);
        }

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
