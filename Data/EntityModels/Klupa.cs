using System;
using System.Collections.Generic;
using System.Text;

namespace Data.EntityModels
{
    public class Klupa
    {
        public int klupaID { get; set; }

        public Utakmica utakmica { get; set; }
        public int? utakmicaID { get; set; }

        public Igrac golmanKlupa { get; set; }
        public int golmanKlupaID { get; set; }

        public Igrac odbranaKlupa { get; set; }
        public int odbranaKlupaID { get; set; }

        public Igrac bekKlupa { get; set; }
        public int bekKlupaID { get; set; }

        public Igrac veznjakKlupa { get; set; }
        public int veznjakKlupaID { get; set; }

        public Igrac kriloKlupa { get; set; }
        public int kriloKlupaID { get; set; }

        public Igrac napadacKlupa { get; set; }
        public int napadacKlupaID { get; set; }
    }
}
