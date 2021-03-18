using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO_projekt
{
    public class Book
    {
        public int ID_KSIAZKA { get; set; }
        public string TYTUL { get; set; }
        public int ID_KATEGORIA { get; set; }
        public string KATEGORIA { get; set; }
        public int ID_AUTOR { get; set; }
        public string AUTOR { get; set; }
        public int ID_WYDAWNICTWO { get; set; }
        public string WYDAWNICTWO { get; set; }
        public int ROK_WYDANIA { get; set; }
        public int ILOSC { get; set; }

        public Book(int ID_KSIAZKA, string TYTUL, int ID_KATEGORIA, string KATEGORIA, int ID_AUTOR, string AUTOR, int ID_WYDAWNICTWO, 
            string WYDAWNICTWO, int ROK_WYDANIA, int ILOSC)
        {
            this.ID_KSIAZKA = ID_KSIAZKA;
            this.TYTUL = TYTUL;
            this.ID_KATEGORIA = ID_KATEGORIA;
            this.KATEGORIA = KATEGORIA;
            this.ID_AUTOR = ID_AUTOR;
            this.AUTOR = AUTOR;
            this.ID_WYDAWNICTWO = ID_WYDAWNICTWO;
            this.WYDAWNICTWO = WYDAWNICTWO;
            this.ROK_WYDANIA = ROK_WYDANIA;
            this.ILOSC = ILOSC;
        }
    }
}
