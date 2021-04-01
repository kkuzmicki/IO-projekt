using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
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
        }

        private void loginB_click(object sender, RoutedEventArgs e)
        {
            var command2 = new FbCommand("select COUNT(*) from PRACOWNICY where LOGIN = '" + loginTB.Text + "' AND HASLO = '" + passwordPB.Password + "'", connection);
            Int32 count = (Int32)command2.ExecuteScalar();
            if(count > 0)
            {
                MainWindow objSecondWindow = new MainWindow("Bibliotekarz"); // wpisana na sztywno wartość
                this.Close();
                objSecondWindow.Show();
            } 
            else
            {
                errorTB.Text = "Błędne dane";
            }
            Console.WriteLine(count);
        }
    }
}
