using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBordo.ViewModels.Izvještaj
{
    public class IzvjestajKreirajVM
    {
        public int izvjestajID { get; set; }
        public int utakmicaID { get; set; }
        public string datumOdigravanja { get; set; }
        public string satnica { get; set; }
        public string nazivStadiona { get; set; }
        public string slikaVrstaTakmicenja { get; set; }
        public string domacin { get; set; }
        public string domacinGrb { get; set; }
        public string gost { get; set; }
        public string gostGrb { get; set; }

        public int goloviGostiju { get; set; }
        public int goloviDomacih { get; set; }
        public List<SelectListItem> igraci { get; set; }
        public int igracUtakmiceID { get; set; }
        public string delegatUtakmice { get; set; }
        public string zapisnik { get; set; }
        public string trener { get; set; }
        public string trenerSlika { get; set; }
        public string youTubeVideoID { get; set; }

        public IFormFile slikaNew { set; get; }
        public string slikaCurrent { get; set; }

        public int brojGledalaca { get; set; }
        public List<SelectListItem> vrijeme { get; set; }
        public string nazivVremena { get; set; }
        public DateTime datumKreiranja { get; set; }
        //public string zapisnik { get; set; }
    }
}
