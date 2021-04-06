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
    /// Logika interakcji dla klasy RentABook.xaml
    /// </summary>
    public partial class RentABook : Window
    {
        FbConnection connection;
        DataGrid CurrentDG;

        public RentABook()
        {
            InitializeComponent();
            FbConnectionStringBuilder csb = new FbConnectionStringBuilder();
            csb.DataSource = (string)Application.Current.Properties["dataSource"];
            csb.Port = 3050;
            csb.Database = (string)Application.Current.Properties["dataBase"];
            csb.UserID = (string)Application.Current.Properties["userID"];
            csb.Password = (string)Application.Current.Properties["password"];
            csb.ServerType = FbServerType.Default;

            connection = new FbConnection(csb.ToString());
            connection.Open();

            refreshBookList();
        }

        private void refreshBookList()
        {
            BooksDG.Items.Clear();
            using (var transaction = connection.BeginTransaction())
            {
                using (var command = new FbCommand("select ID_KSIAZKA, TYTUL, ks.ID_KATEGORIA, KATEGORIA, ks.ID_AUTOR, IMIE || ' ' || NAZWISKO, ks.ID_WYDAWNICTWO, WYDAWNICTWO, " +
                    "ROK_WYDANIA, ILOSC from KSIAZKA ks " +
                    "inner join KATEGORIE ka on ks.ID_KATEGORIA = ka.ID_KATEGORIA " +
                    "inner join AUTORZY a on ks.ID_AUTOR = a.ID_AUTOR " +
                    "inner join WYDAWNICTWA w on ks.ID_WYDAWNICTWO = w.ID_WYDAWNICTWO order by TYTUL", connection, transaction))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            IDataRecord record = reader;
                            Book tmp = new Book((int)record[0], (string)record[1], (int)record[2], (string)record[3], (int)record[4], (string)record[5],
                                (int)record[6], (string)record[7], (int)record[8], (int)record[9]);
                            BooksDG.Items.Add(tmp);
                        }
                    }
                }
            }
        }

        private void btnChoose_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
