using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Logika interakcji dla klasy PublishingListWindow.xaml
    /// </summary>
    public partial class PublishingListWindow : Window
    {
        AddBookWindow MainWindow;
        public FbCommand command;
        public FbConnection connection;
        string sql = "select ID_WYDAWNICTWO, WYDAWNICTWO from WYDAWNICTWA order by WYDAWNICTWO";

        public PublishingListWindow(AddBookWindow MainWindow) // USUN TEN KOMENTARZ
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
                adapter.Fill(dtSet, "WYDAWNICTWA");
                Publishing.DataContext = dtSet;
            }
        }

        private void Publishing_Loaded(object sender, RoutedEventArgs e)
        {
            BindData();
        }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            if (Publishing.SelectedItem != null)
            {
                DataRowView row = Publishing.SelectedItem as DataRowView;
                //System.Windows.MessageBox.Show(row["Imie"].ToString() + " " + row["Nazwisko"].ToString());
                string publishing;
                publishing = row["Wydawnictwo"].ToString();

                //((AddBookWindow)Application.Current.MainWindow).publishingTB.Text = publishing;
                MainWindow.publishingTB.Text = publishing;
                
                this.Close();
            }
            else
            {
                System.Windows.MessageBox.Show("Nic nie wybrano!!!");
            }
        }
    }
}
