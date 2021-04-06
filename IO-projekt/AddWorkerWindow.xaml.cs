using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
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
    /// Interaction logic for AddWorkerWindow.xaml
    /// </summary>
    public partial class AddWorkerWindow : Window
    {
        bool isEdit;
        FbConnection connection;
        int idUser, idRole;

        public AddWorkerWindow()
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
            idUser = 0;
            idRole = 0;
            BindData();
        }

        public AddWorkerWindow(Worker worker)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            isEdit = true;
            AddWorkerW.Title = "Edycja czytelnika";
            HeaderL.Text = "Edycja czytelnika";
            nameTB.Text = worker.IMIE;
            surnameTB.Text = worker.NAZWISKO;
            loginTB.Text = worker.LOGIN;
            FbConnectionStringBuilder csb = new FbConnectionStringBuilder();
            csb.DataSource = (string)Application.Current.Properties["dataSource"];
            csb.Port = 3050;
            csb.Database = (string)Application.Current.Properties["dataBase"];
            csb.UserID = (string)Application.Current.Properties["userID"];
            csb.Password = (string)Application.Current.Properties["password"];
            csb.ServerType = FbServerType.Default;
            connection = new FbConnection(csb.ToString());
            connection.Open();
            idUser = worker.ID_PRACOWNIK;
            idRole = worker.ID_ROLA;
            BindData();
        }

        private void BindData()
        {
            DataTable dataTable = new DataTable();
            FbCommand command = new FbCommand("select ID_ROLA, ROLA from ROLE order by ROLA", connection);

            var reader = command.ExecuteReader();

            dataTable.Columns.Add("ID_ROLA", typeof(int));
            dataTable.Columns.Add("ROLA", typeof(string));

            while (reader.Read())
            {
                IDataRecord record = reader;
                dataTable.Rows.Add((int)record[0], (string)record[1]);
            }

            roleCB.ItemsSource = dataTable.DefaultView;

            if (idRole != 0)
            {
                DataRow[] row = dataTable.Select("ID_ROLA = " + idRole);
                int index = dataTable.Rows.IndexOf(row[0]);
                roleCB.SelectedIndex = index;
            }
        }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            if (nameTB.Text == "")
            {
                MessageBox.Show("Podaj imię!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (surnameTB.Text == "")
            {
                MessageBox.Show("Podaj nazwisko!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (loginTB.Text == "")
            {
                MessageBox.Show("Uzupełnij login!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (idRole == 0)
            {
                MessageBox.Show("Wybierz kategorię!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (passwordTB.Text == "")
            {
                MessageBox.Show("Podaj hasło!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            FbCommand command = new FbCommand("select COUNT(*) from PRACOWNICY where LOGIN = '" + loginTB.Text + "' AND ID_PRACOWNIK <> " + idUser, connection);
            Int32 result = (Int32)command.ExecuteScalar();
            if (result != 0)
            {
                MessageBox.Show("Podany login już istnieje!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            command = new FbCommand("select COUNT(*) from PRACOWNICY where HASLO = '" + AddUserWindow.sha256_hash(passwordTB.Text) + "' AND ID_PRACOWNIK <> " + idUser, connection);
            result = (Int32)command.ExecuteScalar();
            if (result != 0)
            {
                MessageBox.Show("Podane hasło już istnieje!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int result2;
            if (isEdit == false)
            {
                command = new FbCommand("insert into PRACOWNICY (LOGIN, IMIE, NAZWISKO, ID_ROLA, HASLO) values ('" + loginTB.Text + "', '" + nameTB.Text + "', '" +
                    surnameTB.Text + "', '" + idRole + "', '" + AddUserWindow.sha256_hash(passwordTB.Text) + "')", connection);
            }
            else
            {
                command = new FbCommand("update PRACOWNICY set LOGIN = '" + loginTB.Text + "', IMIE = '" + nameTB.Text + "', NAZWISKO = '" + surnameTB.Text + "', ID_ROLA = '" + idRole + "', HASLO = '" + AddUserWindow.sha256_hash(passwordTB.Text) + "' where ID_PRACOWNIK = " + idUser, connection);
            }

            result2 = command.ExecuteNonQuery();
            if (result2 > 0)
            {
                MessageBox.Show("Operacja zakończona powodzeniem!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Operacja nie powiodła się!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void roleCB_Loaded(object sender, RoutedEventArgs e)
        {
            BindData();
        }

        private void roleCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;
            DataRowView row = (DataRowView)e.AddedItems[0];
            idRole = (int)row["ID_ROLA"];
        }
    }
}
