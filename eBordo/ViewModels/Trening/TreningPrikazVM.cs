using Data.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBordo.ViewModels.Trening
{
    public class TreningPrikazVM
    {
        public class Row
        {
            public Row() { }
            public int treningID { get; set; }
            public DateTime datumOdrzavanja { get; set; }
            public string satnica { get; set; }
            public LokacijaTreninga lokacija { get; set; }
            public string fokusTreninga { get; set; }
            public string trener{ get; set; }
            public string trenerSlika{ get; set; }
            public int brojDana{ get; set; }
        }
        public List<Row> treninzi { get; set; }
    }
}
