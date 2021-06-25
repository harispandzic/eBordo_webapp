using System;
using System.Collections.Generic;
using System.Text;

namespace Data.EntityModels
{
    public class Utakmica
    {
        public int utakmicaID { get; set; }
        public DateTime datumOdigravanja { get; set; }
        public string satnica { get; set; }
        public string sudija { get; set; }

        public Stadion stadion { get; set; }
        public int stadionID { get; set; }
        
        public string napomene { get; set; }
        public int brojUlaznica { get; set; }
        public float cijenaUlaznice { get; set; }

        public VrstaTakmicenja vrstaTakmicenja { get; set; }
        public int vrstaTakmicenjaID { get; set; }

        public Igrac kapiten { get; set; }
        public int kapitenID { get; set; }

        public Trener trener { get; set; }
        public int trenerID { get; set; }

        public Klub domacin { get; set; }
        public int domacinID { get; set; }

        public Klub gost { get; set; }
        public int gostID { get; set; }

        public GarnituraDresova garnituraDresa { get; set; }
        public int garnituraDresaID { get; set; }

        public VrstaUtakmice vrstaUtakmice { get; set; }

        public StatusUtakmice statusUtakmice { get; set; }

        public Vrijeme PredvidjenoVrijeme { get; set; }

        public PrvaPostava prvaPostava { get; set; }
        public int prvaPostavaID { get; set; }

        public Klupa klupa { get; set; }
        public int klupaID { get; set; }
    }
}
