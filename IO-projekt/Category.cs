using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO_projekt
{
    public class Category
    {
        public int ID_KATEGORIA { get; set; }
        public string KATEGORIA { get; set; }

        public Category(int iD_KATEGORIA, string kATEGORIA)
        {
            ID_KATEGORIA = iD_KATEGORIA;
            KATEGORIA = kATEGORIA;
        }
    }
}
