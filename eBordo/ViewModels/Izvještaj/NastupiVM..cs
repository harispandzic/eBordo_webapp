using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBordo.ViewModels.Izvještaj
{
    public class NastupiVM
    {
        public class Nastupi
        {
            public Nastupi() { }
            public int igracID { get; set; }
            public string imePrezime { get; set; }
            public string slikaIgraca { get; set; }
            public int brojDresa { get; set; }

            //statisika
            public int minute { get; set; }
            public int golovi { get; set; }
            public int asistencije { get; set; }
            public int zutiKartoni { get; set; }
            public int crveniKartoni { get; set; }
            public float ocjena { get; set; }
            public float prosjecnaOcjenaStavki { get; set; }
        }
        public int utakmicaID { get; set; }
        public List<Nastupi> nastupi { get; set; }
    }
}
