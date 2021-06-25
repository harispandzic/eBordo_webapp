using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBordo_seminarski_rad.ViewModels.VrstaTakmicenja
{
    public class VrstaTakmicenjaPrikaziVM
    {
        public class Row
        {
            public Row() { }
            public int vrstaTakmicenjaID { get; set; }
            public string nazivVrsteTakmicenja { get; set; }
            public string logoCurrent { get; set; }
        }
        public List<Row> vrsteTakmicenja { get; set; }
        public string q;
    }
}
