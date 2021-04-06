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
            DATA_WYPOZYCZENIAtext = DATA_WYPOZYCZENIA.Day + "." + DATA_WYPOZYCZENIA.Month + '.' + DATA_WYPOZYCZENIA.Year;
            DATA_ODDANIA = dATA_ODDANIA;
            DATA_ODDANIAtext = dATA_ODDANIA.Day + "." + DATA_ODDANIA.Month + '.' + DATA_ODDANIA.Year;
            PRACOWNIK = pRACOWNIK;
        }
    }
}
