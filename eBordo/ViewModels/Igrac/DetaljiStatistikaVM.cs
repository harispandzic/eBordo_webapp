using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBordo.ViewModels.Igrac
{
    public class DetaljiStatistikaVM
    {
        public int igracID { get; set; }
        public string ime { get; set; }
        public string prezime { get; set; }
        public string slika { get; set; }
        public string pozicija { get; set; }
        public int brojDresa { get; set; }
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
        public string drzavljanstvo { get; set; }
        public string zastava { get; set; }
        public string gradRodjenja { get; set; }
        public string zastavaZaGrad { get; set; }
    }
}
