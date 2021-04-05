using FirebirdSql.Data.FirebirdClient;
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
    /// Interaction logic for AddPublisherWindow.xaml
    /// </summary>
    public partial class AddPublisherWindow : Window
    {
        bool isEdit;
        FbConnection connection;
        int id;
        public AddPublisherWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            isEdit = false;
            FbConnectionStringBuilder csb = new FbConnectionStringBuilder();
            csb.DataSource = "localhost";
            csb.Port = 3050;
            csb.Database = @"C:\bazy\IO.FDB";
            csb.UserID = "SYSDBA";
            csb.Password = "masterkey";
            csb.ServerType = FbServerType.Default;
            connection = new FbConnection(csb.ToString());
            connection.Open();
            id = 0;
        }
        public AddPublisherWindow(Publisher publisher)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            isEdit = false;
            FbConnectionStringBuilder csb = new FbConnectionStringBuilder();
            csb.DataSource = "localhost";
            csb.Port = 3050;
            csb.Database = @"C:\bazy\IO.FDB";
            csb.UserID = "SYSDBA";
            csb.Password = "masterkey";
            csb.ServerType = FbServerType.Default;
            connection = new FbConnection(csb.ToString());
            connection.Open();
            id = 0;
            HeaderL.Text = "Edycja wydawnictwa";
            AddPublisherW.Title = "Edycja wydawnictwa";
            nameTB.Text = publisher.WYDAWNICTWO;
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
