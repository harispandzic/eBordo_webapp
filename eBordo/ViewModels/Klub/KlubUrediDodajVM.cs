using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBordo_seminarski_rad.ViewModels.Klub
{
    public class KlubUrediDodajVM
    {
        public class GarnitureDresova
        {
            public GarnitureDresova() { }

            public int garnituraID { get; set; }
            public IFormFile garnituraSlika { set; get; }
            public string slikaTrenutna { get; set; }
        }
        public int klubID { get; set; }
        public string nazivKluba { get; set; }
        public GarnitureDresova domaci { get; set; }
        public GarnitureDresova gostujuci { get; set; }

        public string imeStadiona { get; set; }
        public IFormFile slikaStadionaNew { set; get; }
        public string slikaStadionaCurrent { get; set; }

        public int klubGradID { get; set; }
        public List<SelectListItem> gradovi { get; set; }

        public IFormFile grbNew { set; get; }
        public string grbCurrent { get; set; }
    }
}
