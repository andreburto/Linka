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
        public ucStudentEmail()
        {
            InitializeComponent();
        }

        public void UpdateForm(DataRow dr)
        {

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
    }
}
