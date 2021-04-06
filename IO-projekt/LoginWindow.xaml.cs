using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IO_projekt
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        int hour;
        FbConnection connection;
        public LoginWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            hour = DateTime.Now.Hour;
            if (hour >= 18 || hour <= 6)
            {
                welcomeL.Text = "Dobry wieczór!";
            }
            try
            {
                if (File.Exists("CONF.txt"))
                {
                    StreamReader sr = new StreamReader("CONF.txt");
                    Application.Current.Properties["dataSource"] = sr.ReadLine();
                    Application.Current.Properties["dataBase"] = sr.ReadLine();
                    Application.Current.Properties["userID"] = sr.ReadLine();
                    Application.Current.Properties["password"] = sr.ReadLine();
                    sr.Close();
                }
                else
                {
                    Application.Current.Properties["dataSource"] = "localhost";
                    Application.Current.Properties["dataBase"] = @"C:\bazy\IO.FDB";
                    Application.Current.Properties["userID"] = "SYSDBA";
                    Application.Current.Properties["password"] = "masterkey";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd pliku: \n" + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Properties["dataSource"] = "localhost";
                Application.Current.Properties["dataBase"] = @"C:\bazy\IO.FDB";
                Application.Current.Properties["userID"] = "SYSDBA";
                Application.Current.Properties["password"] = "masterkey";
            }
            makeConnection();
        }

        private void makeConnection()
        {
            FbConnectionStringBuilder csb = new FbConnectionStringBuilder();
            //csb.DataSource = "localhost";
            //csb.Port = 3050;
            //csb.Database = @"C:\bazy\IO.FDB";
            //csb.UserID = "SYSDBA";
            //csb.Password = "masterkey";
            //csb.ServerType = FbServerType.Default;
            csb.DataSource = (string)Application.Current.Properties["dataSource"];
            csb.Port = 3050;
            csb.Database = (string)Application.Current.Properties["dataBase"];
            csb.UserID = (string)Application.Current.Properties["userID"];
            csb.Password = (string)Application.Current.Properties["password"];
            csb.ServerType = FbServerType.Default;
            connection = new FbConnection(csb.ToString());
            try
            {
                connection.Open();
                loginTB.IsEnabled = true;
                passwordPB.IsEnabled = true;
                loginB.IsEnabled = true;
            }
            catch(Exception e)
            {
                MessageBox.Show("Błąd połączenia z bazą danych:\n" + e.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                loginTB.IsEnabled = false;
                passwordPB.IsEnabled = false;
                loginB.IsEnabled = false;
            }
        }

        private void loginB_click(object sender, RoutedEventArgs e)
        {
            String pass = sha256_hash(passwordPB.Password);

            var command2 = new FbCommand("select count(*) from PRACOWNICY where LOGIN = '" + loginTB.Text + "' AND HASLO = '" + pass + "'", connection);
            Int32 count = (Int32)command2.ExecuteScalar();
            if(count > 0)
            {
                Int32 id = (Int32)new FbCommand("select ID_ROLA from PRACOWNICY where LOGIN = '" + loginTB.Text + "' AND HASLO = '" + pass + "'", connection).ExecuteScalar();
                MainWindow objSecondWindow = new MainWindow(id);
                this.Close();
                objSecondWindow.Show();
            }
            else
            {
                errorTB.Text = "Błędne dane";
            }
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

        private void settingsB_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.ShowDialog();
            connection.Close();
            makeConnection();
        }
    }
}
