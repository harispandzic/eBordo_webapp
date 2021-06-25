using System;
using System.Collections.Generic;
using System.Text;

namespace Data.EntityModels
{
    public class IgracStatistika
    {
        public int igracStatistikaID { get; set; }
        public int brojNastupa { get; set; }
        public int odigraneMinute { get; set; }
        public int golovi { get; set; }
        public int asistencije { get; set; }
        public int zutiKartoni { get; set; }
        public int crveniKartoni { get; set; }

        public int zbirOcjena { get; set; }
        public float prosjecnaOcjena { get; set; } //na osnovu ocjena sa nastupa

        public Igrac igrac { get; set; }
        public int? igracID { get; set; }
    }
}
