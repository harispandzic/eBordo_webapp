using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBordo_seminarski_rad.ViewModels.Drzava
{
    public class DrzavaPrikaziVM
    {
        public class Row
        {
            public Row() { }
            public int drzavaID { get; set; }
            public string nazivDrzave { get; set; }
            public string zastavaCurrent { get; set; }
        }
        public List<Row> drzave { get; set; }
        public string q;
    }
}
