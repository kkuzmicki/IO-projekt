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

namespace IO_projekt
{
    /// <summary>
    /// Logika interakcji dla klasy BookAdd.xaml
    /// </summary>
    public partial class AddBookWindow : Window
    {
        public AddBookWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        private void btnAuthor_Click(object sender, RoutedEventArgs e)
        {
            AuthorListWindow addWindow = new AuthorListWindow();
            addWindow.ShowDialog();
        }

        private void btnPublishing_Click(object sender, RoutedEventArgs e)
        {
            PublishingListWindow addWindow = new PublishingListWindow();
            addWindow.ShowDialog();
        }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
