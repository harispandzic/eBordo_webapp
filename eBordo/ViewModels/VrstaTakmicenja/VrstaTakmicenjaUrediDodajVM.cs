using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBordo_seminarski_rad.ViewModels.VrstaTakmicenja
{
    public class VrstaTakmicenjaUrediDodajVM
    {
        public int vrstaTakmicenjaID { get; set; }
        public string nazivVrsteTakmicenja { get; set; }

        public IFormFile logoNew { set; get; }
        public string logoCurrent { get; set; }
    }
}
