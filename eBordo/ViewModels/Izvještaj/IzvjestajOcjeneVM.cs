using Data.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBordo.ViewModels.Izvještaj
{
    public class IzvjestajOcjeneVM
    {
        public int utakmicaID { get; set; }

        public string slikaVrstaTakmicenja { get; set; }
        public string domacin { get; set; }
        public string domacinGrb { get; set; }
        public string gost { get; set; }
        public string gostGrb { get; set; }
        public int goloviGostiju { get; set; }
        public int goloviDomacih { get; set; }
        public Rezultat rezultat { get; set; }

        public int igracID { get; set; }
        public string imePrezime { get; set; }
        public string slika { get; set; }
        public int brojDresa { get; set; }
        public string pozicija { get; set; }
        public string komentar { get; set; }
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
