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
    /// Interaction logic for RentWindow.xaml
    /// </summary>
    public partial class RentWindow : Window
    {
        FbConnection connection;
        int id;
        public RentWindow(int id)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            FbConnectionStringBuilder csb = new FbConnectionStringBuilder();
            csb.DataSource = (string)Application.Current.Properties["dataSource"];
            csb.Port = 3050;
            csb.Database = (string)Application.Current.Properties["dataBase"];
            csb.UserID = (string)Application.Current.Properties["userID"];
            csb.Password = (string)Application.Current.Properties["password"];
            csb.ServerType = FbServerType.Default;

            connection = new FbConnection(csb.ToString());
            connection.Open();

            this.id = id;
        }

        private void refreshRented()
        {
            RentalsDG.Items.Clear();
            using (var transaction = connection.BeginTransaction())
            {
                using (var command = new FbCommand("select ID_WYPOZYCZENIE, " +
                    "TYTUL, a.IMIE || ' ' || a.NAZWISKO, DATA_WYPOZYCZENIA, DATA_ODDANIA, p.IMIE || ' ' || p.NAZWISKO " +
                    "from WYPOZYCZENIA w inner join KSIAZKA k on w.ID_KSIAZKA = k.ID_KSIAZKA inner join AUTORZY a on k.ID_AUTOR = a.ID_AUTOR inner join " +
                    "PRACOWNICY p on w.ID_PRACOWNIK = p.ID_PRACOWNIK where w.ID_UZYTKOWNIK = " + id + " order by DATA_WYPOZYCZENIA", connection, transaction))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            IDataRecord record = reader;
                            Rental tmp = new Rental((int)record[0], (string)record[1], (string)record[2], (DateTime)record[3], (DateTime)record[4], (string)record[5]);
                            RentalsDG.Items.Add(tmp);
                        }
                    }
                }
            }
        }

        private void rentB_Click(object sender, RoutedEventArgs e)
        {
            RentABook addWindow = new RentABook();
            addWindow.ShowDialog();
        }

        private void returnB_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
