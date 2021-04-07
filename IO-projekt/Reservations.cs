using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO_projekt
{
    class Reservations
    {
        public DateTime DATA_REZERWACJI { get; set; }
        public string DATA_REZERWACJItext { get; set; }
        public string UZYTKOWNIK { get; set; }

        public Reservations(DateTime dATA_REZERWACJI, string uZYTKOWNIK)
        {
            DATA_REZERWACJI = dATA_REZERWACJI;
            DATA_REZERWACJItext = dATA_REZERWACJI.Day.ToString("D2") + "." + dATA_REZERWACJI.Month.ToString("D2") + '.' + dATA_REZERWACJI.Year.ToString("D2");
            UZYTKOWNIK = uZYTKOWNIK;
        }
    }
}
