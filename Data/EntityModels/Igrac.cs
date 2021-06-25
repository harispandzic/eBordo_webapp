using System;
using System.Collections.Generic;
using System.Text;

namespace Data.EntityModels
{
    public class Igrac
    {
        public int igracID { get; set; }

        public float visina { get; set; }
        public int tezina { get; set; }
        public int brojDresa { get; set; }
        public float trzisnaVrijednost { get; set; }
        public bool reprezentativac { get; set; }
        public string slika { get; set; }
        public DateTime datumPristupaKlubu { get; set; }
        public string dosadasnjiKlubovi { get; set; }
        public string trensferMarktAcc { get; set; }
        public int godine { get; set; }

        public Pozicija pozicija { get; set; }

        public BoljaNoga boljaNoga { get; set; }
        
        public IgracStatistika igracStatistika { get; set; }
        public int igracStatistikaID { get; set; }

        public IgracSkills igracSkills { get; set; }
        public int igracSkillsID { get; set; }

        public Ugovor ugovor { get; set; }
        public int ugovorID { get; set; }

        public Status status { get; set; }

        public Korisnik korisnik { get; set; }
        public string korisnikID { get; set; }
    }
}
