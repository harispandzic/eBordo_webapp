using Data.Data;
using Data.EntityModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBordo.ViewModels.Poruka
{
    public class PorukaPosaljiVM
    {
        public List<SelectListItem> korisnici { get; set; }
        public string korisnikID { get; set; }
        public List<SelectListItem> tipoviPoruka { get; set; }
        public TipPoruke porukaID { get; set; }
        public List<SelectListItem> vrstaPoruke { get; set; }
        public TipNotifikacije vrstaID { get; set; }
        public string tekstPoruke { get; set; }
    }
}
