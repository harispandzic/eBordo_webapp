using System;
using System.Collections.Generic;
using System.Text;

namespace Data.EntityModels
{
    public class Trener
    {
        public int trenerID { get; set; }
        public string imePrezime { get; set; }
        public DateTime prvaZvanicnaUtakmica { get; set; }
        public int godineIskustva { get; set; }
        public DateTime datumPristupaKlubu { get; set; }
        public string slika { get; set; }
        public string dosadasnjiKlubovi { get; set; }
        public string tranfermarktAcc { get; set; }
        public float trzisnaVrijednost { get; set; }

        public TrenerStatistika trenerStatistika { get; set; }
        public int trenerStatistikaID { get; set; }

        public Ugovor ugovor { get; set; }
        public int ugovorID { get; set; }

        public Status status { get; set; }

        public Korisnik korisnik { get; set; }
        public string korisnikID { get; set; }

        public Formacija preferiranaFormacija { get; set; }
        public TrenerskaLicenca trenerskaLicenca { get; set; }
    }
}
