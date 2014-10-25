using System;
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
    /// Interaction logic for ucStuLabel.xaml
    /// </summary>
    public partial class ucStuLabel : UserControl
    {
        public ucStuLabel()
        {
            InitializeComponent();

            this.Reset();
        }

        public void Set(string key, string val)
        {
            txtKey.Visibility = System.Windows.Visibility.Visible;
            txtKey.Text = key;
            txtVal.Visibility = System.Windows.Visibility.Visible;
            txtVal.Text = val;
        }

        public void Reset()
        {
            txtKey.Text = "";
            txtKey.Visibility = System.Windows.Visibility.Hidden;
            txtVal.Text = "";
            txtVal.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}
