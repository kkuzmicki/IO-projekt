using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO_projekt
{
    public class User
    {
        public int ID_UZYTKOWNIK { get; set; }
        public string LOGIN { get; set; }
        public string HASLO { get; set; }
        public string IMIE { get; set; }
        public string NAZWISKO { get; set; }
        public string EMAIL { get; set; }
        public DateTime DATA_URODZENIA { get; set; }
        public string DATA_URODZENIAtext { get; set; }

        public User(int iD_UZYTKOWNIK, string lOGIN, string hASLO, string iMIE, string nAZWISKO, string eMAIL, DateTime dATA_URODZENIA)
        {
            ID_UZYTKOWNIK = iD_UZYTKOWNIK;
            LOGIN = lOGIN;
            HASLO = hASLO;
            IMIE = iMIE;
            NAZWISKO = nAZWISKO;
            EMAIL = eMAIL;
            DATA_URODZENIA = dATA_URODZENIA;
            DATA_URODZENIAtext = DATA_URODZENIA.Day.ToString("D2") + '.' + DATA_URODZENIA.Month.ToString("D2") + '.' + DATA_URODZENIA.Year.ToString();
        }
    }
}