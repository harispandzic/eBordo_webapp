using System;
using System.Collections.Generic;
using System.Text;

namespace Data.EntityModels
{
    public class Klub
    {
        public int klubID { get; set; }
        public string nazivKluba { get; set; }
        public string grbKluba { get; set; }

        public int gradID { get; set; }
        public Grad grad { get; set; }

        public int stadionID { get; set; }
        public Stadion stadion { get; set; }

        public GarnituraDresova domaca{ get; set; }
        public int domacaID{ get; set; }

        public GarnituraDresova gostujuca { get; set; }
        public int gostujucaID { get; set; }
    }
}
