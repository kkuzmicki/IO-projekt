using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO_projekt
{
    public class Worker
    {
        public int ID_PRACOWNIK { get; set; }
        public string LOGIN { get; set; }
        public string HASLO { get; set; }
        public string IMIE { get; set; }
        public string NAZWISKO { get; set; }
        public int ID_ROLA { get; set; }
        public string ROLA { get; set; }

        public Worker(int iD_PRACOWNIK, string lOGIN, string hASLO, string iMIE, string nAZWISKO, int iD_ROLA, string rOLA)
        {
            ID_PRACOWNIK = iD_PRACOWNIK;
            LOGIN = lOGIN;
            HASLO = hASLO;
            IMIE = iMIE;
            NAZWISKO = nAZWISKO;
            ID_ROLA = iD_ROLA;
            ROLA = rOLA;
        }
    }
}
