using System;
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
    /// Interaction logic for AddRoleWindow.xaml
    /// </summary>
    public partial class AddRoleWindow : Window
    {
        bool isEdit;
        public AddRoleWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            isEdit = false;
        }

        public AddRoleWindow(Role role)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            isEdit = true;
        }
        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }        
    }
}
