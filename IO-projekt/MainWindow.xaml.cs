using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
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
        FbConnection db;
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

            db = new FbConnection(csb.ToString());
            db.Open();
        }

        private void loginB_click(object sender, RoutedEventArgs e)
        {
            var transaction = db.BeginTransaction();
            var command = new FbCommand("select * from TEST", db, transaction);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var values = new object[reader.FieldCount];
                reader.GetValues(values);
                Trace.WriteLine(string.Join("|", values));
            }
        }

        private void loginB_click(object sender, RoutedEventArgs e)
        {
            SecondWindow objSecondWindow = new SecondWindow();
            this.Visibility = Visibility.Hidden;
            objSecondWindow.Show();
        }
        var transaction = db.BeginTransaction();
        var command = new FbCommand("select * from TEST", db, transaction);
        var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var values = new object[reader.FieldCount];
        reader.GetValues(values);
                Trace.WriteLine(string.Join("|", values));
            }
}
}
