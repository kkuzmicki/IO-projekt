using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
using FirebirdSql.Data.FirebirdClient;
using System.Data;

namespace IO_projekt
{
    /// <summary>
    /// Logika interakcji dla klasy BookAdd.xaml
    /// </summary>
    public partial class AddBookWindow : Window
    {
        bool isEdit;
        public FbCommand command;
        public FbConnection connection;
        string sql = "select ID_KATEGORIA, KATEGORIA from KATEGORIE";

        public AddBookWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            BindData();
            isEdit = false;
        }

        public AddBookWindow(Book book)
        {
            isEdit = true;
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

            DataTable dataTable = new DataTable();

            using (connection = new FbConnection(csb.ToString()))
            {
                command = new FbCommand(sql, connection);
                
                connection.Open();
                var reader = command.ExecuteReader();

                dataTable.Columns.Add("ID_KATEGORIA", typeof(int));
                dataTable.Columns.Add("KATEGORIA", typeof(string));

                while (reader.Read())
                {
                    IDataRecord record = reader;
                    dataTable.Rows.Add((int)record[0], (string)record[1]);
                }

                genreTB.ItemsSource = dataTable.DefaultView;

            }
        }

        private void btnAuthor_Click(object sender, RoutedEventArgs e)
        {
            AuthorListWindow addWindow = new AuthorListWindow(this);
            addWindow.ShowDialog();
        }

        private void btnPublishing_Click(object sender, RoutedEventArgs e)
        {
            PublishingListWindow addWindow = new PublishingListWindow(this);
            addWindow.ShowDialog();
        }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void genreTB_Loaded(object sender, RoutedEventArgs e)
        {
            BindData();
        }

        private void genreTB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;

            DataRowView row = (DataRowView)e.AddedItems[0];
            MessageBox.Show("ID: " + row["ID_KATEGORIA"] + "\nKATEGORIA: " + row["KATEGORIA"]);
        }
    }
}
