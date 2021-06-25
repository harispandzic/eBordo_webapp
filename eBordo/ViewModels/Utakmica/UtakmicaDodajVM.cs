using Data.EntityModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBordo.ViewModels.Utakmica
{
    public class UtakmicaDodajVM
    {
        public int utakmicaID { get; set; }
        public DateTime datumOdigravanja { get; set; }
        public string satnica { get; set; }
        public string sudija { get; set; }
        public string napomene { get; set; }
        public int brojUlaznica { get; set; }
        public float cijenaulaznice { get; set; }

        public List<SelectListItem> takmicenja{ get; set; }
        public int takmicenjeID { get; set; }

        public List<SelectListItem> igraci { get; set; }
        public int kapitenID { get; set; }

        public List<SelectListItem> treneri { get; set; }
        public int trenerID { get; set; }

        public List<SelectListItem> klubovi { get; set; }
        public int protivnikID { get; set; }

        public List<SelectListItem> vrijeme { get; set; }
        public string nazivVremena { get; set; }

        public List<SelectListItem> stadioni { get; set; }
        public int stadionID { get; set; }

        public int garnituraDresovaID { get; set; }

        public VrstaUtakmice vrstaUtakmice { get; set; }
        //public string garnituraDomacaSlika { get; set; }
        //public string garnituraGostujucaSlika { get; set; }

        //public int domaca { get; set; }
        //public int gostujuca { get; set; }

        public int golmanID { get; set; }
        public int lijeviBekID { get; set; }
        public int prviStoperID { get; set; }
        public int drugiStoperID { get; set; }
        public int desniBekID { get; set; }
        public int prviZadnjiVezniID { get; set; }
        public int drugiZadnjiVezniID { get; set; }
        public int prednjiVezniID { get; set; }
        public int lijevoKriloID { get; set; }
        public int desnoKriloID { get; set; }
        public int napadacID { get; set; }

        public int golmanKlupaID { get; set; }
        public int odbranaKlupaID { get; set; }
        public int bekKlupaID { get; set; }
        public int veznjakKlupaID { get; set; }
        public int kriloKlupaID { get; set; }
        public int napadacKlupaID { get; set; }
    }
}
