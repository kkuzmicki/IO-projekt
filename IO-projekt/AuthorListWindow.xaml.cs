using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    /// Logika interakcji dla klasy AuthorListWindow.xaml
    /// </summary>
    public partial class AuthorListWindow : Window
    {

        public FbCommand command;
        public FbConnection connection;
        string sql = "select ID_AUTOR, IMIE, NAZWISKO from AUTORZY";



        public AuthorListWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            BindData();
        }

        private void BindData()
        {

            FbConnectionStringBuilder csb = new FbConnectionStringBuilder();
            csb.DataSource = "localhost";
            csb.Port = 3050;
            csb.Database = @"C:\bazy\IO.FDB";
            csb.UserID = "SYSDBA";
            csb.Password = "masterkey";
            csb.ServerType = FbServerType.Default;

            DataSet dtSet = new DataSet();

            using (connection = new FbConnection(csb.ToString()))
            {
                command = new FbCommand(sql, connection);
                FbDataAdapter adapter = new FbDataAdapter();
                connection.Open();
                adapter.SelectCommand = command;
                adapter.Fill(dtSet, "AUTORZY");

                Author.DataContext = dtSet;
            }
        }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Author_Loaded(object sender, RoutedEventArgs e)
        {
            BindData();
        }
    }
}