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
    /// Interaction logic for AddAuthorWindow.xaml
    /// </summary>
    public partial class AddAuthorWindow : Window
    {
        bool isEdit;
        int id;
        FbConnection connection;

        public AddAuthorWindow()
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

        public AddAuthorWindow(Author author)
        {
            InitializeComponent();
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
            id = author.ID_AUTOR;
            nameTB.Text = author.IMIE;
            surnameTB.Text = author.NAZWISKO;
            HeaderL.Text = "Edycja autora";
            AddAuthorW.Title = "Edycja autora";
        }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            if (nameTB.Text == "")
            {
                MessageBox.Show("Podaj imię autora!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (surnameTB.Text == "")
            {
                MessageBox.Show("Podaj nazwisko autora!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            FbCommand command = new FbCommand("select count(*) from AUTORZY where IMIE = '" + nameTB.Text + "' and NAZWISKO = '" + surnameTB.Text + "' " +
                "and ID_AUTOR <> " + id, connection);
            Int32 result = (Int32)command.ExecuteScalar();
            if (result > 0)
            {
                MessageBox.Show("Podany autor już istnieje!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                if (isEdit)
                {
                    command = new FbCommand("update AUTORZY set IMIE = '" + nameTB.Text + "' and NAZWISKO = '" + surnameTB.Text + "' where ID_AUTOR = " + id, connection);
                    result = command.ExecuteNonQuery();
                }
                else
                {
                    command = new FbCommand("insert into AUTORZY (IMIE, NAZWISKO) values ('" + nameTB.Text + "', '" + surnameTB.Text + "')", connection);
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
