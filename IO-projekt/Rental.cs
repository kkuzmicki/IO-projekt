using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO_projekt
{
    public class Rental
    {
        public int ID_WYPOZYCZENIE { get; set; }
        public string TYTUL { get; set; }
        public string AUTOR { get; set; }
        public DateTime DATA_WYPOZYCZENIA { get; set; }
        public string DATA_WYPOZYCZENIAtext { get; set; }
        public DateTime DATA_ODDANIA { get; set; }
        public string DATA_ODDANIAtext { get; set; }
        public string PRACOWNIK { get; set; }

        public Rental(int iD_WYPOZYCZENIE, string tYTUL, string aUTOR, DateTime dATA_WYPOZYCZENIA, 
             DateTime dATA_ODDANIA, string pRACOWNIK)
        {
            ID_WYPOZYCZENIE = iD_WYPOZYCZENIE;
            TYTUL = tYTUL;
            AUTOR = aUTOR;
            DATA_WYPOZYCZENIA = dATA_WYPOZYCZENIA;
            DATA_WYPOZYCZENIAtext = DATA_WYPOZYCZENIA.Day.ToString("D2") + "." + DATA_WYPOZYCZENIA.Month.ToString("D2") + '.' + DATA_WYPOZYCZENIA.Year.ToString("D2");
            DATA_ODDANIA = dATA_ODDANIA;
            DATA_ODDANIAtext = dATA_ODDANIA.Day.ToString("D2") + "." + DATA_ODDANIA.Month.ToString("D2") + '.' + DATA_ODDANIA.Year.ToString("D2");
            PRACOWNIK = pRACOWNIK;
        }
    }
}
