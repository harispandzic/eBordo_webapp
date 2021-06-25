using Data.EntityModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBordo.ViewModels.Utakmica
{
    public class UtakmicaPrikazVM
    {
        public class Row
        {
            public Row() { }
            public int utakmicaID { get; set; }
            public string domacin { get; set; }
            public string domacinGrb { get; set; }
            public string gost { get; set; }
            public string gostGrb { get; set; }
            public string stadion { get; set; }
            public StatusUtakmice statusUtakmice { get; set; }
            public string gradDomacina { get; set; }
            public string datumOdigravanja { get; set; }
            public DateTime datum { get; set; }
            public string satnica { get; set; }
            public string vrstaTakmicenja { get; set; }
            public string vrstaTakmicenjaSlika { get; set; }
            public string kapiten { get; set; }
            public string kapitenSlika { get; set; }
            public int brojDana{ get; set; }
            public string vrijeme{ get; set; }
        }
        public List<Row> utakmice { get; set; }

        //Filteri
        public List<SelectListItem> klubovi { get; set; }
        public string nazivKluba { get; set; }

        public List<SelectListItem> takmicenja { get; set; }
        public string nazivTakmicenja { get; set; }

        public List<SelectListItem> vrste { get; set; }
        public string nazivVrste { get; set; }

        public string q; //po stadionu
    }
}
