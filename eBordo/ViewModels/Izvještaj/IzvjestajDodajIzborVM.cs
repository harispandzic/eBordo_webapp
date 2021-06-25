using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBordo.ViewModels.Izvještaj
{
    public class IzvjestajDodajIzborVM
    {
        public List<SelectListItem> utakmice { get; set; }
        public int utakmicaID { get; set; }
    }
}
