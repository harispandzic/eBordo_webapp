using Data.EntityModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBordo.ViewModels.Izvještaj
{
    public class IzvjestajPrikazVM
    {
        public class Row
        {
            public Row() { }
            public int izvjestajID { get; set; }
            public int utakmicaID { get; set; }
            public string domacin { get; set; }
            public string domacinGrb { get; set; }
            public string gost { get; set; }
            public string gostGrb { get; set; }
            public int goloviDomaci { get; set; }
            public int goloviGosti { get; set; }
            public string igracUtakmice { get; set; }
            public string igracUtakmiceSlika { get; set; }
            public string stadion { get; set; }
            public Rezultat rezultat { get; set; }
            public StatusUtakmice statusUtakmice { get; set; }
            public string gradDomacina { get; set; }
            public string datumOdigravanja { get; set; }
            public DateTime datum { get; set; }
            public string vrstaTakmicenjaSlika { get; set; }
            public int nastupiloIgraca { get; set; }
            public int ocjenjenoIgraca { get; set; }
            public int odigranaDani { get; set; }
        }
        public List<Row> utakmice { get; set; }

        //Filteri
        public List<SelectListItem> klubovi { get; set; }
        public string nazivKluba { get; set; }

        public List<SelectListItem> rezultati { get; set; }
        public string rezultat { get; set; }

        public List<SelectListItem> takmicenja { get; set; }
        public string nazivTakmicenja { get; set; }

        public List<SelectListItem> vrste { get; set; }
        public string nazivVrste { get; set; }

        public string q; //po stadionu
    }
}
