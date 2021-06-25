using Data.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBordo.ViewModels.Igrac
{
    public class IgracDetaljiVM
    {
        public int igracID { get; set; }
        public string ime{ get; set; }
        public string prezime { get; set; }
        public DateTime datumRodjenja { get; set; }
        public int godine { get; set; }
        public string adresaPrebivalista { get; set; }
        public string telefon{ get; set; }
        public string emailAdresa { get; set; }
        public string drzavljanstvo{ get; set; }
        public string zastava { get; set; }
        public string gradRodjenja { get; set; }
        public string zastavaZaGrad { get; set; }
        public string username { get; set; }
        public string transferMarktAcc { get; set; }

        public float visina { get; set; }
        public int tezina { get; set; }
        public int brojDresa { get; set; }
        public float trzisnaVrijednost { get; set; }
        public bool reprezentativac { get; set; }
        public string slika { get; set; }
        public DateTime datumPristupaKlubu { get; set; }
        public string dosadasnjiKlubovi { get; set; }
        public string pozicija{ get; set; }
        public string boljaNoga{ get; set; }
        public Status statusIgraca{ get; set; }

        public double brojNastupa{ get; set; }
        public double odigraneMinute{ get; set; }
        public double golovi{ get; set; }
        public double asistencije{ get; set; }
        public double zutiKartoni{ get; set; }
        public double crveniKartoni{ get; set; }

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
        public double prosjecnaOcjenaStavke { get; set; }

        public int brojUtakmica { get; set; }
        public int brojMinuta { get; set; }

        public DateTime datumPocetka { get; set; }
        public DateTime datumZavrsetka { get; set; }
        public float iznosPlate { get; set; }
        public string napomene { get; set; }
    }
}
