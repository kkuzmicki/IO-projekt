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
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FbConnection connection;
        DataGrid CurrentDG;
        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            FbConnectionStringBuilder csb = new FbConnectionStringBuilder();
            csb.DataSource = "localhost";
            csb.Port = 3050;
            csb.Database = @"C:\bazy\IO.FDB";
            csb.UserID = "SYSDBA";
            csb.Password = "masterkey";
            csb.ServerType = FbServerType.Default;

            connection = new FbConnection(csb.ToString());
            connection.Open();

            refreshBookList();
            refreshRoleList();
            refreshPublishersList();

            CurrentDG = Books;
        }


        private void refreshBookList()
        {
            Books.Items.Clear();
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
                            Console.WriteLine("heh");
                            Book tmp = new Book((int)record[0], (string)record[1], (int)record[2], (string)record[3], (int)record[4], (string)record[5],
                                (int)record[6], (string)record[7], (int)record[8], (int)record[9]);
                            Books.Items.Add(tmp);
                        }
                    }
                }
            }
        }
        private void refreshAuthorList()
        {

        }
        private void refreshPublishersList()
        {
            RolesDG.Items.Clear();
            using (var transaction = connection.BeginTransaction())
            {
                using (var command = new FbCommand("select ID_ROLA, ROLA from ROLE order by ROLA", connection, transaction))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            IDataRecord record = reader;
                            Role tmp = new Role((int)record[0], (string)record[1]);
                            RolesDG.Items.Add(tmp);
                        }
                    }
                }
            }
        }
        private void refreshRoleList()
        {
            RolesDG.Items.Clear();
            using (var transaction = connection.BeginTransaction())
            {
                using (var command = new FbCommand("select ID_ROLA, ROLA from ROLE order by ROLA", connection, transaction))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            IDataRecord record = reader;
                            Role tmp = new Role((int)record[0], (string)record[1]);
                            RolesDG.Items.Add(tmp);
                        }
                    }
                }
            }
        }


        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddBookWindow addWindow = new AddBookWindow();
            addWindow.ShowDialog();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            /*
             * pobierać ze wskaźnika selected i getType(), następnie wysłać query do okna i coś z refreshem
             */
            FbCommand tmpQuery = null;
            string confText = "";
            
            string errorText = "";
            string tmpName = "";
            if (CurrentDG.SelectedItem == null) return;
            Console.WriteLine(CurrentDG.SelectedItem.GetType().ToString());
            switch (CurrentDG.SelectedItem.GetType().ToString())
            {
                case "IO_projekt.Book":
                    Book tmpBook = (Book)CurrentDG.SelectedItem;
                    tmpQuery = new FbCommand("delete from KSIAZKA where ID_KSIAZKA = " + tmpBook.ID_KSIAZKA, connection);
                    confText = "książkę";
                    errorText = "książki";
                    tmpName = tmpBook.TYTUL;
                    break;

                case "IO_projekt.Author":
                    Author tmpAuthor = (Author)CurrentDG.SelectedItem;
                    tmpQuery = new FbCommand("delete from AUTORZY where ID_AUTOR = " + tmpAuthor.ID_AUTOR, connection);
                    confText = "autora";
                    errorText = "autora";
                    tmpName = tmpAuthor.IMIE + ' ' + tmpAuthor.NAZWISKO;
                    break;

                case "IO_projekt.Role":
                    Role tmpRole = (Role)CurrentDG.SelectedItem;
                    Int32 checkResult = (Int32)new FbCommand("select count(*) from PRACOWNICY where ID_ROLA = " + tmpRole.ID_ROLA, connection).ExecuteScalar();
                    if(checkResult > 0)
                    {
                        MessageBox.Show("Nie można usunąć", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    tmpQuery = new FbCommand("delete from ROLE where ID_ROLA = " + tmpRole.ID_ROLA, connection);
                    confText = "rolę";
                    errorText = "roli";
                    tmpName = tmpRole.ROLA;
                    break;
            }
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz usunąć " + confText + ": \n" + tmpName + "?", 
                    "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
            if(result == MessageBoxResult.Yes)
            {
                int result2 = tmpQuery.ExecuteNonQuery();
                if (result2 == 0)
                {
                    MessageBox.Show("Nie udało się pomyślnie usunąć " + errorText + ".", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("Pozycja została usunięta.", "Potwierdzenie", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            refreshBookList();
        }
        private void btnBook_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RolesTI_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            
        }

        private void RolesTI_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                refreshRoleList();
                refreshBookList();
                string tabItem = ((sender as TabControl).SelectedItem as TabItem).Name as string;
                Console.WriteLine(tabItem);
                switch(tabItem)
                {
                    case "BookIT":
                        refreshBookList();
                        CurrentDG = Books;
                        break;
                    case "UsersTI":

                        break;
                    case "RolesTI":
                        refreshRoleList();
                        CurrentDG = RolesDG;
                        break;
                }
            }
        }
    }
}
