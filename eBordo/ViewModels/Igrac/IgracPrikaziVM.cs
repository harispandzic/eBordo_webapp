using Data.EntityModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBordo.ViewModels.Igrac
{
    public class IgracPrikaziVM
    {
        public class Row
        {
            public Row() { }
            public int igracID { get; set; }
            public string ime { get; set; }
            public string prezime { get; set; }
            public string slikaIgraca { get; set; }
            public string datumRodjenja { get; set; }
            public string drzavljanstvo { get; set; }
            public string drzavaSlika { get; set; }
            public string gradRodjenja { get; set; }
            public string gradRodjenjaZastava { get; set; }
            public string pozicija { get; set; }
            public int brojDresa { get; set; }
            public Status statusIgraca { get; set; }
            public float trzisnaVrijednost { get; set; }
        }
        public List<Row> igraci { get; set; }
        public List<SelectListItem> drzave { get; set; }
        public List<SelectListItem> pozicije { get; set; }
        public List<SelectListItem> statusi { get; set; }
        //Filteri
        public string nazivDrzave { get; set; }
        public string nazivPozicije { get; set; }
        public string statusIgraca { get; set; }
        public string q { get; set; }
    }
}
