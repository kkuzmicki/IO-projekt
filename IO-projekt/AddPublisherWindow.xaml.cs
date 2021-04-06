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
            isEdit = true;
            FbConnectionStringBuilder csb = new FbConnectionStringBuilder();
            csb.DataSource = "localhost";
            csb.Port = 3050;
            csb.Database = @"C:\bazy\IO.FDB";
            csb.UserID = "SYSDBA";
            csb.Password = "masterkey";
            csb.ServerType = FbServerType.Default;
            connection = new FbConnection(csb.ToString());
            connection.Open();
            id = publisher.ID_WYDAWNICTWO;
            HeaderL.Text = "Edycja";
            AddPublisherW.Title = "Edycja wydawnictwa";
            nameTB.Text = publisher.WYDAWNICTWO;
        }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            if (nameTB.Text == "")
            {
                MessageBox.Show("Podaj nazwę wydawnictwa!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            FbCommand command = new FbCommand("select count(*) from WYDAWNICTWA where WYDAWNICTWO = '" + nameTB.Text + "' and ID_WYDAWNICTWO <> " + id, connection);
            Int32 result = (Int32)command.ExecuteScalar();
            if (result > 0)
            {
                MessageBox.Show("Podane wydawnictwo już istnieje!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                if (isEdit)
                {
                    command = new FbCommand("update WYDAWNICTWA set WYDAWNICTWO = '" + nameTB.Text + "' where ID_WYDAWNICTWO = " + id, connection);
                    result = command.ExecuteNonQuery();
                }
                else
                {
                    command = new FbCommand("insert into WYDAWNICTWA (WYDAWNICTWO) values ('" + nameTB.Text + "')", connection);
                    result = command.ExecuteNonQuery();
                }
            }
            if (result != 0)
            {
                MessageBox.Show("Operacja powiodła się!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Operacja nie powiodła się!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
