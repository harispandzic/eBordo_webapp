using System;
using System.Collections.Generic;
using System.Text;

namespace Data.EntityModels
{
    public class UtakmicaNastupi
    {
        public int utakmicaNastupiID { get; set; }

        public Igrac igrac { get; set; }
        public int igracID { get; set; }

        public Utakmica utakmica { get; set; }
        public int utakmicaID { get; set; }

        public int minute { get; set; }
        public int golovi { get; set; }
        public int asistencije { get; set; }
        public int zutiKartoni { get; set; }
        public int crveniKartoni { get; set; }
        public int ocjena { get; set; }
        public string komentar { get; set; }
    }
}
