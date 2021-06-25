using System;
using System.Collections.Generic;
using System.Text;

namespace Data.EntityModels
{
    public class PrvaPostava
    {
        public int prvaPostavaID { get; set; }

        public Utakmica utakmica { get; set; }
        public int? utakmicaID { get; set; }

        //GOLMAN
        public Igrac golman { get; set; }
        public int golmanID { get; set; }

        //ODBRANA
        public Igrac lijeviBek { get; set; }
        public int lijeviBekID { get; set; }

        public Igrac prviStoper { get; set; }
        public int prviStoperID { get; set; }

        public Igrac drugiStoper { get; set; }
        public int drugiStoperID { get; set; }

        public Igrac desniBek { get; set; }
        public int desniBekID { get; set; }

        //VEZNI RED
        public Igrac prviZadnjiVezni { get; set; }
        public int prviZadnjiVezniID { get; set; }

        public Igrac drugiZadnjiVezni { get; set; }
        public int drugiZadnjiVezniID { get; set; }

        public Igrac prednjiVezni { get; set; }
        public int prednjiVezniID { get; set; }

        //NAPAD

        public Igrac lijevoKrilo { get; set; }
        public int lijevoKriloID { get; set; }

        public Igrac desnoKrilo { get; set; }
        public int desnoKriloID { get; set; }

        public Igrac napadac { get; set; }
        public int napadacID { get; set; }
    }
}
