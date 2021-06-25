using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBordo.ViewModels.Povreda
{
    public class PovredaDodajVM
    {
        public int povredaID { get; set; }
        public DateTime datumPovrede { get; set; }
        public DateTime odsustvo { get; set; }
        public string tipPovrede { get; set; }
        public string kratkiOpis { get; set; }
        public string misljenjeLjekara { get; set; }
        public string odabraniIgrac { get; set; }
        public List<SelectListItem> igraci { get; set; }
        public List<SelectListItem> tipovi { get; set; }
    }
}
