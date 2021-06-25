using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBordo_seminarski_rad.ViewModels.Klub
{
    public class KlubPrikaziVM
    {
        public class Row
        {
            public Row() { }
            public int klubID { get; set; }
            public string nazivKluba { get; set; }
            public string grbCurrent { get; set; }
        }
        public List<Row> klubovi { get; set; }
        public string q;
    }
}
