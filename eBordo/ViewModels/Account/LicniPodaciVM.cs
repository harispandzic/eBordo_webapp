using Data.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBordo.ViewModels.Account
{
    public class LicniPodaciVM
    {
        public int igracID { get; set; }
        public string ime { get; set; }
        public string prezime { get; set; }
        public DateTime datumRodjenja { get; set; }
        public int godine { get; set; }
        public string adresaPrebivalista { get; set; }
        public string telefon { get; set; }
        public string emailAdresa { get; set; }
        public string drzavljanstvo { get; set; }
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
        public string pozicija { get; set; }
        public string boljaNoga { get; set; }
        public Status statusIgraca { get; set; }

        public DateTime datumPocetka { get; set; }
        public DateTime datumZavrsetka { get; set; }
        public float iznosPlate { get; set; }
        public string napomene { get; set; }

    }
}
