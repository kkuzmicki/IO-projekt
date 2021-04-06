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
    /// Interaction logic for AddRoleWindow.xaml
    /// </summary>
    public partial class AddRoleWindow : Window
    {
        bool isEdit;
        int id;
        FbConnection connection;
        public AddRoleWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            isEdit = false;
            FbConnectionStringBuilder csb = new FbConnectionStringBuilder();
            csb.DataSource = (string)Application.Current.Properties["dataSource"];
            csb.Port = 3050;
            csb.Database = (string)Application.Current.Properties["dataBase"];
            csb.UserID = (string)Application.Current.Properties["userID"];
            csb.Password = (string)Application.Current.Properties["password"];
            csb.ServerType = FbServerType.Default;
            connection = new FbConnection(csb.ToString());
            connection.Open();
            id = 0;
        }

        public AddRoleWindow(Role role)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            isEdit = true;
            FbConnectionStringBuilder csb = new FbConnectionStringBuilder();
            csb.DataSource = (string)Application.Current.Properties["dataSource"];
            csb.Port = 3050;
            csb.Database = (string)Application.Current.Properties["dataBase"];
            csb.UserID = (string)Application.Current.Properties["userID"];
            csb.Password = (string)Application.Current.Properties["password"];
            csb.ServerType = FbServerType.Default;
            connection = new FbConnection(csb.ToString());
            connection.Open();

            id = role.ID_ROLA;
            nameTB.Text = role.ROLA;
            HeaderL.Text = "Edycja kategorii";
            AddRoleW.Title = "Edycja kategorii";
        }
        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            if (nameTB.Text == "")
            {
                MessageBox.Show("Podaj nazwę roli!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            FbCommand command = new FbCommand("select count(*) from ROLE where ROLA = '" + nameTB.Text + "' and ID_ROLA <> " + id, connection);
            Int32 result = (Int32)command.ExecuteScalar();
            if (result > 0)
            {
                MessageBox.Show("Podana kategoria już istnieje!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                if (isEdit)
                {
                    command = new FbCommand("update ROLE set ROLA = '" + nameTB.Text + "' where ID_ROLA = " + id, connection);
                    result = command.ExecuteNonQuery();
                }
                else
                {
                    command = new FbCommand("insert into ROLE (ROLA) values ('" + nameTB.Text + "')", connection);
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
