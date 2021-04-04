using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
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
    /// Logika interakcji dla klasy AddUserWindow.xaml
    /// </summary>
    public partial class AddUserWindow : Window
    {
        bool isEdit;
        FbConnection connection;
        int idUser;

        public AddUserWindow()
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
        }

        public AddUserWindow(User user)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            isEdit = true;
            AddUserW.Title = "Edycja czytelnika";
            HeaderL.Text = "Edycja czytelnika";
            nameTB.Text = user.IMIE;
            surnameTB.Text = user.NAZWISKO;
            loginTB.Text = user.LOGIN;
            emailTB.Text = user.EMAIL;
            birthdateDP.SelectedDate = user.DATA_URODZENIA;
            FbConnectionStringBuilder csb = new FbConnectionStringBuilder();
            csb.DataSource = "localhost";
            csb.Port = 3050;
            csb.Database = @"C:\bazy\IO.FDB";
            csb.UserID = "SYSDBA";
            csb.Password = "masterkey";
            csb.ServerType = FbServerType.Default;
            connection = new FbConnection(csb.ToString());
            connection.Open();
            idUser = user.ID_UZYTKOWNIK;
        }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            if(nameTB.Text == "")
            {
                MessageBox.Show("Podaj imię!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if(surnameTB.Text == "")
            {
                MessageBox.Show("Podaj nazwisko!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if(loginTB.Text == "")
            {
                MessageBox.Show("Uzupełnij login!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if(emailTB.Text == "")
            {
                MessageBox.Show("Uzupełnij adres e-mail!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if(birthdateDP.SelectedDate == null)
            {
                MessageBox.Show("Wybierz datę!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if(passwordTB.Text == "")
            {
                MessageBox.Show("Podaj hasło!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            FbCommand command = new FbCommand("select COUNT(*) from UZYTKOWNICY where EMAIL = '" + emailTB.Text + "'", connection);
            Int32 result = (Int32)command.ExecuteScalar();
            if(result != 0)
            {
                MessageBox.Show("Podany e-mail już został zarejestrowany!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            command = new FbCommand("select COUNT(*) from UZYTKOWNICY where LOGIN = '" + loginTB.Text + "'", connection);
            result = (Int32)command.ExecuteScalar();
            if (result != 0)
            {
                MessageBox.Show("Podany login już istnieje!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            command = new FbCommand("select COUNT(*) from UZYTKOWNICY where HASLO = '" + sha256_hash(passwordTB.Text) + "'", connection);
            result = (Int32)command.ExecuteScalar();
            if (result != 0)
            {
                MessageBox.Show("Podane hasło już istnieje!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            int result2;
            DateTime dt = (DateTime)birthdateDP.SelectedDate;
            string dateString = dt.Day.ToString() + '.' + dt.Month.ToString() + '.' + dt.Year.ToString();

            if (isEdit == false)
            {
                command = new FbCommand("insert into UZYTKOWNICY (LOGIN, IMIE, NAZWISKO, EMAIL, DATA_URODZENIA, HASLO) values ('" + loginTB.Text + "', '" + nameTB.Text + "', '" +
                    surnameTB.Text + "', '" + emailTB.Text + "', '" + dateString + "', '" + sha256_hash(passwordTB.Text) + "')", connection);
            }
            else
            {
                command = new FbCommand("update UZYTKOWNICY set LOGIN = '" + loginTB.Text + "', IMIE = '" + nameTB.Text + "', NAZWISKO = '" + surnameTB.Text + "', EMAIL = '" +
                    emailTB.Text + "', DATA_URODZENIA = '" + dateString + "', HASLO = '" + sha256_hash(passwordTB.Text) + "'", connection);
            }
            result2 = command.ExecuteNonQuery();
            if(result2 > 0)
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

        private void birthdateDP_Initialized(object sender, EventArgs e)
        {
            birthdateDP.DisplayDateEnd = DateTime.Now;
        }

        public static String sha256_hash(string value)
        {
            StringBuilder Sb = new StringBuilder();

            using (var hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }
            Console.WriteLine(Sb.ToString());
            return Sb.ToString();
        }
    }
}