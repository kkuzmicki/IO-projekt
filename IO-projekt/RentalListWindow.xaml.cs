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

        public RentalListWindow()
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

            refreshRentalList();
        }

        private void refreshRentalList()
        {
            RentalsDG.Items.Clear();
            using (var transaction = connection.BeginTransaction())
            {
                using (var command = new FbCommand("select r.ID_REZERWACJA, r.ID_UZYTKOWNIK, ks.ID_KSIAZKA, DATA_REZERWACJI, u.LOGIN, u.IMIE || ' ' || u.NAZWISKO, u.EMAIL, u.DATA_URODZENIA" +
                "from REZERWACJE r " +
                "inner join UZYTKOWNICY u on r.ID_UZYTKOWNIK = u.ID_UZYTKOWNIK " +
                "inner join KSIAZKA ks on r.ID_KSIAZKA = ks.ID_KSIAZKA " +
                "order by NAZWISKO", connection, transaction))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            IDataRecord record = reader;
                            User tmp = new User((int)record[0], (string)record[1], (string)record[2], (string)record[3], (string)record[4], (string)record[5], (DateTime)record[6]);
                            RentalsDG.Items.Add(tmp);
                        }
                    }
                }
            }
        }
    }
}
