﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IO_projekt
{
    /// <summary>
    /// Interaction logic for RentWindow.xaml
    /// </summary>
    public partial class RentWindow : Window
    {
        public RentWindow()
        {
            InitializeComponent();
        }

        private void rentB_Click(object sender, RoutedEventArgs e)
        {
            RentABook addWindow = new RentABook();
            addWindow.ShowDialog();
        }

        private void editB_Click(object sender, RoutedEventArgs e)
        {

        }

        private void returnB_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
