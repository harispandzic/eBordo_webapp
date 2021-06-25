using Data.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBordo.ViewModels.Igrac.Index
{
    public class ZadnjaUtakmicaPrikaz
    {
        public int urakmicaID { get; set; }
        public string datumOdigravanja { get; set; }
        public string satnica { get; set; }
        public string nazivStadiona { get; set; }
        public string vrstaTakmicenja { get; set; }
        public string domacin { get; set; }
        public string gost { get; set; }
        public int goloviDomacin { get; set; }
        public int goloviGost { get; set; }
        public Rezultat rezultat { get; set; }
    }
}
