using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBordo.ViewModels.Zahtjev
{
    public class DodajZahtjevVM
    {
        public int zahtjevID { get; set; }
        public DateTime datum { get; set; }

        public List<SelectListItem> svrhe { get; set; }
        public string svrhaID { get; set; }

        public List<SelectListItem> tipoviZahtjeva { get; set; }
        public string tipID { get; set; }

        public List<SelectListItem> prioriteti { get; set; }
        public string prioritetID { get; set; }

        public List<SelectListItem> statusiZahtjeva { get; set; }
        public string statusID { get; set; }

        public string odgovor { get; set; }
    }
}
