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
    /// Logika interakcji dla klasy RentalListWindow.xaml
    /// </summary>
    public partial class RentalListWindow : Window
    {
        FbConnection connection;
        int idBook;

        public RentalListWindow(int idBook)
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
            this.idBook = idBook;


        }

        /*private void refreshRentalList()
        {
            
        }*/

        private void RentalsDG_Loaded(object sender, RoutedEventArgs e)
        {
            RentalsDG.Items.Clear();
            using (var transaction = connection.BeginTransaction())
            {
                using (var command = new FbCommand("select DATA_REZERWACJI, u.IMIE || ' ' || u.NAZWISKO " +
                "from REZERWACJE r " +
                "inner join UZYTKOWNICY u on r.ID_UZYTKOWNIK = u.ID_UZYTKOWNIK " +
                "inner join KSIAZKA ks on r.ID_KSIAZKA = ks.ID_KSIAZKA " +
                "where ks.ID_KSIAZKA = " + idBook +
                " order by NAZWISKO", connection, transaction))
                {
                    int result = command.ExecuteNonQuery();
                    if (result > 0)
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                IDataRecord record = reader;
                                Reservations tmp = new Reservations((DateTime)record[0], (string)record[1]);
                                RentalsDG.Items.Add(tmp);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Brak rezerwacji", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                        this.Close();
                    }
                }
            }
        }
    }
}
