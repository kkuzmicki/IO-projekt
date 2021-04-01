using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO_projekt
{
    public class Publisher
    {
        public int ID_WYDAWNICTWO { get; set; }
        public string WYDAWNICTWO { get; set; }

        public Publisher(int iD_WYDAWNICTWO, string wYDAWNICTWO)
        {
            ID_WYDAWNICTWO = iD_WYDAWNICTWO;
            WYDAWNICTWO = wYDAWNICTWO;
        }
    }
}
