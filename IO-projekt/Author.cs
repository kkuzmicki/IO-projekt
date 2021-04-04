using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO_projekt
{
    public class Author
    {
        public int ID_AUTOR { get; set; }
        public string IMIE { get; set; }
        public string NAZWISKO { get; set; }
        public Author(int ID_AUTOR, string IMIE, string NAZWISKO)
        {
            this.ID_AUTOR = ID_AUTOR;
            this.IMIE = IMIE;
            this.NAZWISKO = NAZWISKO;
        }
    }
}