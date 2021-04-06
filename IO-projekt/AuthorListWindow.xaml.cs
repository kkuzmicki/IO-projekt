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
using Xceed.Wpf.Toolkit;

namespace IO_projekt
{
    /// <summary>
    /// Logika interakcji dla klasy AuthorListWindow.xaml
    /// </summary>
    public partial class AuthorListWindow : Window
    {
        AddBookWindow MainWindow;
        public FbCommand command;
        public FbConnection connection;
        string sql = "select ID_AUTOR, IMIE, NAZWISKO from AUTORZY order by NAZWISKO";

        public AuthorListWindow(AddBookWindow MainWindow)
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            BindData();
            this.MainWindow = MainWindow;
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
            if (Author.SelectedItem != null)
            {
                DataRowView row = Author.SelectedItem as DataRowView;
                //System.Windows.MessageBox.Show(row["Imie"].ToString() + " " + row["Nazwisko"].ToString());
                string authorName, authorSurname;
                authorName = row["Imie"].ToString();
                authorSurname = row["Nazwisko"].ToString();

                //((AddBookWindow)Application.Current.MainWindow).authorNameTB.Text = authorName;
                MainWindow.authorNameTB.Text = authorName;
                MainWindow.authorSurnameTB.Text = authorSurname;

                this.Close();
            }
            else
            {
                System.Windows.MessageBox.Show("Nic nie wybrano!!!");
            }
        }

        private void Author_Loaded(object sender, RoutedEventArgs e)
        {
            BindData();
        }
    }
}