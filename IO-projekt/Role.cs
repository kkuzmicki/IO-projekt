using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO_projekt
{
    public class Role
    {
        public int ID_ROLA { get; set; }
        public string ROLA { get; set; }


        public Role(int ID_ROLA, string ROLA)
        {
            this.ID_ROLA = ID_ROLA;
            this.ROLA = ROLA;
        }
    }
}
