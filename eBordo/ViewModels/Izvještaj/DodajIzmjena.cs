using Data.EntityModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eBordo.ViewModels.Izvještaj
{
    public class DodajIzmjena
    {
        public int utakmicaID { get; set; }
        public int izmjenaID { get; set; }

        public int igracIN { get; set; }
        public int igracOUT { get; set; }
        [Required]
        public int minuta { get; set; }
        public RazlogIzmjene razlog { get; set; }

        public List<SelectListItem> igraciKlupa { get; set; }
        public List<SelectListItem> razlozi { get; set; }
        public List<SelectListItem> igraciPrvaPostava { get; set; }
    }
}
