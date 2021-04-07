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
        int userID;

        public RentABook(int id)
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
            userID = id;
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
            if(BooksDG.SelectedItem != null)
            {
                Book tmp = (Book)BooksDG.SelectedItem;
                string date1 = DateTime.Now.Day + "." + DateTime.Now.Month + "." + DateTime.Now.Year;
                DateTime newDate = DateTime.Now.AddDays(30);
                string date2 = newDate.Day + "." + newDate.Month + "." + newDate.Year;
                FbCommand command = new FbCommand("insert into WYPOZYCZENIA (ID_UZYTKOWNIK, ID_KSIAZKA, ID_PRACOWNIK, DATA_WYPOZYCZENIA, DATA_ODDANIA, CZY_ODDANE) " +
                    "values (" + userID + ", " + tmp.ID_KSIAZKA + ", " + (int)Application.Current.Properties["workerID"] + ", '" + date1 + "', '" + date2 + "', 0)", connection);
                int result = command.ExecuteNonQuery();
                if(result > 0)
                {
                    MessageBox.Show("Wypożyczono książkę!", "Potwierdzenie", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Błąd!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Zaznacz pozycję!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
