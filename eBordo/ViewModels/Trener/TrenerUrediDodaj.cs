using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBordo.ViewModels.Trener
{
    public class TrenerUrediDodaj
    {
        public int trenerID { get; set; }
        public string ime { get; set; }
        public string prezime { get; set; }
        public DateTime datumRodjenja { get; set; }
        public string adresaPrebivalista { get; set; }
        public string telefon { get; set; }
        public string emailAdresa { get; set; }
        public string transferMarktAcc { get; set; }

        public int drzavljanstvoID { get; set; }
        public List<SelectListItem> drzave { get; set; }

        public int gradRodjenjaID { get; set; }
        public List<SelectListItem> gradovi { get; set; }

        public string nazivLicence { get; set; }
        public List<SelectListItem> licence { get; set; }

        public string preferiranaFormacija { get; set; }
        public List<SelectListItem> formacije { get; set; }

        public DateTime prvaZvanicnaUtakmica { get; set; }
        public DateTime datumPristupaKlubu { get; set; }
        public string dosadasnjiKlubovi { get; set; }
        public float trzisnaVrijednost { get; set; }

        public IFormFile slikaNew { set; get; }
        public string slikaCurrent { get; set; }
    }
}
