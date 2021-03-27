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
            dateTB.Text = _dateValue.ToString();
            quantityTB.Text = _numValue.ToString();
        }


        private int _numValue = 0;
        private int _dateValue = 0;

        public int NumValue
        {
            get { return _numValue; }
            set
            {
                _numValue = value;
                quantityTB.Text = value.ToString();
            }
        }

        public int DateValue
        {
            get { return _numValue; }
            set
            {
                _dateValue = value;
                dateTB.Text = value.ToString();
            }
        }

        private void cmdUp_Click(object sender, RoutedEventArgs e)
        {
            NumValue++;
        }

        private void cmdDown_Click(object sender, RoutedEventArgs e)
        {
            NumValue--;
        }

        private void cmd2Up_Click(object sender, RoutedEventArgs e)
        {
            DateValue++;
        }

        private void cmd2Down_Click(object sender, RoutedEventArgs e)
        {
            DateValue--;
        }

        private void txtNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (quantityTB == null)
            {
                return;
            }

            if (!int.TryParse(quantityTB.Text, out _numValue))
                quantityTB.Text = _numValue.ToString();
        }

        private void dateNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (dateTB == null)
            {
                return;
            }

            if (!int.TryParse(dateTB.Text, out _dateValue))
                dateTB.Text = _dateValue.ToString();
        }

        private void btnAuthor_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnPublishing_Click(object sender, RoutedEventArgs e)
        {

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
