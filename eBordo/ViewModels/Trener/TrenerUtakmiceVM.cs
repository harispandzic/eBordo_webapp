using Data.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBordo.ViewModels.Trener
{
    public class TrenerUtakmiceVM
    {
        public class Row
        {
            public Row() { }
            public string domacin { get; set; }
            public string domacinGrb { get; set; }
            public string gost { get; set; }
            public string gostGrb { get; set; }
            public string vrstaTakmicenjaSlika { get; set; }
            public DateTime datum { get; set; }
            public Rezultat rezultat { get; set; }
            public int goloviDomaci { get; set; }
            public int goloviGosti { get; set; }
            public int odigranaDani { get; set; }
        }
        public List<Row> utakmice { get; set; }
    }
}
