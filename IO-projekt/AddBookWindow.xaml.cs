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
        int genreID;

        public AddBookWindow()
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
            BindData();
        }

        public AddBookWindow(Book book)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            isEdit = true;
            FbConnectionStringBuilder csb = new FbConnectionStringBuilder();
            csb.DataSource = "localhost";
            csb.Port = 3050;
            csb.Database = @"C:\bazy\IO.FDB";
            csb.UserID = "SYSDBA";
            csb.Password = "masterkey";
            csb.ServerType = FbServerType.Default;
            connection = new FbConnection(csb.ToString());
            connection.Open();

            BindData();

            titleTB.Text = book.TYTUL;

            publishingTB.Text = book.WYDAWNICTWO;
            publishingUpDownControl.Value = book.ROK_WYDANIA;
            quantityUpDownControl.Value = book.ILOSC;
        }

        private void BindData()
        {
            DataTable dataTable = new DataTable();
            command = new FbCommand(sql, connection);
                
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
            Console.WriteLine(genreTB.SelectedIndex);
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
            //MessageBox.Show("ID: " + row["ID_KATEGORIA"] + "\nKATEGORIA: " + row["KATEGORIA"]);
            genreID = (int)row["ID_KATEGORIA"];
        }
    }
}