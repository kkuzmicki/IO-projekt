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
        string sql = "select ID_KATEGORIA, KATEGORIA from KATEGORIE order by KATEGORIA";
        int genreID;
        int bookID;

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
            genreID = 0;
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

            genreID = book.ID_KATEGORIA;

            BindData();

            titleTB.Text = book.TYTUL;

            using (var transaction = connection.BeginTransaction())
            {
                using (var command2 = new FbCommand("select IMIE, NAZWISKO from AUTORZY where ID_AUTOR = " + book.ID_AUTOR, connection, transaction))
                {
                    using (var reader = command2.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            IDataRecord record = reader;
                            authorNameTB.Text = (string)record[0];
                            authorSurnameTB.Text = (string)record[1];
                        }
                    }
                }
            }
            publishingTB.Text = book.WYDAWNICTWO;
            publishingUpDownControl.Value = book.ROK_WYDANIA;
            quantityUpDownControl.Value = book.ILOSC;
            bookID = book.ID_KSIAZKA;
            AddBookW.Title = "Edycja książki";
            headerL.Text = "Edycja książki";
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

            if(genreID != 0)
            {
                DataRow[] row = dataTable.Select("ID_KATEGORIA = " + genreID);
                int index = dataTable.Rows.IndexOf(row[0]);
                genreTB.SelectedIndex = index;
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

            if(titleTB.Text == null || titleTB.Text == "")
            {
                MessageBox.Show("Uzupełnij tytuł!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if(genreID == 0)
            {
                MessageBox.Show("Wybierz kategorię!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (authorNameTB.Text == null || authorSurnameTB.Text == null || authorNameTB.Text == "" || authorSurnameTB.Text == "")
            {
                MessageBox.Show("Podaj autora!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (publishingTB.Text == null || publishingTB.Text == "")
            {
                MessageBox.Show("Uzupełnij wydawnictwo!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (publishingUpDownControl.Value == null)
            {
                MessageBox.Show("Podaj rok wydania!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (quantityUpDownControl.Value == null)
            {
                MessageBox.Show("Podaj ilość!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Console.WriteLine("Tytuł: " + titleTB.Text);
            Console.WriteLine("ID kategorii: " + genreID);
            Console.WriteLine("Autor: " + authorNameTB.Text + ' ' + authorSurnameTB.Text);
            Console.WriteLine("Wydawnictwo: " + publishingTB.Text);
            Console.WriteLine("Rok wydania: " + publishingUpDownControl.Value);
            Console.WriteLine("Ilość: " + quantityUpDownControl.Value);

            int idAuthor = 0;

            using (var transaction = connection.BeginTransaction())
            {
                using (var command2 = new FbCommand("select ID_AUTOR from AUTORZY where IMIE = '" + authorNameTB.Text + "' AND NAZWISKO = '" + authorSurnameTB.Text + "'", connection, transaction))
                {
                    using (var reader = command2.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            IDataRecord record = reader;
                            idAuthor = (int)record[0];
                        }
                    }
                }
            }

            if(idAuthor == 0)
            {
                command = new FbCommand("insert into AUTORZY (IMIE, NAZWISKO) values ('" + authorNameTB.Text + "', '" + authorSurnameTB.Text + "')", connection);
                command.ExecuteNonQuery();
                using (var transaction = connection.BeginTransaction())
                {
                    using (var command2 = new FbCommand("select ID_AUTOR from AUTORZY where IMIE = '" + authorNameTB.Text + "' AND NAZWISKO = '" + authorSurnameTB.Text + "'", connection, transaction))
                    {
                        using (var reader = command2.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                IDataRecord record = reader;
                                idAuthor = (int)record[0];
                            }
                        }
                    }
                }
            }

            Console.WriteLine("ID autora: " + idAuthor);

            int idPublishing = 0;

            using (var transaction = connection.BeginTransaction())
            {
                using (var command2 = new FbCommand("select ID_WYDAWNICTWO from WYDAWNICTWA where WYDAWNICTWO = '" + publishingTB.Text + "'", connection, transaction))
                {
                    using (var reader = command2.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            IDataRecord record = reader;
                            idPublishing = (int)record[0];
                        }
                    }
                }
            }

            if (idPublishing == 0)
            {
                command = new FbCommand("insert into WYDAWNICTWA (WYDAWNICTWO) values ('" + publishingTB.Text + "');", connection);
                command.ExecuteNonQuery();
                using (var transaction = connection.BeginTransaction())
                {
                    using (var command2 = new FbCommand("select ID_WYDAWNICTWO from WYDAWNICTWA where WYDAWNICTWO = '" + publishingTB.Text + "'", connection, transaction))
                    {
                        using (var reader = command2.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                IDataRecord record = reader;
                                idPublishing = (int)record[0];
                            }
                        }
                    }
                }
            }

            if(isEdit)
            {
                command = new FbCommand("update KSIAZKA set TYTUL = '" + titleTB.Text + "', ID_KATEGORIA = " + genreID + ", ID_AUTOR = " + idAuthor + ", ID_WYDAWNICTWO = " +
                    idPublishing + ", ROK_WYDANIA = " + publishingUpDownControl.Value + ", ILOSC = " + quantityUpDownControl.Value + " where ID_KSIAZKA = " + bookID, connection);
                int result = command.ExecuteNonQuery();
                if(result == 0)
                {
                    MessageBox.Show("Nie udało się zmodyfikować pozycji!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("Operacja zakończona powodzeniem!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                this.Close();
            }
            else
            {
                command = new FbCommand("insert into KSIAZKA (TYTUL, ID_KATEGORIA, ID_AUTOR, ID_WYDAWNICTWO, ROK_WYDANIA, ILOSC) values ('" + titleTB.Text + 
                    "', " + genreID + ", " + idAuthor + ", " + idPublishing + ", " + publishingUpDownControl.Value + ", " + quantityUpDownControl.Value + ");", connection);
                int result = command.ExecuteNonQuery();
                if (result == 0)
                {
                    MessageBox.Show("Nie udało się dodać pozycji!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("Operacja zakończona powodzeniem!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                this.Close();
            }
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