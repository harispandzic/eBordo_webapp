using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBordo.ViewModels.Igrac.Index
{
    public class StatistikaIndex
    {
        public int igracID { get; set; }
        public int brojNastupa { get; set; }
        public int odigraneMinute { get; set; }
        public int golovi { get; set; }
        public int asistencije { get; set; }
        public int zutiKartoni { get; set; }
        public int crveniKartoni { get; set; }
        public float prosjecnaOcjena { get; set; }
        public float kontrolaLopte { get; set; }
        public float dribljanje { get; set; }
        public float dodavanje { get; set; }
        public float kretanje { get; set; }
        public float brzina { get; set; }
        public float sut { get; set; }
        public float snaga { get; set; }
        public float markiranje { get; set; }
        public float klizeciStart { get; set; }
        public float skok { get; set; }
        public float odbrana { get; set; }
        public float prosjecnaOcjenaStavke { get; set; }
        public int brojUtakmica { get; set; }
        public int brojMinuta { get; set; }
    }
}
