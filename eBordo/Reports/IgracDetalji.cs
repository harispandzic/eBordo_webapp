using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TmpReportDesigner
{
    public class IgracDetalji
    {
        public string imePrezime { get; set; }
        public string datumRodjenja { get; set; }
        public int godine { get; set; }
        public string adresaPrebivalista { get; set; }
        public string telefon { get; set; }
        public string emailAdresa { get; set; }
        public string drzavljanstvo { get; set; }
        public string gradRodjenja { get; set; }
        public string username { get; set; }
        public float visina { get; set; }
        public int tezina { get; set; }
        public int brojDresa { get; set; }
        public float trzisnaVrijednost { get; set; }
        public string reprezentativac { get; set; }
        public string datumPristupaKlubu { get; set; }
        public string dosadasnjiKlubovi { get; set; }
        public string pozicija { get; set; }
        public string boljaNoga { get; set; }
        public int brojNastupa { get; set; }
        public int odigraneMinute { get; set; }
        public int golovi { get; set; }
        public int asistencije { get; set; }
        public int zutiKartoni { get; set; }
        public int crveniKartoni { get; set; }
        public double prosjecnaOcjena { get; set; }
        public double kontrolaLopte { get; set; }
        public double dribljanje { get; set; }
        public double dodavanje { get; set; }
        public double kretanje { get; set; }
        public double brzina { get; set; }
        public double sut { get; set; }
        public double snaga { get; set; }
        public double markiranje { get; set; }
        public double klizeciStart { get; set; }
        public double skok { get; set; }
        public double odbrana { get; set; }
        public DateTime datumPocetka { get; set; }
        public DateTime datumZavrsetka { get; set; }
        public float iznosPlate { get; set; }
        public string napomene { get; set; }

        public static List<IgracDetalji> get()
        {
            return new List<IgracDetalji> { };
        }
    }
}