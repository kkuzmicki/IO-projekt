using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
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

            FbConnectionStringBuilder csb = new FbConnectionStringBuilder();
            csb.DataSource = "localhost";
            csb.Port = 3050;
            csb.Database = @"C:\bazy\IO.FDB";
            csb.UserID = "SYSDBA";
            csb.Password = "masterkey";
            csb.ServerType = FbServerType.Default;

            connection = new FbConnection(csb.ToString());
            connection.Open();

            Console.WriteLine("SHA: " + sha256_hash("testŹ"));
        }

        private void loginB_click(object sender, RoutedEventArgs e)
        {
            var command2 = new FbCommand("select count(*) from PRACOWNICY where LOGIN = '" + loginTB.Text + "' AND HASLO = '" + passwordPB.Password + "'", connection);
            Int32 count = (Int32)command2.ExecuteScalar();
            if(count > 0)
            {
                Int32 id = (Int32)new FbCommand("select ID_ROLA from PRACOWNICY where LOGIN = '" + loginTB.Text + "' AND HASLO = '" + passwordPB.Password + "'", connection).ExecuteScalar();
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
            return Sb.ToString();
        }
    }
}
