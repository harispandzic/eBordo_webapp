using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.EntityModels
{
    public class UtakmicaOcjene
    {
        public int utakmicaOcjeneID { get; set; }

        public Igrac igrac { get; set; }
        public int igracID { get; set; }

        public Utakmica utakmica { get; set; }
        public int utakmicaID { get; set; }

        public int kontrolaLopte { get; set; }
        public int dribljanje { get; set; }
        public int dodavanje { get; set; }
        public int kretanje { get; set; }
        public int brzina { get; set; }
        public int sut { get; set; }
        public int snaga { get; set; }
        public int markiranje { get; set; }
        public int klizeciStart { get; set; }
        public int skok { get; set; }
        public int odbrana { get; set; }
        public float prosjecnaOcjena { get; set; }
    }
}
