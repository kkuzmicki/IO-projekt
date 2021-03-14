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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int hour;
        FbConnection connection;
        public MainWindow()
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
            using (var transaction = connection.BeginTransaction())
            {
                using (var command = new FbCommand("select LOGIN, PASSWORD from USERS", connection, transaction))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            IDataRecord record = reader;
                            if (loginTB.Text == (String)record[0] && passwordPB.Password == (String)record[1])
                            {
                                SecondWindow objSecondWindow = new SecondWindow();
                                this.Close();
                                objSecondWindow.Show();
                            }
                        }
                    }
                }
            }
        }
    }
}
