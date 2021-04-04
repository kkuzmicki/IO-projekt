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
    /// Logika interakcji dla klasy AddUserWindow.xaml
    /// </summary>
    public partial class AddUserWindow : Window
    {
        bool isEdit;
        FbConnection connection;

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

            FbCommand command = new FbCommand("select COUNT(*) from UZYTKOWNICY where EMAIL = '" + emailTB.Text + "'");
            Int32 result = (Int32)command.ExecuteScalar();
            if(result != 0)
            {
                MessageBox.Show("Podany e-mail już został zarejestrowany!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            command = new FbCommand("select COUNT(*) from UZYTKOWNICY where LOGIN = '" + loginTB.Text + "'");
            result = (Int32)command.ExecuteScalar();
            if (result != 0)
            {
                MessageBox.Show("Podany login już istnieje!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            command = new FbCommand("select COUNT(*) from UZYTKOWNICY where HASLO = '" + sha256_hash(passwordTB.Text) + "'");
            result = (Int32)command.ExecuteScalar();
            if (result != 0)
            {
                MessageBox.Show("Podane hasło już istnieje!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
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
