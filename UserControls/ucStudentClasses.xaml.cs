﻿using System;
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
    /// Interaction logic for ucStudentClasses.xaml
    /// </summary>
    public partial class ucStudentClasses : UserControl
    {
        public ucStudentClasses()
        {
            InitializeComponent();
        }

        public void UpdateForm(string pidm)
        {
            dataGrid1.ItemsSource = null;
            DataSet ds = DbStuff.LoadStudentClasses(pidm, App.Current.Resources["TERM"].ToString());
            if (ds.Tables.Count > 0) { dataGrid1.ItemsSource = ds.Tables[0].DefaultView; }
        }

        public void ClearForm()
        {
            dataGrid1.ItemsSource = null;
        }
    }
}
