﻿using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for AddCategoryWindow.xaml
    /// </summary>
    public partial class AddCategoryWindow : Window
    {
        bool isEdit;
        int id;
        FbConnection connection;

        public AddCategoryWindow()
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
            id = 0;
        }

        public AddCategoryWindow(Category category)
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
            id = category.ID_KATEGORIA;
            nameTB.Text = category.KATEGORIA;
            HeaderL.Text = "Edycja kategorii";
            AddCategoryW.Title = "Edycja kategorii";
        }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            if(nameTB.Text == "")
            {
                MessageBox.Show("Podaj nazwę kategorii!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            FbCommand command = new FbCommand("select count(*) from KATEGORIE where KATEGORIA = '" + nameTB.Text + "' and ID_KATEGORIA <> " + id, connection);
            Int32 result = (Int32)command.ExecuteScalar();
            if (result > 0)
            {
                MessageBox.Show("Podana kategoria już istnieje!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                if (isEdit)
                {
                    command = new FbCommand("update KATEGORIE set KATEGORIA = '" + nameTB.Text + "' where ID_KATEGORIA = " + id, connection);
                    result = command.ExecuteNonQuery();
                }
                else
                {
                    command = new FbCommand("insert into KATEGORIE (KATEGORIA) values ('" + nameTB.Text + "')", connection);
                    result = command.ExecuteNonQuery();
                }
            }
            if(result != 0)
            {
                MessageBox.Show("Operacja powiodła się!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            } 
            else
            {
                MessageBox.Show("Operacja nie powiodła się!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                
            }
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
