using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eBordo.ViewModels.Igrac
{
    public class IgracUrediDodajVM
    {
        public int igracID { get; set; }
        [Required]
        public string ime { get; set; }
        [Required]
        public string prezime { get; set; }
        [Required]
        public DateTime datumRodjenja { get; set; }
        [Required]
        public string adresaPrebivalista { get; set; }
        [Required]
        public string telefon { get; set; }
        [Required]
        [EmailAddress]
        public string emailAdresa { get; set; }
        [Required]
        public string transferMarktAcc { get; set; }
        [Required]
        public int drzavljanstvoID { get; set; }
        public List<SelectListItem> drzave { get; set; }
        [Required]
        public bool reprezentativac { get; set; }
        [Required]
        public int gradRodjenjaID { get; set; }
        public List<SelectListItem> gradovi { get; set; }
        [Required]
        public float visina { get; set; }
        [Required]
        public int tezina { get; set; }
        [Required]
        public int brojDresa { get; set; }
        [Required]
        public float trzisnaVrijednost { get; set; }
        [Required]
        public DateTime datumPristupaKlubu { get; set; }
        [Required]
        public string dosadasnjiKlubovi { get; set; }
        [Required]
        public string nazivPozicije { get; set; }
        public List<SelectListItem> pozicije { get; set; }
        [Required]
        public string boljaNoga { get; set; }
        public List<SelectListItem> noga { get; set; }
        [Required]
        public IFormFile slikaNew { set; get; }
        public string slikaCurrent { get; set; }
    }
}
