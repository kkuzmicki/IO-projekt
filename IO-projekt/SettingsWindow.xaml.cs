using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Logika interakcji dla klasy SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        readonly string path = "CONF.txt";

        public SettingsWindow()
        {
            InitializeComponent();
            try
            {
                if (File.Exists("CONF.txt"))
                {
                    StreamReader sr = new StreamReader(path);
                    dataSourceTB.Text = sr.ReadLine();
                    dataBaseTB.Text = sr.ReadLine();
                    userIDTB.Text = sr.ReadLine();
                    passwordPB.Password = sr.ReadLine();
                    sr.Close();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Błąd pliku: \n" + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            string dataSourceString = dataSourceTB.Text;
            string dataBaseString = dataBaseTB.Text;
            string userIDString = userIDTB.Text;
            string passwordString = passwordPB.Password;

            if(dataSourceString == "" || dataBaseString == "" || userIDString == "" || passwordString == "")
            {
                MessageBox.Show("Wypełnij wszystkie pola!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                File.Create(path).Dispose();

                StreamWriter sw = new StreamWriter(path);

                sw.WriteLine(dataSourceString);
                sw.WriteLine(dataBaseString);
                sw.WriteLine(userIDString);
                sw.WriteLine(passwordString);
                sw.Close();

                //FbConnectionStringBuilder csb = new FbConnectionStringBuilder();
                //csb.DataSource = dataSourceString;
                //csb.Port = 3050;
                //csb.Database = dataBaseString;
                //csb.UserID = userIDString;
                //csb.Password = passwordString;
                //csb.ServerType = FbServerType.Default;
                //connection = new FbConnection(csb.ToString());
                Application.Current.Properties["dataSource"] = dataSourceString;
                Application.Current.Properties["dataBase"] = dataBaseString;
                Application.Current.Properties["userID"] = userIDString;
                Application.Current.Properties["password"] = passwordString;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd: " + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
