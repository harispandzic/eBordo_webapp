using Data.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBordo.ViewModels.Trener
{
    public class TrenerDetaljiVM
    {
        public int trenerID { get; set; }
        public string ime { get; set; }
        public string prezime { get; set; }
        public string datumRodjenja { get; set; }
        public int godine { get; set; }
        public string adresaPrebivalista { get; set; }
        public string telefon { get; set; }
        public string emailAdresa { get; set; }
        public string drzavljanstvo { get; set; }
        public string zastava { get; set; }
        public string gradRodjenja { get; set; }
        public string zastavaZaGrad { get; set; }
        public string username { get; set; }

        public string prvaZvanicnaUtakmica { get; set; }
        public int godineIskustva { get; set; }
        public string slika { get; set; }
        public string dosadasnjiKlubovi { get; set; }
        public string transferMartkAcc { get; set; }
        public float trzisnaVrijednost { get; set; }
        public Status statusIgraca { get; set; }

        public int brojOdigranihUtakmica { get; set; }
        public int brojUtakmica { get; set; }
        public int brojPobjeda { get; set; }
        public int brojNerjesenih { get; set; }
        public int brojPoraza { get; set; }

        public DateTime datumPocetka { get; set; }
        public DateTime datumZavrsetka { get; set; }
        public float iznosPlate { get; set; }
        public string napomene { get; set; }

        public Formacija preferiranaFormacija { get; set; }
        public TrenerskaLicenca trenerskaLicenca { get; set; }
    }
}
