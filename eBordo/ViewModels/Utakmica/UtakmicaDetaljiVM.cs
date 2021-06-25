using Data.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBordo.ViewModels.Utakmica
{
    public class UtakmicaDetaljiVM
    {
        public class PozvaniIgraci
        {
            public PozvaniIgraci() { }
            public int igracID { get; set; }
            public string ime { get; set; }
            public string prezime { get; set; }
            public string slika { get; set; }
            public int brojDresa { get; set; }
            public float ocjena { get; set; }
        }
        public class Klupa
        {
            public Klupa() { }
            public int igracID { get; set; }
            public string prezime { get; set; }
            public int brojDresa { get; set; }
            public float ocjena { get; set; }
        }
        public int urakmicaID { get; set; }
        public string datumOdigravanja { get; set; }
        public string satnica { get; set; }
        public string sudija { get; set; }
        public string nazivStadiona { get; set; }
        public string stadionSlika { get; set; }
        public Vrijeme vrijeme { get; set; }
        public StatusUtakmice status { get; set; }
        public string zastavicaDrzava { get; set; }
        public string napomene { get; set; }
        public int brojUlaznica { get; set; }//!!!!
        public string vrstaTakmicenja { get; set; }
        public string slikaVrstaTakmicenja { get; set; }
        public string kapiten { get; set; }
        public string kapitenSlika { get; set; }
        public int kapitenID { get; set; }
        public string trener { get; set; }
        public string trenerSlika { get; set; }
        public string domacin { get; set; }
        public string domacinGrb { get; set; }
        public string garnituraDomacin { get; set; }
        public string gost { get; set; }
        public string gostGrb { get; set; }
        public string garnituraGost { get; set; }
        public float cijenaUlanice { get; set; }
        public int vrstaUtakmice { get; set; }
        public PozvaniIgraci golman { get; set; }
        public PozvaniIgraci lijeviBek { get; set; }
        public PozvaniIgraci prviStoper { get; set; }
        public PozvaniIgraci drugiStoper { get; set; }
        public PozvaniIgraci desniBek { get; set; }
        public PozvaniIgraci prviZadnjiVezni { get; set; }
        public PozvaniIgraci drugiZadnjiVezni { get; set; }
        public PozvaniIgraci prednjiVezni { get; set; }
        public PozvaniIgraci lijevoKrilo { get; set; }
        public PozvaniIgraci desnoKrilo { get; set; }
        public PozvaniIgraci napadac { get; set; }

        public Klupa golmanKlupa { get; set; }
        public Klupa odbranaKlupa { get; set; }
        public Klupa bekKlupa { get; set; }
        public Klupa veznjakKlupa { get; set; }
        public Klupa kriloKlupa { get; set; }
        public Klupa napdacKlupa { get; set; }
    }
}
