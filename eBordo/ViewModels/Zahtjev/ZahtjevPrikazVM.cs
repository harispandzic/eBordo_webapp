using Data.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBordo.ViewModels.Zahtjev
{
    public class ZahtjevPrikazVM
    {
        public class Row
        {
            public Row() { }
            public int zahtjevID { get; set; }
            public string datum { get; set; }
            public string svrha { get; set; }
            public string tip { get; set; }
            public string status { get; set; }
            public string prioritet { get; set; }
            public string korisnik { get; set; }
            public int prijeDana{ get; set; }
        }
        public List<Row> zahtjevi { get; set; }
    }
}
