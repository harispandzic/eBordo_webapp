using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBordo.ViewModels.Trening
{
    public class DodajTreningVM
    {
        public int treningID { get; set; }
        public DateTime datumOdrzavanja { get; set; }
        public string satnica { get; set; }
        public List<SelectListItem> lokacije { get; set; }
        public string lokacijaID { get; set; }
        public string fokusTreninga { get; set; }
    }
}
